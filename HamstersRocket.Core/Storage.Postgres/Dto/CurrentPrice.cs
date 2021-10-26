using System;
using System.Collections.Generic;
using System.Text;

namespace HamstersRocket.Core.Storage.Postgres.Dto
{
    internal class CurrentPrice
    {
        public int Id { get; set; }
        public int Ticker_id { get; set; }
        public decimal Open{ get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Previous_close { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_added { get; set; }
    }
}
