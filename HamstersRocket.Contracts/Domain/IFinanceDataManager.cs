using HamstersRocket.Contracts.Models.FinanceDataManager;
using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface IFinanceDataManager
    {
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        Task<CompanyInfo> GetCompanyInfoAsync(string ticker);

        Task FillData(string[] tickers);
    }
}
