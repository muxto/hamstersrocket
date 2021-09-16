using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.Publisher;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace HamstersRocket.Core.Storage.File
{
    public class StorageFile : IStorage
    {
        private long _ticks;

        private const string STRING_DELIMETER = "\t";

        private const string TARGET_PRICE = "targetprice";
        private const string RECOMMENDATION_TREND = "recommendationtrend";
        private const string CURRENT_PRICE = "currentprice";

        private string GetFilename(string fileType)
        {
            return $"{fileType}_{_ticks}.txt";
        }

        public StorageFile()
        {
            _ticks = DateTime.Now.Ticks;
        }

        private string FormatReport(Report report)
        {
            var json = JsonSerializer.Serialize(report);
            return json;
        }

        public async Task SaveReportAsync(Report report)
        {
            var formattedReport = FormatReport(report);
            await System.IO.File.WriteAllTextAsync("report.json", formattedReport);
        }

        public void SaveReport(Report report)
        {
            throw new NotImplementedException();
        }
    }
}
