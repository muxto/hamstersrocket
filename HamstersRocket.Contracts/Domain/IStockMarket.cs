using System.Threading.Tasks;

namespace HamstersRocket.Contracts.Domain
{
    public interface IStockMarket
    {
        Task<string[]> GetTickersAsync();

        // TODO test for sqlite
        Task<HamstersRocket.Contracts.Models.Candle[]> GetHistoricCandlesAsync(string ticker, int monthsAgo);
    }
}
