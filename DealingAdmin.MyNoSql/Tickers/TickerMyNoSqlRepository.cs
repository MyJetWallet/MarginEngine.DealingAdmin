using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.Tickers
{
    public class TickerMyNoSqlRepository : ITickerRepository
    {
        private readonly IMyNoSqlServerDataWriter<TickerMyNoSqlModel> _myNoSqlTable;

        public TickerMyNoSqlRepository(IMyNoSqlServerDataWriter<TickerMyNoSqlModel> myNoSqlTable)
        {
            _myNoSqlTable = myNoSqlTable;
        }

        public async Task InsertOrReplaceAsync(ITicker ticker)
        {
            await _myNoSqlTable.InsertOrReplaceAsync(TickerMyNoSqlModel.Create(ticker));
        }
    }
}