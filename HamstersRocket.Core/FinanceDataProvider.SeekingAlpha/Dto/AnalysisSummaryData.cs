using System;
using System.Globalization;

namespace HamstersRocket.Core.FinanceDataProvider.SeekingAlpha.Dto
{
    public class AnalysisSummaryData
    {
        public Data data { get; set; }

        public Contracts.Models.FinanceDataProvider.PriceTarget ToDomainPriceTarget()
        {
            var targetPrice = decimal.Parse(data.target_price, CultureInfo.InvariantCulture);

            var model = new Contracts.Models.FinanceDataProvider.PriceTarget()
            {
                LastUpdated = DateTime.Now,
                TargetHigh = targetPrice,
                TargetLow = targetPrice,
                TargetMean = targetPrice,
                TargetMedian = targetPrice
            };

            return model;
        }

        public Contracts.Models.FinanceDataProvider.RecommendationTrend ToDomainRecommendationTrend()
        {
            var model = new Contracts.Models.FinanceDataProvider.RecommendationTrend()
            {
                StrongBuy = (int)decimal.Parse(data.analysts_buy, CultureInfo.InvariantCulture),
                Buy = (int)decimal.Parse(data.analysts_outperform, CultureInfo.InvariantCulture),
                Hold = (int)decimal.Parse(data.analysts_hold, CultureInfo.InvariantCulture),
                Sell = (int)decimal.Parse(data.analysts_underperform, CultureInfo.InvariantCulture),
                StrongSell = (int)decimal.Parse(data.analysts_sell, CultureInfo.InvariantCulture),
                Period = DateTime.Now
            };

            return model;
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
