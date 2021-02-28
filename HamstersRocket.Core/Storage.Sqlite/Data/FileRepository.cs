using Dapper;
using HamstersRocket.Core.Storage.Sqlite.Model;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;

namespace HamstersRocket.Core.Storage.Sqlite.Data
{
    public class FileRepository : IFileRepository
    {
        private const string DATABASE_NAME = "veeshnum";

        private string DbFile;

        // private Logger logger;

        public FileRepository(string path)
        {
            DbFile = Path.Combine(path + "\\" + DATABASE_NAME);
            //logger = LogManager.Logger;
        }

        private SqliteConnection SimpleDbConnection()
        {
            return new SqliteConnection("Data Source=" + DbFile);
        }

        private void CreateDatabase()
        {
            //        logger.AddInfo("Create database " + DATABASE_NAME);

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();
                cnn.Execute(
                    @"create table Files
                     (
                        Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
                        Source                  TEXT NOT NULL,
                        DateCreation            DEFAULT CURRENT_TIMESTAMP
                     );
                     create table Tags
                     (
                        Id                       INTEGER PRIMARY KEY,
                        Tag                      TEXT UNIQUE NOT NULL,
                        DateCreation             DEFAULT CURRENT_TIMESTAMP
                     );
                     create table FileTags
                     (
                        FileId                   INTEGER NOT NULL REFERENCES Files(Id) ON DELETE CASCADE,
                        TagId                    INTEGER NOT NULL REFERENCES Tags(Id)  ON DELETE CASCADE,
                        DateCreation             DEFAULT CURRENT_TIMESTAMP
                     );");

                //              logger.AddInfo("Database created");
            }
        }

        public long SaveFile(string source)
        {
            if (!System.IO.File.Exists(DbFile))
            {
                CreateDatabase();
            }

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();

                var param = new { Source = source };

                var id = cnn.Query<long>(
                    @"INSERT INTO Files 
                    ( Source ) VALUES 
                    ( @Source );
                    select last_insert_rowid()", param).First();

                //       logger.AddInfo($"File {source} added to database");

                return id;
            }
        }

        public void AddTags(long id, string[] tags)
        {
            if (!System.IO.File.Exists(DbFile))
            {
                //             logger.AddInfo($"Has no database");
                return;
            }

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();

                var sqLiteTransaction = cnn.BeginTransaction();

                int lines = 0;
                foreach (var tag in tags)
                {
                    var param = new { Id = id, Tag = tag };

                    lines = cnn.Execute(@"
                        INSERT OR IGNORE INTO Tags(Tag) 
                        SELECT @Tag
                            WHERE EXISTS (SELECT 1 FROM Files WHERE Id = @Id);

                        INSERT INTO FileTags(FileId, TagId)
                        SELECT @Id Id, t.Id TagId
                            FROM Tags t
                            WHERE EXISTS (SELECT 1 FROM Files WHERE Id = @Id)
                                AND t.Tag = @Tag;", param);
                }
                sqLiteTransaction.Commit();
                if (lines > 0)
                {
                    //               logger.AddInfo($"File {id} added {tags.Length} tags to database");
                }
                else
                {
                    //              logger.AddInfo($"No processed rows");
                }
            }
        }

        public FileEntry GetFile(string[] tags)
        {
            if (!System.IO.File.Exists(DbFile))
            {
                //            logger.AddInfo($"Has no database");
                return null;
            }

            using (var cnn = SimpleDbConnection())
            {
                cnn.Open();

                var param = new { Source = tags };

                var results = cnn.Query<long>(
                    @"SELECT * FROM Tags WHERE Tag IN @tags", tags);

                //  logger.AddInfo($"File {source} added to database");

                return null;
            }

        }
    }
}
