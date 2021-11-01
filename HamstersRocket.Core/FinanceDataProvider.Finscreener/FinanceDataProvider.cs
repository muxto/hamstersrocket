using Dapper;
using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using HamstersRocket.Core.Helpers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace HamstersRocket.Core.FinanceDataProvider.Finscreener
{
    public class FinanceDataProvider : Contracts.Domain.IFinanceDataProvider
    {
        private string _baseUrl = "https://www.finscreener.org/";

        private HttpClient _httpClient;
        private readonly IOutput output;
        private readonly string _dbConnection;
        private int _delay;

        private Dto.Item[] _items;

        public FinanceDataProviders Provider => FinanceDataProviders.Finscreener;

        public FinanceDataProvider(IOutput output, string dbConnection, int delay)
        {
            this.output = output;
            _dbConnection = dbConnection;
            _delay = delay;
            _httpClient = new HttpClient();
        }

        private async Task<int[]> GetTickersId(params string[] tickers)
        {
            var ids = new List<int>();

            using (var connection = new NpgsqlConnection(_dbConnection))
            {
                foreach (var t in tickers)
                {
                    var query =
                        "SELECT id, ticker " +
                        "FROM finscreener_ticker_id " +
                        "WHERE ticker = @ticker; ";

                    var param = new
                    {
                        ticker = t,
                    };

                    var result = await connection.QueryAsync<int?>(query, param);
                    var id = result?.FirstOrDefault();
                    if (id == null)
                    {
                        continue;
                    }

                    ids.Add(id.Value);
                }
            }
            
            ids.Sort();
            return ids.Distinct().ToArray();
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

        public Task<Recommendations> GetRecommendationsAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public async Task<PriceTarget[]> GetPriceTargetsAsync(string[] tickers)
        {
            if (_items == null)
            {
                _items = await GetData(tickers);
            }

            var priceTargets =
                _items.Select((x) =>
                {
                    var pt = x.Current.ToDomainPriceTarget();
                    pt.Ticker = x.Ticker;
                    return pt;
                }).ToArray();
            return priceTargets;
        }

        public async Task<CurrentPrice[]> GetCurrentPricesAsync(string[] tickers)
        {
            if (_items == null)
            {
                _items = await GetData(tickers);
            }

            var currentPrices =
                _items.Select(x => x.ToDomainCurrentPrice()).ToArray();
            return currentPrices;
        }

        private async Task<Dto.Item[]> GetData(string[] tickers)
        {
            output.Publish($"Finsreener");

            var ids = await GetTickersId(tickers);

            output.Publish($"Got {ids.Length} / {tickers.Length} id");
            output.Publish($"Start");

            var chunkSize = 100;

            var items = new List<Dto.Item>();

            for (int i = 0; i < ids.Length; i += chunkSize)
            {
                output.Publish($"{i}-{i + chunkSize} / {ids.Length}");
                
                var idsChunk = ids.Slice(i, chunkSize);

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("povodna_uri", "analysts/target-price-and-potential/us-markets"),
                    new KeyValuePair<string, string>("zoznam_cp_pokrocile_filtre", string.Join(',', idsChunk))
                });

                var response = await _httpClient.PostAsync(
                    _baseUrl + "/engine/ajax/cielovacenastarter.php",
                    formContent);

                await Task.Delay(1000);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(content);

                var table = document.DocumentNode.Descendants("table").FirstOrDefault();

                foreach (var child in table.ChildNodes)
                {
                    var row = child.ChildNodes;
                    if (row.Any(x => x.Name == "th"))
                    {
                        continue;
                    }

                    var item = new Dto.Item()
                    {
                        Link = row[1].ChildNodes[0].Attributes[0].Value,
                        Ticker = row[1].ChildNodes[0].InnerText,
                        Price = row[2].InnerText,
                        Potential = row[3].InnerText,
                        Current = new Dto.TargetPrice()
                        {
                            Mean = row[4].InnerText,
                            High = row[5].InnerText,
                            Low = row[6].InnerText,
                        },
                        ThreeWeeksAgo = new Dto.TargetPrice()
                        {
                            Mean = row[7].InnerText,
                            High = row[8].InnerText,
                            Low = row[9].InnerText,
                        },
                        Change = new Dto.TargetPrice()
                        {
                            Mean = row[10].InnerText,
                            High = row[11].InnerText,
                            Low = row[12].InnerText,
                        },
                        AnalystRating = row[13].InnerText,
                    };

                    items.Add(item);
                }
            }

            return items.ToArray();
        }

       
    }
}
