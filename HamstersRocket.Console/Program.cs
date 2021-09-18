using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NDesk.Options;
using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models;

namespace HamstersRocket.ConsoleApp
{
    class Program
    {
        class Options
        {
            public bool ShowHelp = false;

            public bool HasTickersLimit => TickersLimit > 0;
            public int TickersLimit = -1;

            public bool NeedToGetHistoricCandles => GetHistoricCandlesMonthAgo > 0;
            public int GetHistoricCandlesMonthAgo = -1;
        }

        static async Task Main(string[] args)
        {
            //args = new string[] { "-historic_candles_month", "6" };

           // args = new string[] { "-historic_candles_month", "6" };

            var options = new Options();

            var p = new OptionSet() {
                { "h|?|help",  v => { options.ShowHelp = v != null; } },
                { "l|limit=",   v => { int.TryParse(v, out options.TickersLimit); } },
                { "historic_candles_month=",   v => { int.TryParse(v, out options.GetHistoricCandlesMonthAgo); } },
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("greet: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.");
                return;
            }

            if (options.ShowHelp)
            {
                ShowHelp(p);
                return;
            }

            await Run(options);
        }

        static async Task Run (Options options)
        {
            var output = GetOutput();

            output.Publish($"TickersLimit {options.TickersLimit}");
            output.Publish($"GetHistoricCandles {options.GetHistoricCandlesMonthAgo}");

            var stockMarketToken = await GetStockMarketTokenAsync();
            if (stockMarketToken == null)
            {
                throw new ArgumentNullException();
            }

            var stockMarket = GetStockMarket(stockMarketToken, output);

            var storageFile = GetStorageFile();

            var financeDataProviders = await GetFinanceDataProvidersAsync();
            var financeDataManager = GetFinanceDataManager(financeDataProviders);

            var publisher = GetPublisher();
            var stockInfoCache = GetStockInfoCache();

            var tickers = await stockMarket.GetTickersAsync();

            var n = tickers.Length;

            // TODO test sqlite
            if (options.NeedToGetHistoricCandles)
            {
                var req = 0;

                var storageDatabase2 = GetStorageDatabase();



                for (int i = 0; i < n; i++)
                {
                    if (i == options.TickersLimit)
                    {
                        break;
                    }

                    var ticker = tickers[i];

                    output.Publish($"{ticker} {i}/{n}");

                    if (req > 100)
                    {
                        output.Publish($"wait");
                        await Task.Delay(360 * 1000);
                        req = 0;
                    }

                    var candles = await stockMarket.GetHistoricCandlesAsync(ticker, options.GetHistoricCandlesMonthAgo);
                    req += 2;

                    if (candles == null )
                    {
                        continue;
                    }

                    foreach (var c in candles)
                    {
                        var rep = new HamstersRocket.Contracts.Models.Publisher.Report()
                        {
                            UpdateDate = c.Date,
                            Stocks = new StockInfo[]
                            {
                                new StockInfo()
                                {
                                    Ticker = ticker,
                                    PriceHigh = c.Price.H,
                                    PriceLow = c.Price.L
                                }
                            }
                        };

                        storageDatabase2.SaveReport(rep);
                    }



                    //await Task.Delay(2000);
                }

               

                output.Publish($"historic candles saved");

                return;
            }


            var stockInfos = new List<StockInfo>();

            var stockInfosFromCache = await stockInfoCache.GetAllAsync();

            var tryCount = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == options.TickersLimit)
                {
                    break;
                }

                var ticker = tickers[i];

                output.Publish($"{ticker} {i}/{n}");

                try
                {
                    var stockInfo = stockInfosFromCache.FirstOrDefault(x => x.Ticker == ticker);
                    if (stockInfo == null)
                    {
                        var companyInfo = await financeDataManager.GetCompanyInfoAsync(ticker);

                        stockInfo = StockInfo.Create(
                            ticker,
                            companyInfo.Industry,
                            companyInfo.CurrentPrice,
                            companyInfo.PriceTarget,
                            companyInfo.RecommendationTrend);

                        await stockInfoCache.SaveAsync(stockInfo);
                    }
                    stockInfos.Add(stockInfo);
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    if (tryCount > 3)
                    {
                        return;
                    }

                    output.Publish($"{ticker} {i}/{n} else one try");
                    await Task.Delay(2000);
                    i--;
                    tryCount++;
                }
                tryCount = 0;
            }

