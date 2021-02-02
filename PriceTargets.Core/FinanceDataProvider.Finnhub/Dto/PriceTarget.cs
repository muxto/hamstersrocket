using System;

namespace PriceTargets.Core.FinanceDataProvider.Finnhub.Dto
{
    public class PriceTarget
    {
        public string symbol { get; set; }
        public decimal targetHigh { get; set; }
        public decimal targetLow { get; set; }
        public decimal targetMean { get; set; }
        public decimal targetMedian { get; set; }
        public string lastUpdated { get; set; }

        public Models.FinanceDataProvider.PriceTarget ToDomain()
        {
            var model = new Models.FinanceDataProvider.PriceTarget()
            {
                Ticker = symbol,
                TargetHigh = targetHigh,
                TargetLow = targetLow,
                TargetMean = targetMean,
                TargetMedian = targetMedian,

            };

            if (DateTime.TryParse(lastUpdated, out var datetime))
            {
                model.LastUpdated = datetime;
            }

            return model;
        }
    }
}
