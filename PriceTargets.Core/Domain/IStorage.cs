using System.Threading.Tasks;

namespace PriceTargets.Core.Domain
{
    public interface IStorage
    {
        Task<decimal?> GetAveragePrice(string paper);

        Task SetAveragePrice(string paper, decimal avgPrice);
    }
}
