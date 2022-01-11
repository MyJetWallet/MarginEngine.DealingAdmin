using System.ComponentModel.DataAnnotations;
using SimpleTrading.Abstraction.Trading.InstrumentsGroup;

namespace DealingAdmin.Abstractions.Models
{
    public class InstrumentGroupModel : IInstrumentGroup
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public int Weight { get; set; }

        public static InstrumentGroupModel Create(IInstrumentGroup src)
        {
            return new InstrumentGroupModel
            {
                Id = src.Id,
                Name = src.Name,
                Weight = src.Weight
            };
        }
    }
}