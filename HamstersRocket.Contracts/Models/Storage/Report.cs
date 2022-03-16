using System;

namespace HamstersRocket.Contracts.Models.Storage
{
    public class Report
    {
        public DateTime UpdateDate { get; set; }
        public StockInfo[] Stocks { get; set; }
    }
}
