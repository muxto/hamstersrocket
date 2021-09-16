using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.Publisher;
using Microsoft.Data.Sqlite;
using Dapper;
using HamstersRocket.Core.Storage.Sqlite.Models;

namespace HamstersRocket.Core.Storage.Sqlite
{
    public class Storage : IStorage
    {
        private const string DATABASE_NAME = "hamstersrocket.db";

        private string DbFile;

        // private Logger logger;

        public Storage(string path)
        {
            DbFile = Path.Combine(path + "\\" + DATABASE_NAME);
            DbFile = DbFile.TrimStart('\\');
            //logger = LogManager.Logger;
        }

        private SqliteConnection SimpleDbConnection()
        {
            return new SqliteConnection("Data Source=" + DbFile);
        }

        private async Task CreateDatabaseAsync()
        {
            // logger.AddInfo("Create database " + DATABASE_NAME);

            using (var cnn = SimpleDbConnection())
            {
                await cnn.OpenAsync();
                await cnn.ExecuteAsync(
                    @"create table Tickers
                     (
                        Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
                        Ticker                  TEXT NOT NULL UNIQUE,
                        Industry                TEXT,
                        DateCreation            DEFAULT CURRENT_TIMESTAMP
                     );
                     create table PricesAndRecommendations
                     (
                        Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
                        TickerId                INTEGER UNIQUE NOT NULL REFERENCES Tickers(Id) ON DELETE CASCADE,
                        Date                    TEXT NOT NULL,
                        PriceLow                REAL,
                        PriceHigh               REAL,
                        TargetPriceLow          REAL,
                        TargetPriceMean         REAL,
                        TargetPriceHigh         REAL,
                        StrongBuy               INTEGER,
                        Buy                     INTEGER,
                        Hold                    INTEGER,
                        Sell                    INTEGER,
                        StrongSell              INTEGER,
                        DateCreation            DEFAULT CURRENT_TIMESTAMP,
                        
                        UNIQUE(TickerId,Date));");

                // logger.AddInfo("Database created");
            }
        }

        public async Task SaveReportAsync(Report report)
        {
            if (!System.IO.File.Exists(DbFile))
            {
                await CreateDatabaseAsync();
            }

            using (var cnn = SimpleDbConnection())
            {
                await cnn.OpenAsync();

                foreach (var stock in report.Stocks)
                {
                    var obj = new
                    {
                        Ticker = stock.Ticker,

                        Industry = stock.Industry,

                        Date = report.UpdateDate.ToShortDateString(),
                    
                        PriceLow = stock.PriceLow,
                        PriceHigh = stock.PriceHigh,

                        TargetPriceLow = stock.TargetPriceLow,
                        TargetPriceMean = stock.TargetPriceMean,
                        TargetPriceHigh = stock.TargetPriceHigh,

                        StrongBuy = stock.StrongBuy,
                        Buy = stock.Buy,
                        Hold = stock.Hold,
                        Sell = stock.Sell,
                        StrongSell = stock.StrongSell
                    };

                    await cnn.ExecuteAsync(
                        @"INSERT OR IGNORE INTO Tickers(Ticker, Industry) VALUES ( @Ticker, @Industry);

                        INSERT OR IGNORE INTO PricesAndRecommendations(
                            TickerId,
                            Date,
                            PriceLow,
                            PriceHigh,
                            TargetPriceLow,
                            TargetPriceMean,
                            TargetPriceHigh,
                            StrongBuy,
                            Buy,
                            Hold,
                            Sell,
                            StrongSell)
                        SELECT t.Id TickerId, 
                            @Date, 
                            @PriceLow, 
                            @PriceHigh, 
                            @TargetPriceLow,
                            @TargetPriceMean,
                            @TargetPriceHigh, 
                            @StrongBuy,
                            @Buy,
                            @Hold,
                            @Sell,
                            @StrongSell 
                        FROM Tickers t WHERE t.Ticker = @Ticker", obj);
                }

                // logger.AddInfo("done");
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

        public object GetFile(string[] tags)
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
