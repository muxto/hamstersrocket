using PriceTargets.Core.Domain;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceTargets.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var output = GetOutput();

            var stockMarketToken = await GetStockMarketToken();
            var stockMarket = GetStockMarket(stockMarketToken, output);

            var storage = GetStorage();

            var financeDataProviderToken = await GetFinanceDataProviderToken();
            var financeDataProvider = GetFinanceDataProvider(financeDataProviderToken);

            var measure = GetMeasure();
            var publisher = GetPublisher();

            var tickers = await stockMarket.GetTickers();

            var n = tickers.Length;

            var stocks = new List<Core.Models.Publisher.StockInfo>();

            for (int i = 0; i< n; i++)
            {
                var ticker = tickers[i];

                output.Publish($"{ticker} {i}/{n}");

                try
                {
                    var currentPrice = await financeDataProvider.GetCurrentPriceAsync(ticker);
                    await Task.Delay(1000);
                    
                    var targetPrice = await financeDataProvider.GetPriceTargetAsync(ticker);
                    await Task.Delay(1000);
                    
                    var recommendationTrends = await financeDataProvider.GetRecommendationTrendsAsync(ticker);
                    await Task.Delay(1000);

                    var priceExpectationLevel = measure.GetPriceExpectationsLevel(currentPrice, targetPrice);
                    var recommendationTrend = recommendationTrends
                        .OrderBy(x => x.Period)
                        .FirstOrDefault() ?? new Core.Models.FinanceDataProvider.RecommendationTrend();

                    var meanTrend =  measure.GetTrendExpectationsLevel(recommendationTrend);

                    var stock = publisher.CreateStockInfo(ticker, currentPrice, targetPrice, priceExpectationLevel, recommendationTrend, meanTrend);
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
                if (i == 5) break;
            }

            var report = publisher.CreateReport(stocks.ToArray());
            var formattedReport = publisher.FormatReport(report);
            await storage.SaveReportAsync(formattedReport);

            output.Publish($"report saved");
        }

        private static async Task<string> GetStockMarketToken()
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

        private static async Task<string> GetFinanceDataProviderToken()
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

        private static IStockMarket GetStockMarket(string token, IOutput output)
        {
            return new PriceTargets.Core.StockMarket.Tinkoff.TinkoffStockMarket(token, output);
        }

        private static IStorage GetStorage()
        {
            var ticks = DateTime.Now.Ticks;
            return new PriceTargets.Core.Storage.File.StorageFile();
        }

        private static IOutput GetOutput()
        {
            return new PriceTargets.Core.Output.Console.Output();
        }

        private static IFinanceDataProvider GetFinanceDataProvider(string token)
        {
            return new PriceTargets.Core.FinanceDataProvider.Finnhub.FinanceDataProvider(token);
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
