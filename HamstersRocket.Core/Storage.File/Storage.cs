using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using HamstersRocket.Contracts.Models.Storage;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace HamstersRocket.Core.Storage.File
{
    public class Storage : IStorage 
    {
        public async Task CreateReportAsync(StockInfo[] stocks)
        {
            var report = new Report()
            {
                UpdateDate = DateTime.Now,
                Stocks = stocks
            };

            var json = JsonSerializer.Serialize(report);
            
            await System.IO.File.WriteAllTextAsync("report.json", json);
        }

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
