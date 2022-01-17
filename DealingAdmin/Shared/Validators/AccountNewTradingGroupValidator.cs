using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealingAdmin.Validators
{
    public class AccountNewTradingGroupValidator : IAccountNewTradingGroupValidator
    {
        private readonly LiveDemoServiceMapper _liveDemoServices;
        private readonly Logger _logger;

        public AccountNewTradingGroupValidator(LiveDemoServiceMapper liveDemoServices, Logger logger)
        {
            _liveDemoServices = liveDemoServices;
            _logger = logger;
        }

        public async Task<string> ValidateAccountNewTradingGroup(UpdateAccountTradingGroupModel request)
        {
            try
            {
                var usedInstrumentsList = new List<string>();

                var activePositionsInstruments = (await _liveDemoServices.GetContext(request.IsLive).StReader
                    .GetActivePositionsInstruments(request.TraderId, request.AccountId)).ToList();

                if (activePositionsInstruments.Count > 0)
                {
                    usedInstrumentsList.AddRange(activePositionsInstruments);
                }

                var pendingOrdersInstruments = (await _liveDemoServices.GetContext(request.IsLive).StReader
                    .GetPendingOrdersInstruments(request.TraderId, request.AccountId)).ToList();

                if (pendingOrdersInstruments.Count > 0)
                {
                    usedInstrumentsList.AddRange(pendingOrdersInstruments);
                    usedInstrumentsList = usedInstrumentsList.Distinct().ToList();
                }

                var closedPositionsInstruments = (await _liveDemoServices.GetContext(request.IsLive).StReader
                     .GetClosedPositionsInstruments(request.TraderId, request.AccountId)).ToList();

                if (closedPositionsInstruments.Count > 0)
                {
                    usedInstrumentsList.AddRange(closedPositionsInstruments);
                    usedInstrumentsList = usedInstrumentsList.Distinct().ToList();
                }

                if (usedInstrumentsList.Count > 0)
                {
                    var newGroupToAssign = (await _liveDemoServices.GetContext(request.IsLive).TradingGroupsRepository
                        .GetAllAsync()).FirstOrDefault(x => x.Id == request.TradingGroupToAssign);

                    if (newGroupToAssign == null)
                    {
                        return $"New Trading Group [{request.TradingGroupToAssign}] couldn't be found";
                    }

                    var newGroup = TradingGroupModel.Create(newGroupToAssign);

                    var newProfileToAssign = (await _liveDemoServices.GetContext(request.IsLive).TradingProfileRepository
                        .GetAllAsync()).FirstOrDefault(profile => profile.Id == newGroup.TradingProfileId);

                    if (newProfileToAssign == null)
                    {
                        return $"Trading Profile '{newGroup.TradingProfileId}'of New Trading Group [{request.TradingGroupToAssign}] couldn't be found";
                    }

                    var newProfile = TradingProfileModel.Create(newProfileToAssign);

                    var newProfileInstrumentList = newProfile.Instruments.Select(x => x.Id).ToList();
                    var absentInstrumentList = new List<string>();

                    foreach (var usedInst in usedInstrumentsList)
                    {
                        if (!newProfileInstrumentList.Contains(usedInst))
                        {
                            absentInstrumentList.Add(usedInst);
                        }
                    }

                    if (absentInstrumentList.Count > 0)
                    {
                        return $"Trading Profile of new trading group doesn't contain the following insrument(s) used by trader:"
                            + $"{Environment.NewLine} {String.Join(", ", absentInstrumentList.ToArray())}";
                    }
                }

                return String.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.Error(e, e.Message);
                return "Error occured during the validation of account new TradingGroup";
            }
        }
    }
}