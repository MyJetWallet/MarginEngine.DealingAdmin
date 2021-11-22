using MyNoSqlServer.Abstractions;

namespace DealingAdmin.MyNoSql
{
    public class DefaultValueMyNoSqlModel: MyNoSqlDbEntity
    {
        public static string GeneratePartitionKey()
        {
            return "dv";
        }
         
        public static string GenerateRowKeyAsLiquidityProviderId()
        {
            return "LiquidityProviderId";
        }
        
        public string Value { get; set; }
        
        public string[] Values { get; set; }
    }
}