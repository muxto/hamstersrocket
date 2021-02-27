using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface IStorage
    {
        Task SaveReportAsync(string report);
    }
}