            var report = publisher.CreateReport(stockInfos.ToArray());
            await storageFile.SaveReportAsync(report);

            var storageDatabase = GetStorageDatabase();
            storageDatabase.SaveReport(report);

            await stockInfoCache.ClearAsync();

            output.Publish($"report saved");
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: greet [OPTIONS]+ message");
            Console.WriteLine("Greet a list of individuals with an optional message.");
            Console.WriteLine("If no message is specified, a generic greeting is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        private static async Task<string> GetStockMarketTokenAsync()
        {
            var tokenFile = "stockmarkettoken.txt";
            if (!System.IO.File.Exists(tokenFile))
            {
                Console.WriteLine("Not found file with token!");
                return null;
            }

            var token = await System.IO.File.ReadAllTextAsync(tokenFile);
            return token;
        }

        private static async Task<string> GetFinanceDataProviderToken(Contracts.Domain.FinanceDataProviders provider)
        {
            if (provider == FinanceDataProviders.Finnhub)
            {
                var tokenFile = "financedataprovidertoken.txt";
                if (!System.IO.File.Exists(tokenFile))
                {
                    Console.WriteLine("Not found file with token!");
                    return null;
                }

                var token = await System.IO.File.ReadAllTextAsync(tokenFile);
                return token;
            }
            return null;
        }

        private static IStockMarket GetStockMarket(string token, IOutput output)
        {
            return new Core.StockMarket.Tinkoff.TinkoffStockMarket(token, output);
        }

        private static IStorage GetStorageFile()
        {
            return new Core.Storage.File.StorageFile();
        }

        private static IStorage GetStorageDatabase()
        {
            return new Core.Storage.Sqlite.Storage("");
        }

        private static IOutput GetOutput()
        {
            return new Core.Output.Console.Output();
        }

        private static IFinanceDataProvider GetFinanceDataProvider(FinanceDataProviders providerName, string token)
        {
            return providerName switch
            {
                FinanceDataProviders.Finnhub => new Core.FinanceDataProvider.Finnhub.FinanceDataProvider(token),
                FinanceDataProviders.TipRanks => new Core.FinanceDataProvider.TipRanks.FinanceDataProvider(),
                FinanceDataProviders.SeekingAlpha => new Core.FinanceDataProvider.SeekingAlpha.FinanceDataProvider(),
                _ => throw new NotSupportedException(),
            };
        }

        private static async Task<IFinanceDataProvider[]> GetFinanceDataProvidersAsync()
        {
            var financeDataProviderToken = await GetFinanceDataProviderToken(FinanceDataProviders.Finnhub);
            if (financeDataProviderToken == null)
            {
                throw new ArgumentNullException();
            }

            var financeDataProviderFinnhub = GetFinanceDataProvider(FinanceDataProviders.Finnhub, financeDataProviderToken);

            var financeDataProviderTipRanks = GetFinanceDataProvider(FinanceDataProviders.TipRanks, null);

            var financeDataProviderSeekingAlpha = GetFinanceDataProvider(FinanceDataProviders.SeekingAlpha, null);

            return new[] {
                financeDataProviderFinnhub,
                financeDataProviderTipRanks,
                financeDataProviderSeekingAlpha
            };
        }

        private static IFinanceDataManager GetFinanceDataManager(IFinanceDataProvider[] providers)
        {
            return new Contracts.FinanceDataManager.Main.FinanceDataManager(providers);
        }

        private static IPublisher GetPublisher()
        {
            return new HamstersRocket.Core.Publisher.Json.Publisher();
        }

        private static IStockInfoCache GetStockInfoCache()
        {
            return new HamstersRocket.Core.StockInfoCache.File.StockInfoCache();
        }
    }
}