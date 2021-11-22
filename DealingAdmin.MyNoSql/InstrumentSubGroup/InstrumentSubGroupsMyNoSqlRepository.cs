using System.Collections.Generic;
using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.InstrumentSubGroup
{
    public class InstrumentSubGroupsMyNoSqlRepository : IInstrumentSubGroupsRepository
    {
        private readonly IMyNoSqlServerDataWriter<InstrumentSubGroupMyNoSqlModel> _table;

        public InstrumentSubGroupsMyNoSqlRepository(IMyNoSqlServerDataWriter<InstrumentSubGroupMyNoSqlModel> table)
        {
            _table = table;
        }

        public async Task<IEnumerable<IInstrumentSubGroup>> GetAllAsync()
        {
            return await _table.GetAsync();
        }

        public async Task<IEnumerable<IInstrumentSubGroup>> GetByGroupIdAsync(string groupId)
        {
            var partitionKey = InstrumentSubGroupMyNoSqlModel.GeneratePartitionKey(groupId);
            return await _table.GetAsync(partitionKey);
        }

        public async Task UpdateAsync(IInstrumentSubGroup item)
        {
            var entity = InstrumentSubGroupMyNoSqlModel.Create(item);
            await _table.InsertOrReplaceAsync(entity);
        }

        public async Task DeleteAsync(string groupId, string subGroupId)
        {
            var partitionKey = InstrumentSubGroupMyNoSqlModel.GeneratePartitionKey(groupId);
            var rowKey = InstrumentSubGroupMyNoSqlModel.GenerateRowKey(subGroupId);
            await _table.DeleteAsync(partitionKey, rowKey);
        }
    }
}