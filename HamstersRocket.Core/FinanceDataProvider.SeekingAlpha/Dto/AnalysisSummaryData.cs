using System.Globalization;

namespace HamstersRocket.Core.FinanceDataProvider.SeekingAlpha.Dto
{
    public class AnalysisSummaryData
    {
        public Data data { get; set; }

        public Contracts.Models.FinanceDataProvider.PriceTarget ToDomainPriceTarget()
        {
            var targetPrice = TryParse(data.target_price);

            var model = new Contracts.Models.FinanceDataProvider.PriceTarget()
            {
                High = targetPrice,
                Low = targetPrice,
                Mean = targetPrice,
                Median = targetPrice
            };

            return model;
        }

        public Contracts.Models.FinanceDataProvider.Recommendations ToDomainRecommendations()
        {
            if (data == null)
            {
                return new Contracts.Models.FinanceDataProvider.Recommendations();
            }

            return new Contracts.Models.FinanceDataProvider.Recommendations
            {
                StrongBuy = TryParse(data.analysts_buy),
                Buy = TryParse(data.analysts_outperform),
                Hold = TryParse(data.analysts_hold),
                Sell = TryParse(data.analysts_underperform),
                StrongSell = TryParse(data.analysts_sell),
            };
        }

        private int TryParse(string field)
        {
            return field == null ? 0 : (int)decimal.Parse(field, CultureInfo.InvariantCulture);
        }
    }

    public class Data
    { 
        public string target_price { get; set; }
        public string rating { get; set; }
        public string analysts_buy { get; set; }
        public string analysts_outperform { get; set; }
        public string analysts_hold { get; set; }
        public string analysts_underperform { get; set; }
        public string analysts_sell { get; set; }
    }


}
