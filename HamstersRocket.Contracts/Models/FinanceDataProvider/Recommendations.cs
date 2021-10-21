using System;

namespace HamstersRocket.Contracts.Models.FinanceDataProvider
{
    public class Recommendations
    {
        public int StrongBuy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int StrongSell { get; set; }
    }
}
