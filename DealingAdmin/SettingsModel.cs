using SimpleTrading.SettingsReader;

namespace DealingAdmin
{
    [YamlAttributesOnly]
    public class SettingsModel
    {
        [YamlProperty("DealingAdmin.MyNoSqlRestUrl")]
        public string MyNoSqlRestUrl { get; set; }

        [YamlProperty("DealingAdmin.MyNoSqlTcpUrl")]
        public string MyNoSqlTcpUrl { get; set; }

        [YamlProperty("DealingAdmin.PricesMyServiceBusReader")]
        public string PricesMyServiceBusReader { get; set; }

        [YamlProperty("DealingAdmin.PricesMyNoSqlServerReader")]
        public string PricesMyNoSqlServerReader { get; set; }

        [YamlProperty("DealingAdmin.CandlesHistoryServiceUrl")]
        public string CandlesHistoryServiceUrl { get; set; }

        [YamlProperty("DealingAdmin.CandlesSaveChunkSize")]
        public int CandlesSaveChunkSize { get; set; }

        [YamlProperty("DealingAdmin.CandlesExpiresMinutes")]
        public string CandlesExpiresMinutes { get; set; }

        [YamlProperty("DealingAdmin.CandlesExpiresHours")]
        public string CandlesExpiresHours { get; set; }

        [YamlProperty("DealingAdmin.AzureStorageCandlesConnection")]
        public string AzureStorageCandlesConnection { get; set; }

        [YamlProperty("DealingAdmin.PostgresLiveConnectionString")]
        public string PostgresLiveConnectionString { get; set; }

        [YamlProperty("DealingAdmin.PostgresLiveSchema")]
        public string PostgresLiveSchema { get; set; }

        [YamlProperty("DealingAdmin.PostgresDemoConnectionString")]
        public string PostgresDemoConnectionString { get; set; }

        [YamlProperty("DealingAdmin.PostgresDemoSchema")]
        public string PostgresDemoSchema { get; set; }

        [YamlProperty("DealingAdmin.CrmDataPostgresConnectionString")]
        public string CrmDataPostgresConnString { get; set; }

        [YamlProperty("DealingAdmin.CrmPostgresSchema")]
        public string CrmPostgresSchema { get; set; }

        [YamlProperty("DealingAdmin.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }
    }
}