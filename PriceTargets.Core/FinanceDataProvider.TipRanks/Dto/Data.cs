using HamstersRocket.Contracts.Models.FinanceDataProvider;

namespace HamstersRocket.Core.FinanceDataProvider.TipRanks.Dto
{
    public class Data
    {
        public string Ticker { get; set; }
        public PtConsensus[] PtConsensus { get; set; }
        public PortfolioHoldingData PortfolioHoldingData { get; set; }
    }
    public class PtConsensus
    {
        public int Period { get; set; }
        public int Bench { get; set; }
        public decimal? PriceTarget { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }

        public PriceTarget ToDomain()
        {
            var model = new PriceTarget()
            {
                TargetHigh = High ?? default,
                TargetLow = Low ?? default,
                TargetMean = PriceTarget ?? default,
                TargetMedian = PriceTarget ?? default,
            };

            return model;
        }
    }

    public class PortfolioHoldingData
    {
        public string SectorId { get; set; }
    }
}
