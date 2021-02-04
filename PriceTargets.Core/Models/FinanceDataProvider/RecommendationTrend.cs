using System;

namespace PriceTargets.Core.Models.FinanceDataProvider
{
    public class RecommendationTrend
    {
        public string Ticker { get; set; }
        public int StrongBuy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int StrongSell { get; set; }
        public DateTime Period { get; set; }
    }
}
