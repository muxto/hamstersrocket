﻿using HamstersRocket.Contracts.Domain;
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

        public Task<RecommendationTrend> GetRecommendationTrendAsync(string ticker)
        {
            throw new NotImplementedException();
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

        public Task SaveReportToFileAsync(string report)
        {
            throw new NotImplementedException();
        }

        public Task SetAboutCompanyAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetAboutCompanyAsync(string ticker, AboutCompany aboutCompany)
        {
            throw new NotImplementedException();
        }

        public Task SetCurrentPriceAsync(DateTime date, string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetCurrentPriceAsync(DateTime date, string ticker, CurrentPrice currentPrice)
        {
            throw new NotImplementedException();
        }

        public Task SetPriceTargetAsync(DateTime date, string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetPriceTargetAsync(DateTime date, string ticker, PriceTarget priceTarget)
        {
            throw new NotImplementedException();
        }

        public Task SetRecommendationTrendAsync(DateTime date, string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetRecommendationTrendAsync(DateTime date, string ticker, RecommendationTrend recommendationTrend)
        {
            throw new NotImplementedException();
        }
    }
}
