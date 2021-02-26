using System.Threading.Tasks;

namespace PriceTargets.Core.Domain
{
    public interface IStorage
    {
        Task SaveReportAsync(string report);
    }
}