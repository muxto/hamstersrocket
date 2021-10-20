namespace HamstersRocket.Contracts.Models.FinanceDataManager
{
    public class CompanyInfo
    {
        public string Ticker { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }
        public string Logo { get; set; }
        public Models.FinanceDataProvider.CurrentPrice CurrentPrice { get; set; }
        public Models.FinanceDataProvider.PriceTarget PriceTarget { get; set; }
        public Models.FinanceDataProvider.RecommendationTrend RecommendationTrend { get; set; }
    }
}
