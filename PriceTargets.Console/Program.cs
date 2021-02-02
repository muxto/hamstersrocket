using PriceTargets.Core.Domain;
using System;
using System.Threading.Tasks;

namespace PriceTargets.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // args = new string[]
            // {
            //     "gethistoricprice",
            //     "GTHX"
            // };

            var token = await GetToken();

            var output = GetOutput();
            var stockMarket = GetStockMarket(token, output);
            var storage = GetStorage();

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

            int x = 9;

          
        }

        private static async Task<string> GetToken()
        {
            var tokenFile = "token.txt";
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
            return new PriceTargets.Core.Storage.File.StorageFile("avgPrice.txt");
        }

        private static IOutput GetOutput()
        {
            return new PriceTargets.Core. Output.Console.Output();
        }



    }
}
