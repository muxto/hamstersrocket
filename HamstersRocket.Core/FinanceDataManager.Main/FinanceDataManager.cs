using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataManager;
using System.Linq;
using System.Threading.Tasks;

namespace HamstersRocket.Contracts.FinanceDataManager.Main
{
    public class FinanceDataManager : IFinanceDataManager
    {
        public IFinanceDataProvider[] dataProviders { get; }

        public FinanceDataManager(IFinanceDataProvider[] dataProviders)
        {
            this.dataProviders = dataProviders;
        }

        public async Task<CompanyInfo> GetCompanyInfoAsync(string ticker)
        {
            var finnhub = dataProviders.First(x => x.Provider == FinanceDataProviders.Finnhub);
            var yahooFinance = dataProviders.First(x => x.Provider == FinanceDataProviders.YahooFinance);

            var currentPrice = await yahooFinance.GetCurrentPriceAsync(ticker);
            var recommendationTrend = await finnhub.GetRecommendationTrends(ticker);
            var targetPrice = await yahooFinance.GetPriceTargetAsync(ticker);
            var aboutCompany = await finnhub.GetAboutCompanyAsync(ticker);

            var companyInfo = new CompanyInfo()
            {
                Ticker = ticker,
                Industry = aboutCompany.Industry,
                CompanyName = aboutCompany.Name,
                Logo = aboutCompany.Logo,
                CurrentPrice = currentPrice,
                PriceTarget = targetPrice,
                RecommendationTrend = recommendationTrend,
            };

            return companyInfo;
        }
    }
}
