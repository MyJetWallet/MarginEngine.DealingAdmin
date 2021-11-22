using System.Collections.Generic;
using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.TradingProfile
{
    public class TradingProfilesMyNoSqlRepository : ITradingProfileRepository
    {
        private readonly IMyNoSqlServerDataWriter<TradingProfileMyNoSqlEntity> _table;

        public TradingProfilesMyNoSqlRepository(IMyNoSqlServerDataWriter<TradingProfileMyNoSqlEntity> table)
        {
            _table = table;
        }
        
        public async Task<IEnumerable<ITradingProfile>> GetAllAsync()
        {
            var partitionKey = TradingProfileMyNoSqlEntity.GeneratePartitionKey();
            return await _table.GetAsync(partitionKey);
        }

        public async Task UpdateAsync(ITradingProfile tradingGroup)
        {
            var entity = TradingProfileMyNoSqlEntity.Create(tradingGroup);
            await _table.InsertOrReplaceAsync(entity);
        }
    }
}