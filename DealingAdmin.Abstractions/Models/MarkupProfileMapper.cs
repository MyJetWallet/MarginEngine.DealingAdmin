using System.Linq;
using SimpleTrading.Abstraction.Markups;
using SimpleTrading.Abstraction.Markups.TradingGroupMarkupProfiles;

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

        public static MarkupProfileDatabaseModel ToDatabaseModel(this MarkupProfileExtModel profile)
        {
            var markupItems = profile
                .MarkupInstruments
                .Select(item => (IMarkupItem)item)
                .ToDictionary(markupItem => markupItem.InstrumentId);

            return MarkupProfileDatabaseModel.Create(profile.ProfileId, markupItems);
        }

        public static MarkupProfilePropertiesModel ToPropertiesModel(this MarkupProfileExtModel profile)
        {
            return new MarkupProfilePropertiesModel
            {
                ProfileId = profile.ProfileId,
                Description = profile.Description,
                IsHidden = profile.IsHidden,
            };
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


        public static MarkupProfileExtModel ToExtModel(this ITradingGroupMarkupProfile profile, ITradingGroupMarkupProfileProperties props)
        {
            return new MarkupProfileExtModel
            {
                ProfileId = profile.ProfileId,
                MarkupInstruments = profile.MarkupInstruments.Values.Select(MarkupItem.Create).ToList(),
                Description = props?.Description,
                IsHidden = (props != null && props.IsHidden),
            };
        }
    }
}