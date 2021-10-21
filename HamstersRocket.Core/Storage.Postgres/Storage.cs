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

        public Task<CurrentPrice> GetCurrentPriceAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetCurrentPriceAsync(DateTime date, string ticker, CurrentPrice currentPrice)
        {
            throw new NotImplementedException();
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

        public Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetPriceTargetAsync(DateTime date, string ticker, PriceTarget priceTarget)
        {
            throw new NotImplementedException();
        }
    }
}
