using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using HamstersRocket.Core.FinanceDataProvider.SeekingAlpha.Dto;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HamstersRocket.Core.FinanceDataProvider.SeekingAlpha
{
    public class FinanceDataProvider : Contracts.Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://seekingalpha.com";

        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;

        public FinanceDataProviders Provider => FinanceDataProviders.SeekingAlpha;

        private AnalysisSummaryData lastData;

        public FinanceDataProvider()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        }

        public void Clear()
        {
            lastData = null;
        }

        private async Task<T> GetJson<T>(string query)
        {
            var response = await _httpClient.GetAsync(query);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
            var message = response.EnsureSuccessStatusCode();
            var content = await message.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
            return model;
        }

        private async Task<AnalysisSummaryData> GetData(string ticker)
        {
            if (lastData == null)
            {
                var query = $"{_baseUrl}/symbol/{ticker}/ratings/analysis_summary_data";
                lastData = await GetJson<AnalysisSummaryData>(query);
            }
            return lastData;
        }

        public Task<AboutCompany> GetAboutCompanyAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var model = await GetData(ticker);
            if (model == null)
            {
                return new PriceTarget();
            }

            return model.ToDomainPriceTarget();
        }

        public async Task<RecommendationTrend[]> GetRecommendationTrendsAsync(string ticker)
        {
            var model = await GetData(ticker);
            if (model == null)
            {
                return new RecommendationTrend[] { };
            }

            return new RecommendationTrend[] { model.ToDomainRecommendationTrend(), };
        }
    }
}
