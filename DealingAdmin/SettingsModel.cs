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

        [YamlProperty("DealingAdmin.DictionariesMyNoSqlServerWriter")]
        public string DictionariesMyNoSqlServerWriter { get; set; }

        [YamlProperty("DealingAdmin.PricesMyServiceBusReader")]
        public string PricesMyServiceBusReader { get; set; }

        [YamlProperty("DealingAdmin.PricesMyNoSqlServerReader")]
        public string PricesMyNoSqlServerReader { get; set; }

        [YamlProperty("DealingAdmin.AuthGrpcServiceUrl")]
        public string AuthGrpcServiceUrl { get; set; }

        [YamlProperty("DealingAdmin.TradeLogGrpcServiceUrl")]
        public string TradeLogGrpcServiceUrl { get; set; }

        [YamlProperty("DealingAdmin.TradingEngineLiveGrpcServerUrl")]
        public string TradingEngineLiveGrpcServerUrl { get; set; }

        [YamlProperty("DealingAdmin.TradingEngineDemoGrpcServerUrl")]
        public string TradingEngineDemoGrpcServerUrl { get; set; }

        [YamlProperty("DealingAdmin.PersonalDataGrpcServiceUrl")]
        public string PersonalDataGrpcServiceUrl { get; set; }

        [YamlProperty("DealingAdmin.TickHistoryServiceUrl")]
        public string TickHistoryServiceUrl { get; set; }

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

        [YamlProperty("DealingAdmin.QuoteFeedRouterUrl")]
        public string QuoteFeedRouterUrl { get; set; }

        [YamlProperty("DealingAdmin.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("DealingAdmin.ChangeBalanceApiKey")]
        public string ChangeBalanceApiKey { get; set; }

        [YamlProperty("DealingAdmin.AdminCrudApiKey")]
        public string AdminCrudApiKey { get; set; }

        [YamlProperty("DealingAdmin.AdminServerServiceUrl")]
        public string AdminServerServiceUrl { get; set; }
    }
}