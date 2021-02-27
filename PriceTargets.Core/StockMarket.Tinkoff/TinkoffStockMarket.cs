using HamstersRocket.Contracts.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;

namespace HamstersRocket.Core.StockMarket.Tinkoff
{
    public class TinkoffStockMarket : IStockMarket
    {
        private string Token { get; set; }
        private Context Context { get; set; }

        private Portfolio Portfolio { get; set; }

        private IOutput Output { get; set; }


        public TinkoffStockMarket(string token, IOutput output)
        {
            Token = token;
            Output = output;

            var connection = ConnectionFactory.GetConnection(token);
            Context = connection.Context;
        }

        public async Task Refresh()
        {
            Portfolio = await Context.PortfolioAsync();
        }

        public Portfolio.Position GetPortfolioPositionByTicker(string ticker)
        {
            return Portfolio.Positions.FirstOrDefault(x => x.Ticker == ticker);
        }

        public async Task BuyStonks(string figi, decimal buyPrice, int count)
        {
            Output.Publish($"Покупка {count} акций");
            var o = await Context.PlaceLimitOrderAsync(new LimitOrder(figi, count, OperationType.Buy, buyPrice));
            Output.Publish($"Создана заявка {o.Operation} {o.OrderId} {o.Status} {o.RejectReason}");
        }

        public async Task SellStonks(string figi, decimal sellPrice, int count)
        {
            Output.Publish($"Продажа {count} акции");
            var o = await Context.PlaceLimitOrderAsync(new LimitOrder(figi, count, OperationType.Sell, sellPrice));
            Output.Publish($"Создана заявка {o.Operation} {o.OrderId} {o.Status} {o.RejectReason}");
        }

        public async Task<MarketInstrument> GetInstrument(string ticker)
        {
            //  if (!string.IsNullOrEmpty(paperOptions.Figi))
            //  {
            //      var stonk = await context.MarketSearchByFigiAsync(paperOptions.Figi);
            //      if (stonk != null)
            //      {
            //          return stonk;
            //      }
            //  }

            var instruments = await Context.MarketSearchByTickerAsync(ticker);

            if (instruments?.Instruments?.Count == 0)
            {
                return null;
            }

            return instruments.Instruments[0];
        }

        public async Task<Orderbook> GetOrderbook(string figi)
        {
            return await Context.MarketOrderbookAsync(figi, 5);
        }

        public async Task<List<Operation>> GetOperations(DateTime from, DateTime to, string figi)
        {
            return await Context.OperationsAsync(from, to, figi);
        }


        public async Task<CandleList> GetCandles(DateTime from, DateTime to, string figi)
        {
            return await Context.MarketCandlesAsync(figi, from, to, CandleInterval.FiveMinutes);
        }

        public async Task GetHistoricPrice(string ticker)
        {
            await Refresh();

            var position = GetPortfolioPositionByTicker(ticker);

            var start = DateTime.Parse("2020.01.01");
            var end = start.AddDays(1);

            var req = 0;

            while (end < DateTime.Now)
            {
                if (req > 50)
                {
                    Output.Publish($"wait");
                    await Task.Delay(360 * 1000);
                    req = 0;
                }

                Output.Publish($"{start.ToShortDateString()}");
                var candles = await Context.MarketCandlesAsync(position.Figi, start, end, CandleInterval.FiveMinutes);

                req++;

                foreach (var candle in candles.Candles)
                {
                    var str = $"{candle.Open}\t{candle.Close}\t{candle.High}\t{candle.Low}\t{candle.Volume}\t{candle.Time}\t{candle.Interval}\t{candle.Figi}{Environment.NewLine}";
                    System.IO.File.AppendAllText("candles.txt", str);
                }


                start = end;
                end = start.AddDays(1);
            }
            Output.Publish("Done");
        }

        public async Task<string[]> GetTickersAsync()
        {
            var stocks = await Context.MarketStocksAsync();

            var tickers = stocks.Instruments.Select(x => x.Ticker).ToArray();

            return tickers;
        }
    }

}