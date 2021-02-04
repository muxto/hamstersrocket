namespace PriceTargets.Core.Models.FinanceDataProvider
{
    public class CurrentPrice
    {
        // Open price of the day
        public decimal O { get; set; }
        
        // High price of the day
        public decimal  H { get; set; }

        // Low price of the day
        public decimal  L { get; set; }

        // Current price
        public decimal  C { get; set; }

        // Previous close price
        public decimal PC { get; set; }
    }
}
