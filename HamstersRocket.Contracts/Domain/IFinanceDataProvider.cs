﻿using HamstersRocket.Contracts.Models.FinanceDataProvider;
using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface IFinanceDataProvider
    {
        FinanceDataProviders Provider { get; }

        Task<PriceTarget> GetPriceTargetAsync(string ticker);

        Task<CurrentPrice> GetCurrentPriceAsync(string ticker);

        Task<RecommendationTrend> GetRecommendationTrends(string ticker);

        Task<AboutCompany> GetAboutCompanyAsync(string ticker);
    }
}
