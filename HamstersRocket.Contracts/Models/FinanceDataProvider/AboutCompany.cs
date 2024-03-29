﻿namespace HamstersRocket.Contracts.Models.FinanceDataProvider
{
    public class AboutCompany
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Industry { get; set; }
        public string Exchange { get; set; }
        public string IPO { get; set; }
        public decimal MarketCapitalization { get; set; }
        public string Phone { get; set; }
        public decimal ShareOutstanding { get; set; }
        public string Weburl { get; set; }
        public string Logo { get; set; }
    }
}
