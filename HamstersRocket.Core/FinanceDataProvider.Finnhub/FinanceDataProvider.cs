using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;

namespace HamstersRocket.Contracts.FinanceDataProvider.Finnhub
{
    public class FinanceDataProvider : Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://finnhub.io/api/v1/";
        private string _token;

        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;

        public FinanceDataProviders Provider => FinanceDataProviders.Finnhub;

        public FinanceDataProvider(string token)
        {
            _token = token;
            _httpClient = new HttpClient();

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        }

        private async Task<T> GetJson<T>(string query)
        {
            var response = await _httpClient.GetAsync(query);
            var message = response.EnsureSuccessStatusCode();
            var content = await message.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
            return model;
        }

        public async Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            var query = $"{_baseUrl}/quote?symbol={ticker}&token={_token}";
            var model = await GetJson<CurrentPrice>(query);
            return model;
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var query = $"{_baseUrl}/stock/price-target?symbol={ticker}&token={_token}";
            var model = await GetJson<Core.FinanceDataProvider.Finnhub.Dto.PriceTarget>(query);
            return model.ToDomain();
        }

        public async Task<RecommendationTrend[]> GetRecommendationTrendsAsync(string ticker)
        {
            var query = $"{_baseUrl}/stock/recommendation?symbol={ticker}&token={_token}";
            var models = await GetJson<Core.FinanceDataProvider.Finnhub.Dto.RecommendationTrend[]>(query);
            return models.Select(x => x.ToDomain()).ToArray();
        }

        public Task<string> GetIndustryAsync(string ticker)
        {
            throw new System.NotImplementedException();
        }
    }
}
