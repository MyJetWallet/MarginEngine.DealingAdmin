using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealingAdmin.Validators
{
    public class TradingProfileNewInstrumentsValidator : ITradingProfileNewInstrumentsValidator
    {
        private readonly LiveDemoServiceMapper _liveDemoServices;
        private readonly Logger _logger;

        public TradingProfileNewInstrumentsValidator(
            LiveDemoServiceMapper liveDemoServices,
            Logger logger)
        {
            _liveDemoServices = liveDemoServices;
            _logger = logger;
        }

        public async Task<string> ValidateTradingProfileNewInstruments(TradingProfileModel request, bool isLive)
        {
            var newInstruments = request.Instruments.Select(x => x.Id).ToList();
            var allProfiles = (await _liveDemoServices.GetContext(isLive).TradingProfileRepository.GetAllAsync());
            var profileId = request.Id;

            if (allProfiles.Count(x => x.Id == profileId) == 0)
            {
                return String.Empty;
            }

            var currentInstruments = allProfiles.First(x => x.Id == profileId).Instruments.Select(x => x.Id).ToList();

            var deletedInstruments = currentInstruments.Except(newInstruments).ToList();

            if (deletedInstruments.Count > 0)
            {
                var usedInstruments = new List<string>();

                var boundTradingGroups = (await _liveDemoServices.GetContext(isLive).TradingGroupsRepository.GetAllAsync())
                    .Where(group => group.TradingProfileId == profileId)?.Select(x => x.Id).ToList();

                var profileActivePositionsInstruments =
                    (await _liveDemoServices.GetContext(isLive).StReader
                        .GetActivePositionsInstrumentsByTradingGroups(boundTradingGroups)).ToList();

                usedInstruments.AddRange(deletedInstruments.Intersect(profileActivePositionsInstruments));

                if (usedInstruments.Count > 0)
                {
                    return $"Cannot remove the following instruments that are used in active orders:"
                            + $"{Environment.NewLine} {String.Join(", ", usedInstruments.ToArray())}";
                }

                var allPendingOrdersInstruments = (await _liveDemoServices.GetContext(isLive).StReader.
                    GetPendingOrdersInstrumentsByTradingGroups(boundTradingGroups)).ToList();

                usedInstruments.AddRange(deletedInstruments.Intersect(allPendingOrdersInstruments));

                if (usedInstruments.Count > 0)
                {
                    return $"Cannot remove the following instruments that are used in pending orders:"
                            + $"{Environment.NewLine} {String.Join(", ", usedInstruments.ToArray())}";
                }

                var closedPositionsInstruments = (await _liveDemoServices.GetContext(isLive).StReader.
                    GetClosedPositionsInstrumentsByTradingGroups(boundTradingGroups)).ToList();

                usedInstruments.AddRange(deletedInstruments.Intersect(closedPositionsInstruments));

                if (usedInstruments.Count > 0)
                {
                    return $"Cannot remove the following instruments that were used in closed orders:"
                            + $"{Environment.NewLine} {String.Join(", ", usedInstruments.ToArray())}";
                }
            }

            return String.Empty;
        }
    }
}