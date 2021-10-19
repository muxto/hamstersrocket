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
        private string baseUrl = "https://query1.finance.yahoo.com/v10/finance/quoteSummary";

        private HttpClient httpClient;
        private JsonSerializerOptions jsonSerializerOptions;
        private readonly int delay;

        private string lastTicker;
        private Dto.FinancialData lastFinancialData;

        public FinanceDataProviders Provider => FinanceDataProviders.YahooFinance;

        public FinanceDataProvider(int delay)
        {
            httpClient = new HttpClient();

            jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.PropertyNameCaseInsensitive = true;
            this.delay = delay;
        }

        private async Task<Dto.FinancialData> GetFinancialData(string ticker)
        {
            if (lastTicker == ticker && lastFinancialData != null)
            {
                return lastFinancialData;
            }

            lastTicker = ticker;
            lastFinancialData = null;

            var query = $"{baseUrl}/{ticker}?modules=financialData";
            var data = await GetJson<Dto.Data>(query);
            lastFinancialData = data?.quoteSummary.result.FirstOrDefault().financialData;
            return lastFinancialData;
        }

        private async Task<T> GetJson<T>(string query)
        {
            await Task.Delay(delay);

            var response = await httpClient.GetAsync(query);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
            var message = response.EnsureSuccessStatusCode();
            var content = await message.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);
            return model;
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var financialData = await GetFinancialData(ticker);
            if (financialData == null)
            {
                return new PriceTarget();
            }

            return financialData.ToDomain();
        }

        public async Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            var financialData = await GetFinancialData(ticker);
            if (financialData == null)
            {
                return new CurrentPrice();
            }

            return new CurrentPrice() { C = financialData.currentPrice.raw };
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