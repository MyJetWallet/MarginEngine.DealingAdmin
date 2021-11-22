using System.Collections.Generic;
using System.Linq;
using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;
using DealingAdmin.MyNoSql.TradingProfileInstrument;

namespace DealingAdmin.MyNoSql.TradingProfile
{
    public class TradingProfileMyNoSqlEntity : MyNoSqlDbEntity, ITradingProfile
    {
        
        public static string GeneratePartitionKey()
        {
            return "profile";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public double MarginCallPercent { get; set; }
        public double StopOutPercent { get; set; }
        public double PositionToppingUpPercent { get; set; }
        IEnumerable<ITradingProfileInstrument> ITradingProfile.Instruments => Instruments;
        public List<TradingProfileInstrumentMyNoSqlModel> Instruments { get; set; }


        public static TradingProfileMyNoSqlEntity Create(ITradingProfile src)
        {
            return new TradingProfileMyNoSqlEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                MarginCallPercent = src.MarginCallPercent,
                PositionToppingUpPercent = src.PositionToppingUpPercent,
                StopOutPercent = src.StopOutPercent,
                Instruments = src.Instruments.Select(TradingProfileInstrumentMyNoSqlModel.Create).ToList()
            };
        }
    }
}