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
    }
}