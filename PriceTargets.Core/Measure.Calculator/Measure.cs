using PriceTargets.Core.Models.FinanceDataProvider;
using PriceTargets.Core.Models.Measure;
using System;

namespace PriceTargets.Core.Measure.Calculator
{
    public class Measure : Domain.IMeasure
    {
        public PriceExpectationLevel GetPriceExpectationsLevel(CurrentPrice currentPrice, PriceTarget priceTarget)
        {
            var priceExpectationLevel = new PriceExpectationLevel();

            var c = currentPrice.C;

            if (c == 0) return priceExpectationLevel;

            priceExpectationLevel.TargetPriceHighPercent = priceTarget.TargetHigh / c * 100;
            priceExpectationLevel.TargetPriceMeanPercent= priceTarget.TargetMean/ c * 100;
            priceExpectationLevel.TargetPriceMedianPercent = priceTarget.TargetMedian / c * 100;
            priceExpectationLevel.TargetPriceLowPercent = priceTarget.TargetLow / c * 100;

            return priceExpectationLevel;
        }

        public decimal GetTrendExpectationsLevel(RecommendationTrend recommendationTrend)
        {

            decimal n = recommendationTrend.StrongBuy +
                recommendationTrend.Buy +
                recommendationTrend.Hold +
                recommendationTrend.Sell +
                recommendationTrend.StrongSell;

            if (n == 0) return 0;

            decimal trend = (recommendationTrend.StrongBuy * 1 +
                recommendationTrend.Buy * 2 +
                recommendationTrend.Hold * 3 +
                recommendationTrend.Sell * 4 +
                recommendationTrend.StrongSell * 5) / n;

            return Math.Round(trend, 2); ;
        }
    }
}
