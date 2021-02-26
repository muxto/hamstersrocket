using PriceTargets.Core.Models.FinanceDataProvider;
using PriceTargets.Core.Models.Measure;

namespace PriceTargets.Core.Models
{
    public class StockInfo
    {
        public string Ticker { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal TargetPriceHigh { get; set; }
        public decimal TargetPriceHighPercent { get; set; }
        public decimal TargetPriceMean { get; set; }
        public decimal TargetPriceMeanPercent { get; set; }
        public decimal TargetPriceMedian { get; set; }
        public decimal TargetPriceMedianPercent { get; set; }
        public decimal TargetPriceLow { get; set; }
        public decimal TargetPriceLowPercent { get; set; }

        public int StrongBuy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int StrongSell { get; set; }

        public decimal RecommendationTrend { get; set; }

        public string Industry { get; set; }

        public static StockInfo Create(
            string ticker,
            string industry,
            CurrentPrice currentPrice,
            PriceTarget priceTarget,
            PriceExpectationLevel priceExpectationLevel,
            RecommendationTrend recommendationTrend,
            decimal meanTrend
            )
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

                Industry = industry
            };
            return stock;
        }
    }
}
