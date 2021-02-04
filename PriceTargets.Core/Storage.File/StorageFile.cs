using System;
using System.Threading.Tasks;
using PriceTargets.Core.Domain;
using PriceTargets.Core.Models.FinanceDataProvider;

namespace PriceTargets.Core.Storage.File
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

        public async Task SaveCurrentPriceAsync(string ticker, CurrentPrice currentPrice)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(ticker);
            sb.Append(STRING_DELIMETER);

            sb.Append(currentPrice.O);
            sb.Append(STRING_DELIMETER);
            sb.Append(currentPrice.H);
            sb.Append(STRING_DELIMETER);
            sb.Append(currentPrice.L);
            sb.Append(STRING_DELIMETER);
            sb.Append(currentPrice.C);
            sb.Append(STRING_DELIMETER);
            sb.Append(currentPrice.PC);
            sb.Append(STRING_DELIMETER);

            var newLine = sb.ToString();

            var lines = new[] { newLine };

            await System.IO.File.AppendAllLinesAsync(GetFilename(CURRENT_PRICE), lines);
        }

        public async Task SavePriceTargetAsync(string ticker, PriceTarget priceTarget)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(ticker);
            sb.Append(STRING_DELIMETER);

            sb.Append(priceTarget.TargetHigh);
            sb.Append(STRING_DELIMETER);
            sb.Append(priceTarget.TargetLow);
            sb.Append(STRING_DELIMETER);
            sb.Append(priceTarget.TargetMedian);
            sb.Append(STRING_DELIMETER);
            sb.Append(priceTarget.TargetMean);
            sb.Append(STRING_DELIMETER);
            sb.Append(priceTarget.LastUpdated.ToShortDateString());
            sb.Append(STRING_DELIMETER);

            var newLine = sb.ToString();

            var lines = new[] { newLine };

            await System.IO.File.AppendAllLinesAsync(GetFilename(TARGET_PRICE), lines);
        }

        public async Task SaveRecommendationTrendAsync(string ticker, RecommendationTrend recommendationTrend)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(ticker);
            sb.Append(STRING_DELIMETER);

            sb.Append(recommendationTrend.StrongBuy);
            sb.Append(STRING_DELIMETER);
            sb.Append(recommendationTrend.Buy);
            sb.Append(STRING_DELIMETER);
            sb.Append(recommendationTrend.Hold);
            sb.Append(STRING_DELIMETER);
            sb.Append(recommendationTrend.Sell);
            sb.Append(STRING_DELIMETER);
            sb.Append(recommendationTrend.StrongSell);
            sb.Append(STRING_DELIMETER);
            sb.Append(recommendationTrend.Period.ToShortDateString());
            sb.Append(STRING_DELIMETER);

            var newLine = sb.ToString();

            var lines = new[] { newLine };

            await System.IO.File.AppendAllLinesAsync(GetFilename(RECOMMENDATION_TREND), lines);
        }

        public async Task SaveReportAsync(string report)
        {
            await System.IO.File.WriteAllTextAsync("report.json", report);
        }
    }
}
