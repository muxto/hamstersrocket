using HamstersRocket.Contracts.Models.FinanceDataProvider;

namespace HamstersRocket.Core.FinanceDataProvider.YahooFinance.Dto
{
    public class Data
    {
        public QuoteSummary quoteSummary { get; set; }
    }

    public class QuoteSummary
    {
        public Result[] result { get; set; }
    }

    public class Result
    {
        public FinancialData financialData { get; set; }
    }

    public class FinancialData
    {
        public Price currentPrice { get; set; }
        public Price targetHighPrice { get; set; }
        public Price targetLowPrice { get; set; }
        public Price targetMeanPrice { get; set; }
        public Price targetMedianPrice { get; set; }

        public PriceTarget ToDomain()
        {
            var model = new PriceTarget()
            {
                High = targetHighPrice?.raw ?? default,
                Low = targetLowPrice?.raw ?? default,
                Mean = targetMeanPrice?.raw ?? default,
                Median = targetMedianPrice?.raw ?? default,
            };

            return model;
        }
    }

    public class Price
    {
        public decimal raw { get; set; }
    }
}

