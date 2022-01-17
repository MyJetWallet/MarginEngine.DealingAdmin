using System.Collections.Generic;

namespace DealingAdmin.Abstractions.Models
{
    public class SwapGroupProfile
    {
        public string ProfileId { get; set; }

        public List<SwapProfileModel> Instruments { get; set; }
    }
}