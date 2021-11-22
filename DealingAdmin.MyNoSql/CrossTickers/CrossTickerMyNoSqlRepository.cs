using System.Threading.Tasks;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.CrossTickers
{
    public class CrossTickerMyNoSqlRepository :  ICrossTickerRepository
    {
        private readonly IMyNoSqlServerDataWriter<CrossTickerMyNoSqlModel> _myNoSqlTable;

        public CrossTickerMyNoSqlRepository(IMyNoSqlServerDataWriter<CrossTickerMyNoSqlModel> myNoSqlTable)
        {
            _myNoSqlTable = myNoSqlTable;
        }

        public async Task InsertOrReplaceAsync(ICrossTickerModel src)
        {
            await _myNoSqlTable.InsertOrReplaceAsync(CrossTickerMyNoSqlModel.Create(src));
        }
    }
}