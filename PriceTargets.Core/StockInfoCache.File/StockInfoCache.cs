using PriceTargets.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PriceTargets.Core.Cache.File
{
    public class StockInfoCache : Domain.IStockInfoCache
    {
        private const string CACHE_FILE = "cache";

        private string Serialize(Models.StockInfo value)
        {
            return JsonSerializer.Serialize(value);
        }

        private Models.StockInfo Deserialize(string value)
        {
            return JsonSerializer.Deserialize<Models.StockInfo>(value);
        }

        public async Task<StockInfo[]> GetAllAsync()
        {
            if (!System.IO.File.Exists(CACHE_FILE))
            {
                return Array.Empty<StockInfo>();
            }

            var file = await System.IO.File.ReadAllLinesAsync(CACHE_FILE);
            var dict = new Dictionary<string, StockInfo>();
            foreach (var line in file)
            {
                var stockInfo = Deserialize(line);
                dict[stockInfo.Ticker] = stockInfo;
            }
            return dict.Values.ToArray();
        }

        public async Task SaveAsync(StockInfo value)
        {
            var json = Serialize(value);
            var lines = new[] { json };

            await System.IO.File.AppendAllLinesAsync(CACHE_FILE, lines);
        }

        public Task ClearAsync()
        {
            System.IO.File.Delete(CACHE_FILE);
            return Task.CompletedTask;
        }
    }
}
