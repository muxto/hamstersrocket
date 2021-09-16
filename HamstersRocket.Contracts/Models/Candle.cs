using System;

namespace HamstersRocket.Contracts.Models
{
    // TODO test for sqlite
    public class Candle
    {
        public DateTime Date { get; set; }
        public string Ticker { get; set; }
        public FinanceDataProvider.CurrentPrice Price { get; set; }
    }
}
