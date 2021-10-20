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

        public Task SetCurrentPriceAsync(DateTime date, string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<PriceTarget> GetPriceTargetAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task<RecommendationTrend> GetRecommendationTrendAsync(string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SaveReportToFileAsync(string report)
        {
            throw new NotImplementedException();
        }

        public Task SetPriceTargetAsync(DateTime date, string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetRecommendationTrendAsync(DateTime date, string ticker)
        {
            throw new NotImplementedException();
        }

        public Task SetCurrentPriceAsync(DateTime date, string ticker, CurrentPrice currentPrice)
        {
            throw new NotImplementedException();
        }

        public Task SetRecommendationTrendAsync(DateTime date, string ticker, RecommendationTrend recommendationTrend)
        {
            throw new NotImplementedException();
        }

      

        public Task SetPriceTargetAsync(DateTime date, string ticker, PriceTarget priceTarget)
        {
            throw new NotImplementedException();
        }
    }
}
