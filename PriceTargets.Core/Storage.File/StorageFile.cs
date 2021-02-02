using System.Threading.Tasks;
using PriceTargets.Core.Domain;
using PriceTargets.Core.Models.FinanceDataProvider;

namespace PriceTargets.Core.Storage.File
{
    public class StorageFile : IStorage
    {
        private string FilePath;
        private string STRING_DELIMETER = "\t";

        public StorageFile(string filePath)
        {
            FilePath = filePath;
        }

        public async Task<decimal?> GetAveragePrice(string paper)
        {
            var lines = await GetLines();
            if (lines == null) return null;

            var num = GetPaperLine(lines, paper);
            if (num < 0) return null;

            if (decimal.TryParse(lines[num].Split(STRING_DELIMETER)[1], out var avgPrice))
            {
                return avgPrice;
            }

            return null;
        }


        private async Task<string[]> GetLines()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                return null;
            }

            var lines = await System.IO.File.ReadAllLinesAsync(FilePath);
            return lines;
        }

        private int GetPaperLine(string[] lines, string paper)
        {
            var key = paper + STRING_DELIMETER;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(key))
                    return i;
            }
            return -1;
        }


        public async Task SetAveragePrice(string paper, decimal avgPrice)
        {
            var newLine = paper.ToString() + STRING_DELIMETER + avgPrice.ToString();
            var newLines = new[] { newLine };


            var lines = await GetLines();
            if (lines == null)
            {
                await System.IO.File.AppendAllLinesAsync(FilePath, newLines);
                return;

            }

            var num = GetPaperLine(lines, paper);
            if (num < 0)
            {
                await System.IO.File.AppendAllLinesAsync(FilePath, newLines);
                return;
            }

            lines[num] = newLine;

            await System.IO.File.WriteAllLinesAsync(FilePath, lines);
        }

        public async Task SavePriceTarget(string ticker, CurrentPrice currentPrice, PriceTarget priceTarget)
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
            
            await System.IO.File.AppendAllLinesAsync(FilePath, lines);
        }
    }
}
