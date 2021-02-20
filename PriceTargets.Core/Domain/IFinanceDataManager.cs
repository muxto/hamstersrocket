using System.Threading.Tasks;

namespace PriceTargets.Core.Domain
{
    public interface IFinanceDataManager
    {
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        Task<Models.FinanceDataManager.CompanyInfo> GetCompanyInfoAsync(string ticker);
    }
}
