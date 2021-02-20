using System.Threading.Tasks;

namespace PriceTargets.Core.Domain
{
    public interface IFinanceDataProvider
    {
        FinanceDataProviders Provider { get; }

        Task<Models.FinanceDataProvider.PriceTarget> GetPriceTargetAsync(string ticker);

        Task<Models.FinanceDataProvider.CurrentPrice> GetCurrentPriceAsync(string ticker);

        Task<Models.FinanceDataProvider.RecommendationTrend[]> GetRecommendationTrendsAsync(string ticker);
    }
}
