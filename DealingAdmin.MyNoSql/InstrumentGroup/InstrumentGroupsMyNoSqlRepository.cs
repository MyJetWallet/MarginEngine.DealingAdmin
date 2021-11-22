using System.Collections.Generic;
using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.InstrumentGroup
{
    public class InstrumentGroupsMyNoSqlRepository : IInstrumentGroupsRepository
    {
        private readonly IMyNoSqlServerDataWriter<InstrumentGroupMyNoSqlModel> _table;

        public InstrumentGroupsMyNoSqlRepository(IMyNoSqlServerDataWriter<InstrumentGroupMyNoSqlModel> table)
        {
            _table = table;
        }

        public async Task<IEnumerable<IInstrumentGroup>> GetAllAsync()
        {
            var partitionKey = InstrumentGroupMyNoSqlModel.GeneratePartitionKey();
            return await _table.GetAsync(partitionKey);
        }

        public async Task UpdateAsync(IInstrumentGroup item)
        {
            var entity = InstrumentGroupMyNoSqlModel.Create(item);
            await _table.InsertOrReplaceAsync(entity);
        }
    }
}