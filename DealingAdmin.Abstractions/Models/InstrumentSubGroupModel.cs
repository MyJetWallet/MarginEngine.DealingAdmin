using System.ComponentModel.DataAnnotations;
using SimpleTrading.Abstraction.Trading.InstrumentsGroup;

namespace DealingAdmin.Abstractions.Models
{
    public class InstrumentSubGroupModel : IInstrumentSubGroup
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string GroupId { get; set; }

        [Required]
        public int Weight { get; set; }

        public static InstrumentSubGroupModel Create(IInstrumentSubGroup src)
        {
            return new InstrumentSubGroupModel
            {
                Id = src.Id,
                Name = src.Name,
                GroupId = src.GroupId,
                Weight = src.Weight
            };
        }
    }
}