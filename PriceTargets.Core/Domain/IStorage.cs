using System.Threading.Tasks;
using PriceTargets.Core.Models.FinanceDataProvider;

namespace PriceTargets.Core.Domain
{
    public interface IStorage
    {
        Task SaveCurrentPriceAsync(string ticker, CurrentPrice currentPrice);

        Task SavePriceTargetAsync(string ticker, PriceTarget priceTarget);

        Task SaveRecommendationTrendAsync(string ticker, RecommendationTrend recommendationTrend);

        Task SaveReportAsync(string report);
    }
}