using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NDesk.Options;
using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models;
using System.Text.Json;

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

            var output = GetOutput();

            var config = await GetTokensAsync();
            if (config == null)
            {
                throw new ArgumentNullException();
            }

            var tinkoffToken = config.Tinkoff.Tokens?.FirstOrDefault();
            if (tinkoffToken == null)
            {
                throw new ArgumentNullException();
            }

            var stockMarket = GetStockMarket(tinkoffToken, output);

            var fileStorage = GetFileStorage();
            var dbStorage = GetDbStorage(config);

            var financeDataProviders = GetFinanceDataProviders(output, config);
            var financeDataManager = GetFinanceDataManager(output, financeDataProviders, dbStorage);

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

            
            if (tickersLimit < 0)
            {
                output.Publish($"Fill data");
                await financeDataManager.FillData(tickers);
            }

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
                            companyInfo.CompanyName,
                            companyInfo.Logo,
                            companyInfo.CurrentPrice,
                            companyInfo.PriceTarget,
                            companyInfo.Recommendations);

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

            await fileStorage.CreateReportAsync(stockInfos.ToArray());
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

        private static async Task<Models.Config> GetTokensAsync()
        {
            var fileName = "config.json";

            if (!System.IO.File.Exists(fileName))
            {
                Console.WriteLine("Not found file with token!");
                return null;
            }

            var configString = await System.IO.File.ReadAllTextAsync(fileName);
            var config = JsonSerializer.Deserialize<Models.Config>(configString);
            return config;
        }


        private static IStockMarket GetStockMarket(string token, IOutput output)
        {
            return new Core.StockMarket.Tinkoff.TinkoffStockMarket(token, output);
        }

        private static IStorage GetFileStorage()
        {
            return new Core.Storage.File.Storage();
        }

        private static IStorage GetDbStorage(Models.Config config)
        {
            var postgres = config.Postgres;
            return new Core.Storage.Postgres.Storage(postgres.DbConnection);
        }

        private static IOutput GetOutput()
        {
            return new Core.Output.Console.Output();
        }

        private static IFinanceDataProvider[] GetFinanceDataProviders(IOutput output, Models.Config config)
        {
            var finscreenerDb = config.Finscreener.DbConnection;
            var finscreenerDelay = config.Finscreener.Delay;
            if (finscreenerDb == null || finscreenerDelay== null)
            {
                throw new ArgumentNullException();
            }
            var finscreener = new Core.FinanceDataProvider.Finscreener.FinanceDataProvider(output, finscreenerDb, finscreenerDelay.Value);

            var finnhubToken = config.Finnhub.Tokens?.FirstOrDefault();
            var finnhubDelay = config.Finnhub.Delay;
            if (finnhubToken == null || finnhubDelay == null)
            {
                throw new ArgumentNullException();
            }
            var finnhub = new Core.FinanceDataProvider.Finnhub.FinanceDataProvider(finnhubToken, finnhubDelay.Value);

            var yahooFinanceDelay = config.YahooFinance.Delay;
            if (yahooFinanceDelay == null)
            {
                throw new ArgumentNullException();
            }
            var yahooFinance = new Core.FinanceDataProvider.YahooFinance.FinanceDataProvider(yahooFinanceDelay.Value);

            return new IFinanceDataProvider[] {
                finscreener,
                finnhub,
                yahooFinance,
            };
        }

        private static IFinanceDataManager GetFinanceDataManager(IOutput output, IFinanceDataProvider[] providers, IStorage storage)
        {
            return new Contracts.FinanceDataManager.Main.FinanceDataManager(output, providers, storage);
        }

        private static IStockInfoCache GetStockInfoCache()
        {
            return new HamstersRocket.Core.StockInfoCache.File.StockInfoCache();
        }
    }
}