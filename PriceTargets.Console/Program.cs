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
        static async Task Main(string[] args)
        {
            var tickersLimit = -1;
            var showHelp = false;

            var p = new OptionSet() {
                { "h|?|help",  v => { showHelp = v != null; } },
                { "l|limit=",   v => { int.TryParse(v, out tickersLimit); } },
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

            if (showHelp)
            {
                ShowHelp(p);
                return;
            }

            await Run(tickersLimit);
        }

        static async Task Run (int tickersLimit)
        {
            var output = GetOutput();

            var stockMarketToken = await GetStockMarketTokenAsync();
            if (stockMarketToken == null)
            {
                throw new ArgumentNullException();
            }

            var stockMarket = GetStockMarket(stockMarketToken, output);

            var storage = GetStorage();

            var financeDataProviders = await GetFinanceDataProvidersAsync();
            var financeDataManager = GetFinanceDataManager(financeDataProviders);

            var publisher = GetPublisher();
            var stockInfoCache = GetStockInfoCache();

            var tickers = await stockMarket.GetTickersAsync();

            var n = tickers.Length;

            var stockInfos = new List<StockInfo>();

            var stockInfosFromCache = await stockInfoCache.GetAllAsync();

            if (tickersLimit > 0)
            {
                output.Publish($"tickers limit = {tickersLimit}");
            }

            var tryCount = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == tickersLimit)
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
            var formattedReport = publisher.FormatReport(report);
            await storage.SaveReportAsync(formattedReport);
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

        private static IStorage GetStorage()
        {
            var ticks = DateTime.Now.Ticks;
            return new Core.Storage.File.StorageFile();
        }

        private static IOutput GetOutput()
        {
            return new Core.Output.Console.Output();
        }

        private static IFinanceDataProvider GetFinanceDataProvider(FinanceDataProviders providerName, string token)
        {
            return providerName switch
            {
                FinanceDataProviders.Finnhub => new Contracts.FinanceDataProvider.Finnhub.FinanceDataProvider(token),
                FinanceDataProviders.TipRanks => new Contracts.FinanceDataProvider.TipRanks.FinanceDataProvider(),
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

            return new[] { financeDataProviderFinnhub, financeDataProviderTipRanks };
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