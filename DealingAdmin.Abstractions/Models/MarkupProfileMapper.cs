using System.Linq;
using SimpleTrading.Abstraction.Markups;

namespace DealingAdmin.Abstractions.Models
{
    public static class MarkupProfileMapper
    {
        public static MarkupProfileDatabaseModel ToDatabaseModel(this MarkupProfileModel profile)
        {
            var markupItems = profile
                .MarkupInstruments
                .Select(item => (IMarkupItem)item)
                .ToDictionary(markupItem => markupItem.InstrumentId);

            return MarkupProfileDatabaseModel.Create(profile.ProfileId, markupItems);
        }

        public static MarkupProfileModel ToModel(this IMarkupProfile profile)
        {
            return MarkupProfileModel.Create(
                profile.ProfileId,
                profile.MarkupInstruments.Values.Select(MarkupItem.Create));
        }

        public static MarkupProfileModel ToModel(this IIMarkupProfileBase profile)
        {
            return MarkupProfileModel.Create(
                profile.ProfileId,
                profile.MarkupInstruments.Values.Select(MarkupItem.Create));
        }
    }
}