using HamstersRocket.Contracts.Domain;
using System.Threading.Tasks;

namespace HamstersRocket.Core.Storage.File
{
    public class StorageFile : IStorage
    {
        public async Task SaveReportAsync(string report)
        {
            await System.IO.File.WriteAllTextAsync("report.json", report);
        }
    }
}
