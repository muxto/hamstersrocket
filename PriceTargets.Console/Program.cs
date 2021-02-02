using PriceTargets.Core.Domain;
using System;
using System.Text.Json;
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


            //  if (args != null && args.Length > 1)
            //  {
            //      var action = args[0];
            //
            //      if (action == "gethistoricprice" && args.Length == 2)
            //      {
            //          var ticker = args[1];
            //          await stockMarket.GetHistoricPrice(ticker);
            //          return;
            //      }
            //  }




            var tickers = await stockMarket.GetTickers();

            var n = tickers.Length;
            for (int i = 0; i< n; i++)
            {
                var ticker = tickers[i];

                output.Publish($"{ticker} {i}/{n}");

                try
                {

                    var targetPrice = await financeDataProvider.GetPriceTargetAsync(ticker);
                    await Task.Delay(1000);
                    var currentPrice = await financeDataProvider.GetCurrentPriceAsync(ticker);
                    await Task.Delay(1000);
                    await storage.SavePriceTarget(ticker, currentPrice, targetPrice);
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    output.Publish($"{ticker} {i}/{n} else one try");
                    await Task.Delay(2000);
                    i--;
                }
            }


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
            var fileName = $"targetPrice_{ticks}.txt";
            return new PriceTargets.Core.Storage.File.StorageFile(fileName);
        }

        private static IOutput GetOutput()
        {
            return new PriceTargets.Core.Output.Console.Output();
        }

        private static IFinanceDataProvider GetFinanceDataProvider(string token)
        {
            return new PriceTargets.Core.FinanceDataProvider.Finnhub.FinanceDataProvider(token);
        }
    }
}
