using System.Threading.Tasks;
using PriceTargets.Core.Models.FinanceDataProvider;

namespace PriceTargets.Core.Domain
{
    public interface IStorage
    {
        Task SaveReportAsync(string report);
    }
}