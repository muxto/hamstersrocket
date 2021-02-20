using PriceTargets.Core.Models;
using PriceTargets.Core.Models.FinanceDataProvider;
using PriceTargets.Core.Models.Measure;
using PriceTargets.Core.Models.Publisher;
using System;
using System.Text.Json;

namespace PriceTargets.Core.Publisher.Json
{
    public class Publisher : Domain.IPublisher
    {
        public StockInfo CreatePublishItem(string ticker, CurrentPrice currentPrice, PriceTarget priceTarget, PriceExpectationLevel priceExpectationLevel, RecommendationTrend recommendationTrend, decimal meanTrend)
        {
            var stock = new StockInfo()
            {
                Ticker = ticker,
                CurrentPrice = currentPrice.C,

                TargetPriceHigh = priceTarget.TargetHigh,
                TargetPriceHighPercent = priceExpectationLevel.TargetPriceHighPercent,

                TargetPriceMean = priceTarget.TargetMean,
                TargetPriceMeanPercent = priceExpectationLevel.TargetPriceMeanPercent,
                TargetPriceMedian = priceTarget.TargetMedian,
                TargetPriceMedianPercent = priceExpectationLevel.TargetPriceMedianPercent,
                TargetPriceLow = priceTarget.TargetLow,
                TargetPriceLowPercent = priceExpectationLevel.TargetPriceLowPercent,

                StrongBuy = recommendationTrend.StrongBuy,
                Buy = recommendationTrend.Buy,
                Hold = recommendationTrend.Hold,
                Sell = recommendationTrend.Sell,
                StrongSell = recommendationTrend.StrongSell,
                RecommendationTrend = meanTrend,
            };
            return stock;
        }

        public Report CreateReport(StockInfo[] stocks)
        {
            var report = new Report()
            {
                UpdateDate = DateTime.Now,
                Stocks = stocks
            };

            return report;
        }

        public string FormatReport(Report report)
        {
            var json =  JsonSerializer.Serialize(report);
            return json;
        }
    }
}
