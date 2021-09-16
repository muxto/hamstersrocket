using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models;
using HamstersRocket.Contracts.Models.Publisher;
using System;
using System.Text.Json;

namespace HamstersRocket.Core.Publisher.Json
{
    public class Publisher : IPublisher
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
    }
}