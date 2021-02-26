using PriceTargets.Core.Models;
using PriceTargets.Core.Models.FinanceDataProvider;
using PriceTargets.Core.Models.Measure;
using PriceTargets.Core.Models.Publisher;
using System;
using System.Text.Json;

namespace PriceTargets.Core.Publisher.Json
{
    public class Publisher : Domain.IPublisher
    {
        public Report CreateReport(StockInfo[] stocks)
        {
            var report = new Report()
            {
                UpdateDate = DateTime.Now,
                Stocks = stocks
            };

            return report;
        }

        public string FormatReport(Report report)
        {
            var json = JsonSerializer.Serialize(report);
            return json;
        }
    }
}