using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;

namespace HamstersRocket.Core.FinanceDataProvider.Finnhub
{
    public class FinanceDataProvider : Contracts.Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://finnhub.io/api/v1/";
        private string _token;

        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;
        private int _delay;

        public FinanceDataProviders Provider => FinanceDataProviders.Finnhub;

        public FinanceDataProvider(string token, int delay)
        {
            _token = token;
            _delay = delay;
            _httpClient = new HttpClient();

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        }

        private async Task<string> GetToken()
        {
            await Task.Delay(_delay);
            return _token;
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
            var token = await GetToken();
            var query = $"{_baseUrl}/quote?symbol={ticker}&token={token}";
            var model = await GetJson<CurrentPrice>(query);
            return model;
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var token = await GetToken();
            var query = $"{_baseUrl}/stock/price-target?symbol={ticker}&token={token}";
            var model = await GetJson<Core.FinanceDataProvider.Finnhub.Dto.PriceTarget>(query);
            return model.ToDomain();
        }

        public async Task<Recommendations> GetRecommendationsAsync(string ticker)
        {
            var token = await GetToken();
            var query = $"{_baseUrl}/stock/recommendation?symbol={ticker}&token={token}";
            var models = await GetJson<Core.FinanceDataProvider.Finnhub.Dto.RecommendationTrend[]>(query);
            var recommedationModel = models?
               .OrderBy(x => x.period)
               .FirstOrDefault();

            return recommedationModel?.ToDomain() ?? new Recommendations();
        }

        public async Task<AboutCompany> GetAboutCompanyAsync(string ticker)
        {
            var token = await GetToken();
            var query = $"{_baseUrl}/stock/profile2?symbol={ticker}&token={token}";
            var model = await GetJson<Core.FinanceDataProvider.Finnhub.Dto.AboutCompany>(query);
            return model.ToDomain();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}
