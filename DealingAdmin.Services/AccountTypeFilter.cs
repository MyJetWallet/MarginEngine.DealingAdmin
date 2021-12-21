using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using SimpleTrading.Abstraction.Trading.Positions;

namespace DealingAdmin.Services
{
    public class AccountTypeFilter : IAccountTypeFilter
    {
        private readonly ICrmDataReader _crmReader;

        public AccountTypeFilter(ICrmDataReader crmReader)
        {
            _crmReader = crmReader;
        }

        public async Task<IEnumerable<ITradeOrder>> FilterPositions(IEnumerable<ITradeOrder> positions, AccountType accType)
        {
            var internalAccounts = (await _crmReader.GetAccountsType()).ToList();

            var filteredActivePositions = accType switch
            {
                AccountType.Real => positions.Where(pos =>
                    internalAccounts.Exists(acc => !acc.IsInternal && acc.Id == pos.AccountId)).ToList(),
                AccountType.Internal => positions.Where(pos =>
                    internalAccounts.Exists(acc => acc.IsInternal && acc.Id == pos.AccountId)
                    || !internalAccounts.Exists(acc => acc.Id == pos.AccountId)).ToList(),
                _ => positions
            };

            return filteredActivePositions;
        }
    }
}