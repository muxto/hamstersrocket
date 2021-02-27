using System;

namespace HamstersRocket.Contracts.Models.Publisher
{
    public class Report
    {
        public DateTime UpdateDate { get; set; }
        public StockInfo[] Stocks { get; set; }
    }
}
