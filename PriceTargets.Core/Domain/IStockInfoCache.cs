using PriceTargets.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PriceTargets.Core.Domain
{
    public interface IStockInfoCache : ICache<StockInfo>
    {
    }
}
