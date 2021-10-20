using System.Globalization;

namespace HamstersRocket.Core.FinanceDataProvider.AlphaVantage.Dto
{
    public class Overview
    {
        public string AnalystTargetPrice { get; set; }

        public Contracts.Models.FinanceDataProvider.PriceTarget ToDomainPriceTarget()
        {
            var targetPrice = TryParse(AnalystTargetPrice);

            var model = new Contracts.Models.FinanceDataProvider.PriceTarget()
            {
                TargetHigh = targetPrice,
                TargetLow = targetPrice,
                TargetMean = targetPrice,
                TargetMedian = targetPrice
            };

            return model;
        }

        private int TryParse(string field)
        {
            return (field == null || field == "None") ? 0 : (int)decimal.Parse(field, CultureInfo.InvariantCulture);
        }
    }
}
