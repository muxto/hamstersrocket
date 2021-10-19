namespace HamstersRocket.Core.FinanceDataProvider.Finnhub.Dto
{
    public class AboutCompany
    {
        public string name { get; set; }
        public string ticker { get; set; }
        public string country { get; set; }
        public string currency { get; set; }
        public string finnhubIndustry { get; set; }
        public string exchange { get; set; }
        public string ipo { get; set; }
        public decimal marketCapitalization { get; set; }
        public string phone { get; set; }
        public decimal shareOutstanding { get; set; }
        public string weburl { get; set; }
        public string logo { get; set; }

        public Contracts.Models.FinanceDataProvider.AboutCompany ToDomain()
        {
            var model = new Contracts.Models.FinanceDataProvider.AboutCompany()
            {
                Name = name,
                Ticker = ticker,
                Country = country,
                Currency = currency,
                Industry = finnhubIndustry,
                Exchange = exchange,
                IPO = ipo,
                MarketCapitalization = marketCapitalization,
                Phone = phone,
                ShareOutstanding = shareOutstanding,
                Weburl = weburl,
                Logo = logo,
            };

            return model;
        }
    }
}
