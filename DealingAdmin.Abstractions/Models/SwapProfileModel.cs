using SimpleTrading.Abstraction.Trading.Swaps;

namespace DealingAdmin.Abstractions.Models
{
    public class SwapProfileModel : ISwapProfile
    {
        public string Id { get; set; }
        
        public string InstrumentId { get; set;}
        
        public double Long { get; set;}
        
        public double Short { get; set;}

        public static SwapProfileModel Create(ISwapProfile src)
        {
            return new SwapProfileModel
            {
                Id = src.Id,
                Long = src.Long,
                Short = src.Short,
                InstrumentId = src.InstrumentId
            };
        }
    }
}