using System;

namespace HamstersRocket.Core.FinanceDataProvider.Finnhub.Dto
{
    public class RecommendationTrend
    {
        public string symbol { get; set; }
        public int strongBuy { get; set; }
        public int buy { get; set; }
        public int hold { get; set; }
        public int sell { get; set; }
        public int strongSell { get; set; }
        public string period { get; set; }

        public Contracts.Models.FinanceDataProvider.RecommendationTrend ToDomain()
        {
            var model = new Contracts.Models.FinanceDataProvider.RecommendationTrend()
            {
                StrongBuy = strongBuy,
                Buy = buy,
                Hold = hold,
                Sell = sell,
                StrongSell = strongSell,
            };

            if (DateTime.TryParse(period, out var datetime))
            {
                model.Period = datetime;
            }

            return model;
        }
    }
}
