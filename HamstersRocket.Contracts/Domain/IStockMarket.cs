using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface IStockMarket
    {
        Task<string[]> GetTickersAsync();
    }
}
