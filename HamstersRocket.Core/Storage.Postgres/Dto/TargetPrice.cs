using System;

namespace HamstersRocket.Core.Storage.Postgres.Dto
{
    internal class TargetPrice
    {
        public int Id { get; set; }
        public int Ticker_id { get; set; }
        public decimal High{ get; set; }
        public decimal Low { get; set; }
        public decimal Mean { get; set; }
        public decimal Median { get; set; }
        public DateTime Date { get; set; }
        public DateTime Date_added { get; set; }
    }
}
