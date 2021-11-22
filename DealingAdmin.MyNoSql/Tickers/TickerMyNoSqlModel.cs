using System.Collections.Generic;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.Tickers
{
    public class TickerMyNoSqlModel : MyNoSqlDbEntity, ITicker
    {
        public static string GeneratePartitionKey()
        {
            return "tckr";
        }

        public static string GenerateRowKey(string instrumentId)
        {
            return instrumentId;
        }

        public string Id => RowKey;
        public int Digits { get; set; }
        public double TickSize { get; set; }
        public IList<ITickerSourceSettings> SourceSettings { get; set; }

        public static TickerMyNoSqlModel Create(ITicker src)
        {
            return new TickerMyNoSqlModel
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                TickSize = src.TickSize,
                SourceSettings = src.SourceSettings
            };
        }
    }
}