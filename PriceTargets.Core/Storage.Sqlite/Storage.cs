using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using System;
using System.Threading.Tasks;

namespace HamstersRocket.Core.Storage.Sqlite
{
    public class Storage : IStorage
    {
        public Storage(string path)
        {

        }

        public Task SaveCurrentPriceAsync(string ticker, CurrentPrice currentPrice)
        {
            throw new NotImplementedException();
        }

        public Task SavePriceTarget(string ticker, CurrentPrice currentPrice, PriceTarget priceTarget)
        {
            throw new NotImplementedException();
        }

        public Task SavePriceTargetAsync(string ticker, PriceTarget priceTarget)
        {
            throw new NotImplementedException();
        }

        public Task SaveRecommendationTrendAsync(string ticker, RecommendationTrend recommendationTrend)
        {
            throw new NotImplementedException();
        }

        public Task SaveReportAsync(string report)
        {
            throw new NotImplementedException();
        }
    }
}
