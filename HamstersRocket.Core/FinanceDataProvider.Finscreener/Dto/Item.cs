using System;
using System.Globalization;

namespace HamstersRocket.Core.FinanceDataProvider.Finscreener.Dto
{
    internal class TargetPrice
    {
        public string High { get; set; }
        public string Mean { get; set; }
        public string Low { get; set; }

        public Contracts.Models.FinanceDataProvider.PriceTarget ToDomainPriceTarget()
        {
            var model = new Contracts.Models.FinanceDataProvider.PriceTarget()
            {
                High = TryParse(this.High),
                Low = TryParse(this.Low),
                Mean = TryParse(this.Mean),
                Median = TryParse(this.Mean)
            };

            return model;
        }

        private decimal TryParse(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return 0;
            }

            field = string.Join("", field.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return decimal.Parse(field, CultureInfo.InvariantCulture);
        }
    }

    internal class Item
    {
        public string Rank { get; set; }
        public string Ticker { get; set; }
        public string Price { get; set; }
        public string Potential { get; set; }
        public TargetPrice Current { get; set; }
        public TargetPrice ThreeWeeksAgo { get; set; }
        public TargetPrice Change { get; set; }
        public string AnalystRating { get; set; }
        public string History { get; set; }

        public string Link { get; set; }

        public Contracts.Models.FinanceDataProvider.CurrentPrice ToDomainCurrentPrice()
        {
            var model = new Contracts.Models.FinanceDataProvider.CurrentPrice()
            {
                C = TryParse(this.Price),
                Ticker = this.Ticker
            };

            return model;
        }

        private decimal TryParse(string field)
        {
            if (string.IsNullOrEmpty(field))
            {
                return 0;
            }

            field = string.Join("", field.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return decimal.Parse(field, CultureInfo.InvariantCulture);
        }
    }
}
