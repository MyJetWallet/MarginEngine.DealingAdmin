using System.Collections.Generic;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.Tickers
{
    public class TickerMyNoSqlReader : ITickerReader
    {
        private readonly IMyNoSqlServerDataReader<TickerMyNoSqlModel> _read;

        public TickerMyNoSqlReader(IMyNoSqlServerDataReader<TickerMyNoSqlModel> read)
        {
            _read = read;
        }

        public IEnumerable<ITicker> GetAll()
        {
            return _read.Get();
        }

        public ITicker GetById(string ticker)
        {
            var pk = TickerMyNoSqlModel.GeneratePartitionKey();
            var rk = TickerMyNoSqlModel.GenerateRowKey(ticker);
            return _read.Get(pk, rk);
        }
    }
}