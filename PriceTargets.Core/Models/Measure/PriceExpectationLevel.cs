namespace PriceTargets.Core.Models.Measure
{
    public class PriceExpectationLevel
    {
        public decimal TargetPriceHighPercent { get; set; }
        public decimal TargetPriceMeanPercent { get; set; }
        public decimal TargetPriceMedianPercent { get; set; }
        public decimal TargetPriceLowPercent { get; set; }
    }
}
