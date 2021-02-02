using System;

namespace PriceTargets.Core.Models.FinanceDataProvider
{
    public class PriceTarget
    {
        public string Ticker { get; set; }

        public decimal TargetHigh { get; set; }

        public decimal TargetLow { get; set; }

        public decimal TargetMean { get; set; }

        public decimal TargetMedian { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
