namespace HamstersRocket.Core.FinanceDataProvider.Finnhub.Dto
{
    public class PriceTarget
    {
        public string symbol { get; set; }
        public decimal targetHigh { get; set; }
        public decimal targetLow { get; set; }
        public decimal targetMean { get; set; }
        public decimal targetMedian { get; set; }
        public string lastUpdated { get; set; }

        public Contracts.Models.FinanceDataProvider.PriceTarget ToDomain()
        {
            var model = new Contracts.Models.FinanceDataProvider.PriceTarget()
            {
                High = targetHigh,
                Low = targetLow,
                Mean = targetMean,
                Median = targetMedian,
            };

            return model;
        }
    }
}
