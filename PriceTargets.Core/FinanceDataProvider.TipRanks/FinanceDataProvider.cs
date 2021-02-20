using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using PriceTargets.Core.Domain;
using PriceTargets.Core.Models.FinanceDataProvider;


namespace PriceTargets.Core.FinanceDataProvider.TipRanks
{
    public class FinanceDataProvider : Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://www.tipranks.com/api/stocks";

        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;

        public FinanceDataProviders Provider => FinanceDataProviders.TipRanks;

        private Dto.Data lastData;

        public FinanceDataProvider()
        {
            _httpClient = new HttpClient();

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
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

        private async Task<Dto.Data> GetData(string ticker)
        {
            if (lastData == null || lastData.Ticker != ticker)
            {
                var query = $"{_baseUrl}/getData/?name={ticker}";
                lastData = await GetJson<Dto.Data>(query);
            }
            return lastData;
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var model = await GetData(ticker);
            if (model == null)
            {
                return new PriceTarget();
            }

            var targetPrice = model.PtConsensus?.FirstOrDefault();
            return targetPrice.ToDomain();
        }

        public async Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RecommendationTrend[]> GetRecommendationTrendsAsync(string ticker)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetIndustryAsync(string ticker)
        {
            var model = await GetData(ticker);
            if (model == null)
            {
                return null;
            }

            return model.PortfolioHoldingData?.SectorId;
        }
    }
}
