using PriceTargets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceTargets.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var output = GetOutput();

            var stockMarketToken = await GetStockMarketTokenAsync();
            var stockMarket = GetStockMarket(stockMarketToken, output);

            var storage = GetStorage();


            var financeDataProviders = await GetFinanceDataProvidersAsync();

            var financeDataManager = GetFinanceDataManager(financeDataProviders);

            var measure = GetMeasure();
            var publisher = GetPublisher();

            var tickers = await stockMarket.GetTickersAsync();

            var n = tickers.Length;

            var stocks = new List<Core.Models.Publisher.StockInfo>();

            for (int i = 0; i< n; i++)
            {
                var ticker = tickers[i];

                output.Publish($"{ticker} {i}/{n}");

                try
                {
                    var companyInfo = await financeDataManager.GetCompanyInfoAsync(ticker);

                    var priceExpectationLevel = measure.GetPriceExpectationsLevel(
                        companyInfo.CurrentPrice,
                        companyInfo.PriceTarget);

                    var meanTrend = measure.GetTrendExpectationsLevel(companyInfo.RecommendationTrend);

                    var stock = publisher.CreateStockInfo(
                        ticker,
                        companyInfo.CurrentPrice,
                        companyInfo.PriceTarget,
                        priceExpectationLevel,
                        companyInfo.RecommendationTrend,
                        meanTrend);

                    stocks.Add(stock);

                    //  await storage.SavePriceTargetAsync(ticker, targetPrice);
                    //  await storage.SaveCurrentPriceAsync(ticker, currentPrice);
                    //  foreach (var r in recommendationTrends)
                    //  {
                    //      await storage.SaveRecommendationTrendAsync(ticker, r);
                    //  }
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    output.Publish($"{ticker} {i}/{n} else one try");
                    await Task.Delay(2000);
                    i--;
                }
            }

            var report = publisher.CreateReport(stocks.ToArray());
            var formattedReport = publisher.FormatReport(report);
            await storage.SaveReportAsync(formattedReport);

            output.Publish($"report saved");
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

        private static async Task<string> GetFinanceDataProviderToken(Core.Domain.FinanceDataProviders provider)
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
                FinanceDataProviders.Finnhub => new Core.FinanceDataProvider.Finnhub.FinanceDataProvider(token),
                FinanceDataProviders.TipRanks => new Core.FinanceDataProvider.TipRanks.FinanceDataProvider(),
                _ => throw new NotSupportedException(),
            };
        }

        private static async Task<IFinanceDataProvider[]> GetFinanceDataProvidersAsync()
        {
            var financeDataProviderToken = await GetFinanceDataProviderToken(Core.Domain.FinanceDataProviders.Finnhub);
            var financeDataProviderFinnhub = GetFinanceDataProvider(FinanceDataProviders.Finnhub, financeDataProviderToken);

            var financeDataProviderTipRanks = GetFinanceDataProvider(FinanceDataProviders.TipRanks, null);

            return new[] { financeDataProviderFinnhub, financeDataProviderTipRanks };
        }

       


        private static IFinanceDataManager GetFinanceDataManager(IFinanceDataProvider[] providers)
        {
            return new Core.FinanceDataManager.Main.FinanceDataManager(providers);
        }

        private static IMeasure GetMeasure()
        {
            return new PriceTargets.Core.Measure.Calculator.Measure();
        }

        private static IPublisher GetPublisher()
        {
            return new PriceTargets.Core.Publisher.Json.Publisher();
        }


    }
}
