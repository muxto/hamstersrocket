using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using HamstersRocket.Core.FinanceDataProvider.TipRanks.Dto;

namespace HamstersRocket.Core.FinanceDataProvider.TipRanks
{
    public class FinanceDataProvider : Contracts.Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://www.tipranks.com/api/stocks";
        private string _baseUrl2 = "https://widgets.tipranks.com/api/IB";
        
        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;

        public FinanceDataProviders Provider => FinanceDataProviders.TipRanks;

        private Data lastData;

        public FinanceDataProvider()
        {
            _httpClient = new HttpClient();

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

        private async Task<Data> GetData(string ticker)
        {
            if (lastData == null || lastData.Ticker != ticker)
            {
                var query = $"{_baseUrl}/getData/?name={ticker}";
                lastData = await GetJson<Data>(query);
            }
            return lastData;
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var query = $"{_baseUrl2}/analystratings?ticker={ticker}";
            var analystRatings = await GetJson<AnalystRatings>(query);

            if (analystRatings == null)
            {
                return new PriceTarget();
            }

            return analystRatings.ToDomain();
        }

        public async Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RecommendationTrend[]> GetRecommendationTrendsAsync(string ticker)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AboutCompany> GetAboutCompanyAsync(string ticker)
        {
            var model = await GetData(ticker);
            if (model == null)
            {
                return null;
            }

            return new AboutCompany()
            {
                Ticker = ticker,
                Industry = model.PortfolioHoldingData?.SectorId
            };
        }
    }
}
