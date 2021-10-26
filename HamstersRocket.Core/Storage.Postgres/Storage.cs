using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataProvider;
using System;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using System.Linq;

namespace HamstersRocket.Core.Storage.Postgres
{
    public class Storage : IStorage
    {
        private string dbConnection;

        public Storage(string dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public Task SaveReportToFileAsync(string report)
        {
            throw new NotImplementedException();
        }


        public async Task<AboutCompany> GetAboutCompanyAsync(string ticker)
        {
            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "SELECT name, ticker, industry, logo, date_added " +
                    "FROM companies " +
                    "WHERE ticker = @ticker";

                var param = new
                {
                    ticker = ticker,
                };

                var result = await connection.QueryAsync<Dto.Company>(query, param);
                var c = result?.FirstOrDefault();
                if (c == null)
                {
                    return null;
                }

                return new AboutCompany()
                {
                    Ticker = c.Ticker,
                    Name = c.Name,
                    Industry = c.Industry,
                    Logo = c.Logo
                };
            }
        }

        public async Task SetAboutCompanyAsync(string ticker, AboutCompany aboutCompany)
        {
            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "UPDATE companies SET name = @name, industry = @industry, logo = @logo, date_added = now() WHERE ticker = @ticker;" +
                    "INSERT INTO companies(name, ticker, industry, logo, date_added)" +
                    "SELECT @name, @ticker, @industry, @logo, now()" +
                    "WHERE NOT EXISTS(SELECT 1 FROM companies WHERE ticker = @ticker); ";

                var param = new
                {
                    ticker = ticker,
                    name = aboutCompany.Name,
                    industry = aboutCompany.Industry,
                    logo = aboutCompany.Logo,
                };

                await connection.ExecuteAsync(query, param);
            }
        }

        public async Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            var date = DateTime.Now.Date;

            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "SELECT cp.id, cp.ticker_id, cp.open, cp.close, cp.high, cp.low, cp.previous_close, cp.date, cp.date_added " +
                    "FROM current_prices cp INNER JOIN companies c ON cp.ticker_id = c.id " +
                    "WHERE c.ticker = @ticker AND cp.date = @date; ";

                var param = new
                {
                    ticker = ticker,
                    date = date
                };

                var result = await connection.QueryAsync<Dto.CurrentPrice>(query, param);
                var cp = result?.FirstOrDefault();
                if (cp == null)
                {
                    return null;
                }

                return new CurrentPrice()
                {
                    O = cp.Open,
                    C = cp.Close,
                    H = cp.High,
                    L = cp.Low,
                    PC = cp.Previous_close
                };
            }
        }

        public async Task SetCurrentPriceAsync(DateTime date, string ticker, CurrentPrice currentPrice)
        {
            date = date.Date;

            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "INSERT INTO current_prices(ticker_id, open, close, high, low, previous_close, date, date_added) " +
                    "SELECT c.id, @open, @close, @high, @low, @previous_close, @date, now() " +
                    "FROM companies c " +
                    "WHERE c.ticker = @ticker; ";

                var param = new
                {
                    ticker = ticker,
                    open = currentPrice.O,
                    close = currentPrice .C,
                    high = currentPrice.H,
                    low = currentPrice.L,
                    previous_close = currentPrice.PC,
                    date = date
                };

                await connection.ExecuteAsync(query, param);
            }
        }

        public async Task<Recommendations> GetRecommendationsAsync(string ticker)
        {
            var date = DateTime.Now.Date;

            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "SELECT r.id, r.ticker_id, r.strong_buy, r.buy, r.hold, r.sell, r.strong_sell, r.date, r.date_added " +
                    "FROM recommendations r INNER JOIN companies c ON r.ticker_id = c.id " +
                    "WHERE c.ticker = @ticker AND r.date = @date; ";

                var param = new
                {
                    ticker = ticker,
                    date = date
                };

                var result = await connection.QueryAsync<Dto.Recommendations>(query, param);
                var r = result?.FirstOrDefault();
                if (r == null)
                {
                    return null;
                }

                return new Recommendations()
                {
                    StrongBuy = r.Strong_buy,
                    Buy = r.Buy,
                    Hold = r.Hold,
                    Sell = r.Sell,
                    StrongSell = r.Strong_sell
                };
            }
        }

        public async Task SetRecommendationsAsync(DateTime date, string ticker, Recommendations recommendations)
        {
            date = date.Date;

            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "INSERT INTO recommendations(ticker_id, strong_buy, buy, hold, sell, strong_sell, date, date_added) " +
                    "SELECT c.id, @strong_buy, @buy, @hold, @sell, @strong_sell, @date, now() " +
                    "FROM companies c " +
                    "WHERE c.ticker = @ticker; ";

                var param = new
                {
                    ticker = ticker,
                    strong_buy = recommendations.StrongBuy,
                    buy = recommendations.Buy,
                    hold = recommendations.Hold,
                    sell = recommendations.Sell,
                    strong_sell = recommendations.StrongSell,
                    date = date
                };

                await connection.ExecuteAsync(query, param);
            }
        }

        public async Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            var date = DateTime.Now.Date;

            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "SELECT tp.id, tp.ticker_id, tp.high, tp.low, tp.mean, tp.median, tp.date, tp.date_added " +
                    "FROM target_prices tp INNER JOIN companies c ON tp.ticker_id = c.id " +
                    "WHERE c.ticker = @ticker AND tp.date = @date; ";

                var param = new
                {
                    ticker = ticker,
                    date = date
                };

                var result = await connection.QueryAsync<Dto.TargetPrice>(query, param);
                var tp = result?.FirstOrDefault();
                if (tp == null)
                {
                    return null;
                }

                return new PriceTarget()
                {
                    High = tp.High,
                    Low = tp.Low,
                    Mean = tp.Mean,
                    Median  = tp.Median
                };
            }
        }

        public async Task SetPriceTargetAsync(DateTime date, string ticker, PriceTarget priceTarget)
        {
            date = date.Date;

            using (var connection = new NpgsqlConnection(dbConnection))
            {
                var query =
                    "INSERT INTO target_prices(ticker_id, high, low, mean, median, date, date_added) " +
                    "SELECT c.id, @high, @low, @mean, @median, @date, now() " +
                    "FROM companies c " +
                    "WHERE c.ticker = @ticker; ";

                var param = new
                {
                    ticker = ticker,
                    high = priceTarget.High,
                    low = priceTarget.Low,
                    mean = priceTarget.Mean,
                    median = priceTarget.Median,
                    date = date
                };

                await connection.ExecuteAsync(query, param);
            }
        }
    }
}
