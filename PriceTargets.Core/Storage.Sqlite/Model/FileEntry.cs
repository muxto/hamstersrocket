using System;

namespace veeshnum.Database.Sqlite.Model
{
    public class FileEntry
    {
        public long Id { get; set; }
        public string Source { get; set; }
        public DateTime DateCreate { get; set; }
        public string[] Tags { get; set; }
    }
}