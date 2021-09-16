using System;

namespace HamstersRocket.Core.Storage.Sqlite.Models
{
    public class TickerEntry
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Industry { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
