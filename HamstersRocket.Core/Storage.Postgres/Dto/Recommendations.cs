using System;
using System.Collections.Generic;
using System.Text;

namespace HamstersRocket.Core.Storage.Postgres.Dto
{
    internal class Recommendations
    {
        public int Id { get; set; }
        public int Ticker_id { get; set; }
        public int Strong_buy { get; set; }
        public int Buy { get; set; }
        public int Hold { get; set; }
        public int Sell { get; set; }
        public int Strong_sell { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_added { get; set; }
    }
}
