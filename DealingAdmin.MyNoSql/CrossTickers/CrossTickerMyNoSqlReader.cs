using System.Collections.Generic;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.CrossTickers
{
    public class CrossTickerMyNoSqlReader :  ICrossTickerReader
    {
        private readonly IMyNoSqlServerDataReader<CrossTickerMyNoSqlModel> _read;

        public CrossTickerMyNoSqlReader(IMyNoSqlServerDataReader<CrossTickerMyNoSqlModel> read)
        {
            _read = read;
        }

        public IEnumerable<ICrossTickerModel> GetAll()
        {
            return _read.Get();
        }
    }
}