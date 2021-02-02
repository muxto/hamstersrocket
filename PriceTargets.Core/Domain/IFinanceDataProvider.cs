using System.Threading.Tasks;

namespace PriceTargets.Core.Domain
{
    public interface IFinanceDataProvider
    {
        Task<Models.FinanceDataProvider.PriceTarget> GetPriceTargetAsync(string ticker);

        Task<Models.FinanceDataProvider.CurrentPrice> GetCurrentPriceAsync(string ticker);
    }
}
