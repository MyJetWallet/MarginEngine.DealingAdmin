using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using MyPostgreSQL;
using SimpleTrading.Abstraction.Accounts;

namespace DealingAdmin.Shared.Services
{
    public class CrmDataReader : ICrmDataReader
    {
        private static class Tables
        {
            public const string Accounts = "mt5_accounts";
            public const string StInternalAccountsView = "st_internal_accounts_view";
            public const string StInternalTraderAccountsView = "st_internal_trader_accounts_view";
        }

        private readonly IPostgresConnection _crmConnection;

        private class TraderIdModel
        {
            public string TraderId { get; set; }
        }

        public CrmDataReader(IPostgresConnection crmConnection)
        {
            _crmConnection = crmConnection ?? throw new ArgumentNullException(nameof(crmConnection));
        }

        public async Task<IEnumerable<IInternalAccount>> GetAccountsType()
        {
            var sql = @$"SELECT * FROM {Tables.StInternalAccountsView}";
            var result = await _crmConnection.GetRecordsAsync<InternalAccountCrmModel>(sql);
            return result.Select(acc => acc.ToInternalAccountModel());
        }

        public async Task<InternalTraderModel> GetAccountType(string traderId)
        {
            if (String.IsNullOrEmpty(traderId))
            {
                return null;
            }

            var sql = @$"SELECT id AS TraderId, COALESCE(isinternal, true) AS isinternal FROM personaldata WHERE id = @traderId";
            var result = await _crmConnection.GetFirstRecordOrNullAsync<InternalTraderModel>(sql, new { traderId = traderId });
            return result;
        }

        public async Task<string> GetTraderIdBySearch(string phrase)
        {
            var traderIdSearch = await GetTraderIdByAccountId(phrase);

            if (!String.IsNullOrEmpty(traderIdSearch))
            {
                return traderIdSearch;
            }

            var sqlSearchByTraderId = @$"SELECT traderid FROM {Tables.Accounts} WHERE traderid = @traderId";
            var traderResult = await _crmConnection.
                GetFirstRecordOrNullAsync<TraderIdModel>(sqlSearchByTraderId, new { traderId = phrase });

            if (traderResult != null && !String.IsNullOrEmpty(traderResult.TraderId))
            {
                return traderResult.TraderId;
            }

            return null;
        }

        public async Task<string> GetTraderIdByAccountId(string accountId)
        {
            var sqlSearchByAccountId = @$"SELECT traderid FROM {Tables.Accounts} WHERE accountid = @accountId";
            var accountResult = await _crmConnection.
                GetFirstRecordOrNullAsync<TraderIdModel>(sqlSearchByAccountId, new { accountId = accountId });

            if (accountResult != null && !String.IsNullOrEmpty(accountResult.TraderId))
            {
                return accountResult.TraderId;
            }

            return null;
        }
    }

    public static class CrmDataReaderHelpers
    {
        public static bool IsEmail(this string str)
        {
            return str.Contains("@");
        }

        public static bool IsAccountId(this string str)
        {
            return str.Length == 15;
        }
    }
}