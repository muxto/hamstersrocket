using PriceTargets.Core.Models.FinanceDataProvider;

namespace PriceTargets.Core.Models
{
    public class StockInfo
    {
        public string Ticker { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal TargetPriceHigh { get; set; }
        public decimal TargetPriceMean { get; set; }
        public decimal TargetPriceMedian { get; set; }
        public decimal TargetPriceLow { get; set; }

        public int StrongBuy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int StrongSell { get; set; }

        public string Industry { get; set; }

        public static StockInfo Create(
            string ticker,
            string industry,
            CurrentPrice currentPrice,
            PriceTarget priceTarget,
            RecommendationTrend recommendationTrend
            )
        {
            var stock = new StockInfo()
            {
                Ticker = ticker,
                CurrentPrice = currentPrice.C,

                TargetPriceHigh = priceTarget.TargetHigh,
                TargetPriceMean = priceTarget.TargetMean,
                TargetPriceMedian = priceTarget.TargetMedian,
                TargetPriceLow = priceTarget.TargetLow,

                StrongBuy = recommendationTrend.StrongBuy,
                Buy = recommendationTrend.Buy,
                Hold = recommendationTrend.Hold,
                Sell = recommendationTrend.Sell,
                StrongSell = recommendationTrend.StrongSell,

                Industry = industry
            };
            return stock;
        }
    }
}
