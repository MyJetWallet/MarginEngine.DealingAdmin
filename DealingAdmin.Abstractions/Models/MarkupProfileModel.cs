using System.Collections.Generic;
using System.Linq;
using SimpleTrading.Abstraction.Markups;
using SimpleTrading.Abstraction.Markups.TradingGroupMarkupProfiles;

namespace DealingAdmin.Abstractions.Models
{
    public class MarkupProfileModel
    {
        public string ProfileId { get; set; }
       
        public List<MarkupItem> MarkupInstruments { get; set; }

        public static MarkupProfileModel Create(string profileId, IEnumerable<MarkupItem> items)
        {
            return new MarkupProfileModel
            {
                ProfileId = profileId,
                MarkupInstruments = items.ToList()
            };
        }
    }

    public class MarkupProfilePropertiesModel : ITradingGroupMarkupProfileProperties
    {
        public string ProfileId { get; set; }

        public string Description { get; set; }

        public bool IsHidden { get; set; }

        public static MarkupProfilePropertiesModel Create(ITradingGroupMarkupProfileProperties src)
        {
            return new MarkupProfilePropertiesModel
            {
                ProfileId = src.ProfileId,
                Description = src.Description,
                IsHidden = src.IsHidden,
            };
        }
    }

    public class MarkupProfileExtModel
    {
        public string ProfileId { get; set; }

        public List<MarkupItem> MarkupInstruments { get; set; }

        public string Description { get; set; }

        public bool IsHidden { get; set; }
    }


    public class MarkupProfileDatabaseModel : IIMarkupProfileBase, IMarkupProfile, ITradingGroupMarkupProfile
    {
        public string ProfileId { get; set; }
        
        public IReadOnlyDictionary<string, IMarkupItem> MarkupInstruments { get; set; }
        
        IReadOnlyDictionary<string, IMarkupItem> IIMarkupProfileBase.MarkupInstruments => MarkupInstruments;

        public static MarkupProfileDatabaseModel Create(
            string profileId,
            IReadOnlyDictionary<string, IMarkupItem> items)
        {
            return new MarkupProfileDatabaseModel
            {
                ProfileId = profileId,
                MarkupInstruments = items
            };
        }
    }

    public class MarkupItem : IMarkupItem
    {
        public string InstrumentId { get; set; }
        
        public int MarkupBid { get; set; }
        
        public int MarkupAsk { get; set; }

        public static MarkupItem Create(string instrumentId, int markupBid, int markupAsk)
        {
            return new MarkupItem
            {
                InstrumentId = instrumentId,
                MarkupBid = markupBid,
                MarkupAsk = markupAsk
            };
        }

        public static MarkupItem Create(IMarkupItem item)
        {
            return new MarkupItem
            {
                InstrumentId = item.InstrumentId,
                MarkupAsk = item.MarkupAsk,
                MarkupBid = item.MarkupBid
            };
        }
    }
}