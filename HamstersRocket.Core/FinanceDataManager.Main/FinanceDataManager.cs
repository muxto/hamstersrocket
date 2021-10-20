using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataManager;
using System.Linq;
using System.Threading.Tasks;

namespace HamstersRocket.Contracts.FinanceDataManager.Main
{
    public class FinanceDataManager : IFinanceDataManager
    {
        private IStorage storage;
        private IFinanceDataProvider[] dataProviders;

        public FinanceDataManager(IFinanceDataProvider[] dataProviders, IStorage storage)
        {
            this.dataProviders = dataProviders;
            this.storage = storage;
        }

        public async Task<CompanyInfo> GetCompanyInfoAsync(string ticker)
        {
            var finnhub = dataProviders.First(x => x.Provider == FinanceDataProviders.Finnhub);
            var yahooFinance = dataProviders.First(x => x.Provider == FinanceDataProviders.YahooFinance);

            var currentPrice = await yahooFinance.GetCurrentPriceAsync(ticker);
            var recommendationTrend = await finnhub.GetRecommendationTrends(ticker);
            var targetPrice = await yahooFinance.GetPriceTargetAsync(ticker);

            var aboutCompany = await storage.GetAboutCompanyAsync(ticker);
            if (aboutCompany == null)
            {
                aboutCompany = await finnhub.GetAboutCompanyAsync(ticker);
                await storage.SetAboutCompanyAsync(ticker, aboutCompany);
            }

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
