using HamstersRocket.Contracts.Models.FinanceDataProvider;

namespace HamstersRocket.Contracts.Models
{
    public class StockInfo
    {
        public string Ticker { get; set; }
        public string Industry { get; set; }
        public string CompanyName { get; set; }
        public string Logo { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal TargetPriceHigh { get; set; }
        public decimal TargetPriceMean { get; set; }
        public decimal TargetPriceMedian { get; set; }
        public decimal TargetPriceLow { get; set; }

        public int StrongBuy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int StrongSell { get; set; }

        public static StockInfo Create(
            string ticker,
            string industry,
            string companyName,
            string logo,
            CurrentPrice currentPrice,
            PriceTarget priceTarget,
            Recommendations recommendationTrend
            )
        {
            var stock = new StockInfo()
            {
                Ticker = ticker,
                Industry = industry,
                CompanyName = companyName,
                Logo = logo,

                CurrentPrice = currentPrice.C,

                TargetPriceHigh = priceTarget.High,
                TargetPriceMean = priceTarget.Mean,
                TargetPriceMedian = priceTarget.Median,
                TargetPriceLow = priceTarget.Low,

                StrongBuy = recommendationTrend.StrongBuy,
                Buy = recommendationTrend.Buy,
                Hold = recommendationTrend.Hold,
                Sell = recommendationTrend.Sell,
                StrongSell = recommendationTrend.StrongSell,

                
            };
            return stock;
        }
    }
}
