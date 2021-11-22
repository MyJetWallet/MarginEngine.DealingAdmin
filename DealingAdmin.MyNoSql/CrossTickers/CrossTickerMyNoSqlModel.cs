using DealingAdmin.Abstractions;
using MyNoSqlServer.Abstractions;

namespace DealingAdmin.MyNoSql.CrossTickers
{
    public class CrossTickerMyNoSqlModel : MyNoSqlDbEntity, ICrossTickerModel
    {
        public static string GeneratePartitionKey()
        {
            return "crtckr";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string BaseTickerId { get; set; }
        public string QuoteTickerId { get; set; }

        public static CrossTickerMyNoSqlModel Create(ICrossTickerModel src)
        {
            return new CrossTickerMyNoSqlModel
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                BaseTickerId = src.BaseTickerId,
                QuoteTickerId = src.QuoteTickerId
            };
        }
    }
}