using HamstersRocket.Contracts.Models.FinanceDataProvider;
using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface IFinanceDataProvider
    {
        FinanceDataProviders Provider { get; }

        Task<PriceTarget> GetPriceTargetAsync(string ticker);

        Task<PriceTarget[]> GetPriceTargetsAsync(string[] tickers);

        Task<CurrentPrice> GetCurrentPriceAsync(string ticker);

        Task<CurrentPrice[]> GetCurrentPricesAsync(string[] tickers);

        Task<Recommendations> GetRecommendationsAsync(string ticker);

        Task<AboutCompany> GetAboutCompanyAsync(string ticker);
    }
}
