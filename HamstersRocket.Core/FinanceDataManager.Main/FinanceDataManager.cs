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
            var tipranks = dataProviders.First(x => x.Provider == FinanceDataProviders.TipRanks);
            var seekingAlpha = dataProviders.First(x => x.Provider == FinanceDataProviders.SeekingAlpha);

            tipranks.Clear();
            seekingAlpha.Clear();

            var currentPrice = await finnhub.GetCurrentPriceAsync(ticker);
            await Task.Delay(1000);

            //var recommendationTrends = await seekingAlpha.GetRecommendationTrendsAsync(ticker);
            var recommendationTrends = await finnhub.GetRecommendationTrendsAsync(ticker);
            await Task.Delay(1000);

            var recommendationTrend = recommendationTrends?
                .OrderBy(x => x.Period)
                .FirstOrDefault() ?? new Contracts.Models.FinanceDataProvider.RecommendationTrend();

            //var targetPrice = await seekingAlpha.GetPriceTargetAsync(ticker);
            var targetPrice = await tipranks.GetPriceTargetAsync(ticker);
            await Task.Delay(1000);

            var aboutCompany = await finnhub.GetAboutCompanyAsync(ticker);
            await Task.Delay(1000);

            var companyInfo = new CompanyInfo()
            {
                Ticker = ticker,
                Industry = aboutCompany.Industry,
                CurrentPrice = currentPrice,
                PriceTarget = targetPrice,
                RecommendationTrend = recommendationTrend,
            };

            return companyInfo;
        }
    }
}
