using System.Threading.Tasks;
using PriceTargets.Core.Domain;

namespace PriceTargets.Core.Storage.File
{
    public class StorageFile : IStorage
    {
        private string FilePath;
        private string Delimeter = "\t";

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

            if (decimal.TryParse(lines[num].Split(Delimeter)[1], out var avgPrice))
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
            var key = paper + Delimeter;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(key))
                    return i;
            }
            return -1;
        }


        public async Task SetAveragePrice(string paper, decimal avgPrice)
        {
            var newLine = paper.ToString() + Delimeter + avgPrice.ToString();
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


    }
}
