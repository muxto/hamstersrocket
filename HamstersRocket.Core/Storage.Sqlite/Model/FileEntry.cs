using System;

namespace HamstersRocket.Core.Storage.Sqlite.Model
{
    public class FileEntry
    {
        public long Id { get; set; }
        public string Source { get; set; }
        public DateTime DateCreate { get; set; }
        public string[] Tags { get; set; }
    }
}