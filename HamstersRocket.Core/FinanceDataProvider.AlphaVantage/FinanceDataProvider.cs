using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HamstersRocket.Core.FinanceDataProvider.AlphaVantage
{
    public class FinanceDataProvider : Contracts.Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://www.alphavantage.co/";
        private string[] _tokens;
        private readonly int _delay;
        private static int _tokenNum;

        private HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;

        public FinanceDataProviders Provider => FinanceDataProviders.AlphaVantage;

        public FinanceDataProvider(string[] tokens, int delay)
        {
            _tokens = tokens;
            _delay = delay;
            _httpClient = new HttpClient();

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;

            _tokenNum = 0;
        }

        private async Task<string> GetToken()
        {
            // такой вот итератор
            if (_tokenNum >= _tokens.Length)
            {
                _tokenNum = 0;
            }

            var token = _tokens[_tokenNum];
            _tokenNum++;

            await Task.Delay(_delay);
            return token;
        }

        private async Task<T> GetJson<T>(string query)
        {
            var response = await _httpClient.GetAsync(query);
            var message = response.EnsureSuccessStatusCode();
            var content = await message.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
            return model;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<AboutCompany> GetAboutCompanyAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var token = await GetToken();
            var query = $"{_baseUrl}query?function=OVERVIEW&symbol={ticker}&apikey={token}";
            var model = await GetJson<Core.FinanceDataProvider.AlphaVantage.Dto.Overview>(query);
            return model.ToDomainPriceTarget();
        }

        public Task<Recommendations> GetRecommendationsAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<PriceTarget[]> GetPriceTargetsAsync(string[] tickers)
        {
            throw new NotImplementedException();
        }

        public Task<CurrentPrice[]> GetCurrentPricesAsync(string[] tickers)
        {
            throw new NotImplementedException();
        }
    }
}
