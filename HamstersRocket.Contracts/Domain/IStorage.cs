using HamstersRocket.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface IStorage 
    {
        Task CreateReportAsync(StockInfo[] stockInfos);

        Task<Models.FinanceDataProvider.CurrentPrice> GetCurrentPriceAsync(string ticker);
        Task SetCurrentPriceAsync(DateTime date, string ticker, Models.FinanceDataProvider.CurrentPrice currentPrice);

        Task<Models.FinanceDataProvider.Recommendations> GetRecommendationsAsync (string ticker);
        Task SetRecommendationsAsync(DateTime date, string ticker, Models.FinanceDataProvider.Recommendations recommendations);

        Task<Models.FinanceDataProvider.AboutCompany> GetAboutCompanyAsync(string ticker);
        Task SetAboutCompanyAsync(string ticker, Models.FinanceDataProvider.AboutCompany aboutCompany);

        Task<Models.FinanceDataProvider.PriceTarget> GetPriceTargetAsync(string ticker);
        Task SetPriceTargetAsync(DateTime date, string ticker, Models.FinanceDataProvider.PriceTarget priceTarget);
    }
}