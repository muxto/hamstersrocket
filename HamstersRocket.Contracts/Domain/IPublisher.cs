using HamstersRocket.Contracts.Models;
using HamstersRocket.Contracts.Models.Publisher;

namespace HamstersRocket.Contracts.Domain
{
    public interface IPublisher
    {
        Report CreateReport(StockInfo[] stocks);

        string FormatReport(Report report);
    }
}
