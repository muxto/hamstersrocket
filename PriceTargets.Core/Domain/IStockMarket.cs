using System.Threading.Tasks;

namespace PriceTargets.Core.Domain
{
    public interface IStockMarket
    {
        Task<string[]> GetTickers();
    }
}
