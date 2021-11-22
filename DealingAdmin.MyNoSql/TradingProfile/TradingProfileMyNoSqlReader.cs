using System.Collections.Generic;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.TradingProfile
{
    public class TradingProfileMyNoSqlReader : ITradingProfilesReader
    {
        private readonly IMyNoSqlServerDataReader<TradingProfileMyNoSqlEntity> _reader;

        public TradingProfileMyNoSqlReader(IMyNoSqlServerDataReader<TradingProfileMyNoSqlEntity> reader)
        {
            _reader = reader;
        }
        
        public IEnumerable<ITradingProfile> GetAll()
        {
            var partitionKey = TradingProfileMyNoSqlEntity.GeneratePartitionKey();
            return _reader.Get(partitionKey);
        }

        public ITradingProfile Get(string id)
        {
            var partitionKey = TradingProfileMyNoSqlEntity.GeneratePartitionKey();
            var rowKey = TradingProfileMyNoSqlEntity.GenerateRowKey(id);
            return _reader.Get(partitionKey, rowKey);
        }
    }
}