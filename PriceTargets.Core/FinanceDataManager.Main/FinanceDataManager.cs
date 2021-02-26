using PriceTargets.Core.Models.FinanceDataManager;
using System.Linq;
using System.Threading.Tasks;
using PriceTargets.Core.Domain;

namespace PriceTargets.Core.FinanceDataManager.Main
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

            var currentPrice = await finnhub.GetCurrentPriceAsync(ticker);
            await Task.Delay(1000);

            var recommendationTrends = await finnhub.GetRecommendationTrendsAsync(ticker);
            await Task.Delay(1000);

            var recommendationTrend = recommendationTrends
                .OrderBy(x => x.Period)
                .FirstOrDefault() ?? new Core.Models.FinanceDataProvider.RecommendationTrend();

            var targetPrice = await tipranks.GetPriceTargetAsync(ticker);
            var industry = await tipranks.GetIndustryAsync(ticker);
            //await Task.Delay(1000);

            var companyInfo = new CompanyInfo()
            {
                Ticker = ticker,
                Industry = industry,
                CurrentPrice = currentPrice,
                PriceTarget = targetPrice,
                RecommendationTrend = recommendationTrend,
            };

            return companyInfo;
        }
    }
}
