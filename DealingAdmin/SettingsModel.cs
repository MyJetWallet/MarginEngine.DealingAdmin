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

        [YamlProperty("DealingAdmin.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }
    }
}