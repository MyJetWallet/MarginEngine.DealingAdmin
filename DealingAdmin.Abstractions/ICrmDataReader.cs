﻿using SimpleTrading.Abstraction.Accounts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ICrmDataReader
    {
        Task<IEnumerable<IInternalAccount>> GetAccountsType();
        Task<string> GetTraderIdBySearch(string phrase);
    }
}