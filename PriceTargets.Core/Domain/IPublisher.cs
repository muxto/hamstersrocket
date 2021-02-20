using PriceTargets.Core.Models;
using PriceTargets.Core.Models.FinanceDataProvider;
using PriceTargets.Core.Models.Measure;
using PriceTargets.Core.Models.Publisher;

namespace PriceTargets.Core.Domain
{
    public interface IPublisher
    {
        Report CreateReport(StockInfo[] stocks);

        string FormatReport(Report report);
    }
}
