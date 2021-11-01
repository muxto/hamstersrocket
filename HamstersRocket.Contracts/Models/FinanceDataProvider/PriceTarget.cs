namespace HamstersRocket.Contracts.Models.FinanceDataProvider
{
    public class PriceTarget
    {
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Mean { get; set; }
        public decimal Median { get; set; }

        public string Ticker { get; set; }
    }
}
