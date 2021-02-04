namespace PriceTargets.Core.Models.Publisher
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
    }
}
