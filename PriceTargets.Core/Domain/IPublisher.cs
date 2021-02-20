using PriceTargets.Core.Models;
using PriceTargets.Core.Models.FinanceDataProvider;
using PriceTargets.Core.Models.Measure;
using PriceTargets.Core.Models.Publisher;

namespace PriceTargets.Core.Domain
{
    public interface IPublisher
    {
        StockInfo CreatePublishItem(
            string ticker,
            CurrentPrice currentPrice,
            PriceTarget priceTarget,
            PriceExpectationLevel priceExpectationLevel,
            RecommendationTrend recommendationTrend,
            decimal meanTrend);

        Report CreateReport(StockInfo[] stocks);

        string FormatReport(Report report);
    }
}
