namespace HamstersRocket.Contracts.Models
{
    public class StockInfoViewModel
    {
        public StockInfoViewModel(StockInfoFull stockInfoFull)
        {
            FillModel(stockInfoFull);
        }

        public string MyChoice { get; set; }
        public string Ticker { get; set; }
        public string Industry { get; set; }
        public string CurrentPrice { get; set; }
        public string TargetPrices { get; set; }
        public string TargetPercents { get; set; }
        public string Rs { get; set; }
        public string RT { get; set; }


        private void FillModel(StockInfoFull stockInfoFull)
        {
            var p = stockInfoFull.CurrentPrice + stockInfoFull.CurrentPrice * (stockInfoFull.MyChoicePercent / 100);
            MyChoice = $"{ p.ToString("F2")} ( { stockInfoFull.MyChoicePercent.ToString("F0")} %)";

            Ticker = stockInfoFull.Ticker;
            Industry = stockInfoFull.Industry;
            CurrentPrice = stockInfoFull.CurrentPrice.ToString("F2");

            TargetPrices = stockInfoFull.TargetPriceLow.ToString("F2");
            TargetPercents = stockInfoFull.TargetPriceLowPercent.ToString("F0");
            if (stockInfoFull.TargetPriceLow != stockInfoFull.TargetPriceHigh)
            {
                var priceM = (stockInfoFull.TargetPriceMean + stockInfoFull.TargetPriceMedian) / 2;
                var percentM = (stockInfoFull.TargetPriceMeanPercent + stockInfoFull.TargetPriceMedianPercent) / 2;

                TargetPrices += $" - { priceM.ToString("F2")} - { stockInfoFull.TargetPriceHigh.ToString("F2")}";
                TargetPercents += $" - { percentM.ToString("F0")} - { stockInfoFull.TargetPriceHighPercent.ToString("F0")}";
            }

            Rs = $"{ stockInfoFull.StrongBuy}, { stockInfoFull.Buy}, { stockInfoFull.Hold}, { stockInfoFull.Sell}, { stockInfoFull.StrongSell}";

            RT = $"{ stockInfoFull.RecommendationTrend.ToString("F2")}";
        }
    }
}
