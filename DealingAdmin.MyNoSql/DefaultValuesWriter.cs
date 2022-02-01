using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;
using SimpleTrading.QuotesFeedRouter.Abstractions;

namespace DealingAdmin.MyNoSql
{
    public class DefaultValuesWriter : IDefaultLiquidityProviderWriter
    {
        private readonly IMyNoSqlServerDataWriter<DefaultValueMyNoSqlModel> _table;

        public DefaultValuesWriter(IMyNoSqlServerDataWriter<DefaultValueMyNoSqlModel> table)
        {
            _table = table;
        }
 
        ValueTask IDefaultLiquidityProviderWriter.SetAsync(string value)
        {
            var entity = new DefaultValueMyNoSqlModel
            {
                PartitionKey = DefaultValueMyNoSqlModel.GeneratePartitionKey(),
                RowKey = DefaultValueMyNoSqlModel.GenerateRowKeyAsLiquidityProviderId(),
                Value = value
            };

            return _table.InsertOrReplaceAsync(entity);
        }


        async ValueTask<string> IDefaultLiquidityProviderWriter.GetAsync()
        {
            var pk = DefaultValueMyNoSqlModel.GeneratePartitionKey();
            var rk = DefaultValueMyNoSqlModel.GenerateRowKeyAsLiquidityProviderId();

            return (await _table.GetAsync(pk, rk))?.Value;
        }
    }
}