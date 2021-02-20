namespace PriceTargets.Core.FinanceDataProvider.TipRanks.Dto
{
    public class Data
    {
        public string Ticker { get; set; }
        public PtConsensus[] PtConsensus { get; set; }
    }
    public class PtConsensus
    {
        public int Period { get; set; }
        public int Bench { get; set; }
        public decimal? PriceTarget { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }

        public Models.FinanceDataProvider.PriceTarget ToDomain()
        {
            var model = new Models.FinanceDataProvider.PriceTarget()
            {
                TargetHigh = High ?? default,
                TargetLow = Low ?? default,
                TargetMean = PriceTarget ?? default,
                TargetMedian = PriceTarget ?? default,
            };

            return model;
        }
    }
}
