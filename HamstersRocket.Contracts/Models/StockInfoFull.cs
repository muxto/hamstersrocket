namespace HamstersRocket.Contracts.Models
{
    public class StockInfoFull : StockInfo
    {
        public StockInfoFull(StockInfo stock) : base()
        {
            this.Ticker = stock.Ticker;
            this.Industry = stock.Industry;
            this.CompanyName = stock.CompanyName;
            this.Logo = stock.Logo;
            this.CurrentPrice = stock.CurrentPrice;
            this.TargetPriceHigh = stock.TargetPriceHigh;
            this.TargetPriceMean = stock.TargetPriceMean;
            this.TargetPriceMedian = stock.TargetPriceMedian;
            this.TargetPriceLow = stock.TargetPriceLow;
            this.StrongBuy = stock.StrongBuy;
            this.Buy = stock.Buy;
            this.Hold = stock.Hold;
            this.Sell = stock.Sell;
            this.StrongSell = stock.StrongSell;

            FillModel();
        }

        public decimal TargetPriceHighPercent { get; set; }
        public decimal TargetPriceMeanPercent { get; set; }
        public decimal TargetPriceMedianPercent { get; set; }
        public decimal TargetPriceLowPercent { get; set; }

        public decimal RecommendationTrend { get; set; }

        public decimal MyChoicePercent { get; set; }

        private void FillModel()
        {
            TargetPriceHighPercent = CurrentPrice / TargetPriceHigh;
            TargetPriceMeanPercent = CurrentPrice / TargetPriceMean;
            TargetPriceMedianPercent = CurrentPrice / TargetPriceMedian;
            TargetPriceLowPercent = CurrentPrice / TargetPriceLow;

            RecommendationTrend =
                (1 * StrongBuy +
                2 * Buy +
                3 * Hold +
                4 * Sell +
                5 * StrongSell) / (StrongBuy + Buy + Hold + Sell + StrongSell);

            MyChoicePercent = GetMyChoice();
        }

        private decimal GetMyChoice()
        {
            if (RecommendationTrend == 0 || RecommendationTrend > 3)
                return 0;
            var percentm = (TargetPriceMeanPercent + TargetPriceMedian) / 2;

            if (percentm < 10)
                return 0;

            return percentm;
        }

    }
}