using MyNoSqlServer.Abstractions;
using DealingAdmin.Abstractions;

namespace DealingAdmin.MyNoSql.InstrumentSubGroup
{
    public class InstrumentSubGroupMyNoSqlModel: MyNoSqlDbEntity, IInstrumentSubGroup
    {
        public static string GeneratePartitionKey(string groupId)
        {
            return groupId;
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        
        public string Name { get; set; }

        public string GroupId => PartitionKey;

        public int Weight { get; set; }

        public static InstrumentSubGroupMyNoSqlModel Create(IInstrumentSubGroup src)
        {
            return new InstrumentSubGroupMyNoSqlModel
            {
                PartitionKey = GeneratePartitionKey(src.GroupId),
                RowKey = GenerateRowKey(src.Id),
                Name = src.Name,
                Weight = src.Weight
            };
        }
    }
}