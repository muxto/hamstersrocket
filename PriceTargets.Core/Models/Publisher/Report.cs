using System;

namespace PriceTargets.Core.Models.Publisher
{
    public class Report
    {
        public DateTime UpdateDate { get; set; }
        public StockInfo[] Stocks { get; set; }
    }
}
