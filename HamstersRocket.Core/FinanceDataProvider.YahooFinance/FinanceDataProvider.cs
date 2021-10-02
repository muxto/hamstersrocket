using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace HamstersRocket.Core.FinanceDataProvider.YahooFinance
{
    /// <summary>
    /// Unnoficial API, not legal to use it
    /// </summary>
    public class FinanceDataProvider : Contracts.Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://query1.finance.yahoo.com/v10/finance/quoteSummary";

        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;
        private readonly int _delay;

        public FinanceDataProviders Provider => FinanceDataProviders.YahooFinance;

        public FinanceDataProvider(int delay)
        {
            _httpClient = new HttpClient();

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
            _delay = delay;
        }

        public void Clear()
        {
        }

        private async Task<T> GetJson<T>(string query)
        {
            await Task.Delay(_delay);

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

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var query = $"{_baseUrl}/{ticker}?modules=financialData";
            var financialData = await GetJson<Dto.Data>(query);

            if (financialData == null)
            {
                return new PriceTarget();
            }

            return financialData.quoteSummary.result.FirstOrDefault().financialData.ToDomain();
        }

        public Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            throw new System.NotImplementedException();
        }

        public Task<RecommendationTrend> GetRecommendationTrends(string ticker)
        {
            throw new System.NotImplementedException();
        }

        public Task<AboutCompany> GetAboutCompanyAsync(string ticker)
        {
            throw new System.NotImplementedException();
        }
    }
}