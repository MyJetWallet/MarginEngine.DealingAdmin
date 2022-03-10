using DealingAdmin.Abstractions.Models;
using SimpleTrading.Abstraction.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ICrmDataReader
    {
        Task<IEnumerable<IInternalAccount>> GetAccountsType();

        Task<InternalTraderModel> GetAccountType(string traderId);

        Task<string> GetTraderIdBySearch(string phrase);

        Task<string> GetTraderIdByAccountId(string accountId);
    }
}