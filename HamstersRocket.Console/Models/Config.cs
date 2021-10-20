namespace HamstersRocket.ConsoleApp.Models
{
    internal class Config
    {
        public ConfigItem Tinkoff { get; set; }
        public ConfigItem Finnhub { get; set; }
        public ConfigItem AlphaVantage { get; set; }
        public ConfigItem YahooFinance { get; set; }
        public ConfigItem Postgres { get; set; }
    }

    internal class ConfigItem
    {
        public string DbConnection { get; set; }
        public string[] Tokens { get; set; }
        public int? Delay { get; set; }
    }
}
