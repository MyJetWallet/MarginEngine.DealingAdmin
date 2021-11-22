using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.InstrumentGroup
{
    public class InstrumentGroupMyNoSqlModel : MyNoSqlDbEntity, IInstrumentGroup
    {
        public static string GeneratePartitionKey()
        {
            return "ig";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        
        public string Name { get; set; }
        public int Weight { get; set; }

        public static InstrumentGroupMyNoSqlModel Create(IInstrumentGroup src)
        {
            return new InstrumentGroupMyNoSqlModel
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                Name = src.Name,
                Weight = src.Weight
            };
        }
    }
}