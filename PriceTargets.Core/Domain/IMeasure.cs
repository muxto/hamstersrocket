using PriceTargets.Core.Models.Measure;

namespace PriceTargets.Core.Domain
{
    public interface IMeasure
    {
        PriceExpectationLevel GetPriceExpectationsLevel(Models.FinanceDataProvider.CurrentPrice currentPrice, Models.FinanceDataProvider.PriceTarget priceTarget);

        decimal GetTrendExpectationsLevel(Models.FinanceDataProvider.RecommendationTrend recommendationTrend);
    }
}
