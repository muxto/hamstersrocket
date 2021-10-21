using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using System;
using System.Threading.Tasks;

namespace HamstersRocket.Core.Storage.File
{
    public class Storage : IStorage
    {
        public Task<AboutCompany> GetAboutCompanyAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<Recommendations> GetRecommendationsAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public async Task SaveReportToFileAsync(string report)
        {
            await System.IO.File.WriteAllTextAsync("report.json", report);
        }

        public Task SetAboutCompanyAsync(string ticker, AboutCompany aboutCompany)
        {
            throw new NotImplementedException();
        }

        public Task SetCurrentPriceAsync(DateTime date, string ticker, CurrentPrice currentPrice)
        {
            throw new NotImplementedException();
        }

        public Task SetPriceTargetAsync(DateTime date, string ticker, PriceTarget priceTarget)
        {
            throw new NotImplementedException();
        }

        public Task SetRecommendationsAsync(DateTime date, string ticker, Recommendations recommendations)
        {
            throw new NotImplementedException();
        }
    }
}
