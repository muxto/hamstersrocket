using HamstersRocket.Contracts.Models.FinanceDataProvider;

namespace HamstersRocket.Core.FinanceDataProvider.TipRanks.Dto
{
    public class AnalystRatings
    {
        public AnalystPriceTarget analystPriceTarget { get; set; }

        public PriceTarget ToDomain()
        {
            var model = new PriceTarget()
            {
                High = analystPriceTarget?.high ?? default,
                Low = analystPriceTarget?.low ?? default,
                Mean = analystPriceTarget?.average ?? default,
                Median = analystPriceTarget?.average ?? default,
            };

            return model;
        }
    }

    public class AnalystPriceTarget
    {
        public decimal? average { get; set; }
        public decimal? high { get; set; }
        public decimal? low { get; set; }
    }
}
