using HamstersRocket.Contracts.Domain;
using HamstersRocket.Contracts.Models.FinanceDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HamstersRocket.Contracts.FinanceDataManager.Main
{
    public class FinanceDataManager : IFinanceDataManager
    {
        private IOutput output;
        private IStorage storage;
        private IFinanceDataProvider[] dataProviders;

        public FinanceDataManager(IOutput output, IFinanceDataProvider[] dataProviders, IStorage storage)
        {
            this.output = output;
            this.dataProviders = dataProviders;
            this.storage = storage;
        }

        public async Task FillData(string[] sourceTickers)
        {
            var finscreener = dataProviders.First(x => x.Provider == FinanceDataProviders.Finscreener);

            var date = DateTime.Now;

            var tickersList = new List<string>();

            foreach (var t in sourceTickers)
            {
                var targetPrice = await storage.GetPriceTargetAsync(t);
                if (targetPrice == null)
                {
                    tickersList.Add(t);
                }
            }

            var tickers = tickersList.ToArray();

            var priceTargets = await finscreener.GetPriceTargetsAsync(tickers);

            foreach (var pt in priceTargets)
            {
                await storage.SetPriceTargetAsync(date, pt.Ticker, pt);
            }

            var currentPrices = await finscreener.GetCurrentPricesAsync(tickers);
            foreach (var cp in currentPrices)
            {
                await storage.SetCurrentPriceAsync(date, cp.Ticker, cp);
            }
        }
        

        public async Task<CompanyInfo> GetCompanyInfoAsync(string ticker)
        {
            var finnhub = dataProviders.First(x => x.Provider == FinanceDataProviders.Finnhub);
            var yahooFinance = dataProviders.First(x => x.Provider == FinanceDataProviders.YahooFinance);

            var currentPrice = await storage.GetCurrentPriceAsync(ticker);
            if (currentPrice == null)
            {
                currentPrice= await yahooFinance.GetCurrentPriceAsync(ticker);
                await storage.SetCurrentPriceAsync(DateTime.Now, ticker, currentPrice);
            }
            
            var recommendationTrend = await storage.GetRecommendationsAsync(ticker);
            if (recommendationTrend == null)
            {
                recommendationTrend = await finnhub.GetRecommendationsAsync(ticker);
                await storage.SetRecommendationsAsync(DateTime.Now, ticker, recommendationTrend);
            }

            var targetPrice = await storage.GetPriceTargetAsync(ticker);
            if (targetPrice == null)
            {
                targetPrice = await yahooFinance.GetPriceTargetAsync(ticker);
                await storage.SetPriceTargetAsync(DateTime.Now, ticker, targetPrice);
            }

            var aboutCompany = await storage.GetAboutCompanyAsync(ticker);
            if (aboutCompany == null)
            {
                aboutCompany = await finnhub.GetAboutCompanyAsync(ticker);
                await storage.SetAboutCompanyAsync(ticker, aboutCompany);
            }

            var companyInfo = new CompanyInfo()
            {
                Ticker = ticker,
                Industry = aboutCompany.Industry,
                CompanyName = aboutCompany.Name,
                Logo = aboutCompany.Logo,
                CurrentPrice = currentPrice,
                PriceTarget = targetPrice,
                Recommendations = recommendationTrend,
            };

            return companyInfo;
        }
    }
}
