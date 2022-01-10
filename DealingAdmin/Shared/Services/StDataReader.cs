using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DealingAdmin.Abstractions;
using MyPostgreSQL;
using SimpleTrading.Abstraction.Trading.Positions;
using SimpleTrading.Postgres;
using SimpleTrading.Postgres.Positions;

namespace DealingAdmin.Shared.Services
{
    public class StDataReader : IStDataReader
    {
        private readonly IPostgresConnection _postgresConnection;

        public StDataReader(IPostgresConnection postgresConnection)
        {
            _postgresConnection = postgresConnection;
        }

        public async Task<IEnumerable<ITradeOrder>> GetClosedPositionsAsync(DateTime dateFrom, DateTime dateTo)
        {
            var sql = $"SELECT * FROM {Views.PositionsClosed} WHERE closedate>=@dateFrom AND closedate<@dateTo";
            return await _postgresConnection.GetRecordsAsync<TradeOrderPostgresEntity>(sql, new { dateFrom, dateTo });
        }

        public async Task<IEnumerable<ITradeOrder>> GetClosedPositionsAsync(
            string traderId,
            string accountId,
            DateTime dateFrom,
            DateTime dateTo)
        {
            var sql = $"SELECT * FROM {Views.PositionsClosed} WHERE traderid=@traderId AND accountid=@accountId AND closedate>=@dateFrom AND closedate<@dateTo";
            return await _postgresConnection.GetRecordsAsync<TradeOrderPostgresEntity>(sql, new { traderId, accountId, dateFrom, dateTo });
        }

        public async Task<IEnumerable<ITradeOrder>> GetActivePositionsAsync()
        {
            var sql = $"SELECT * FROM {Views.PositionsActive}";
            return await _postgresConnection.GetRecordsAsync<TradeOrderPostgresEntity>(sql);
        }

        public async Task<IEnumerable<ITradeOrder>> GetPendingOrdersAsync()
        {
            var sql = $"SELECT * FROM {Views.PendingOrders}";
            return await _postgresConnection.GetRecordsAsync<TradeOrderPostgresEntity>(sql);
        }

        public async Task<IEnumerable<ITradeOrder>> GetActivePositionsAsync(string traderId, string accountId)
        {
            var sql = $"SELECT * FROM {Views.PositionsActive} WHERE traderid=@traderId AND accountid=@accountId";
            return await _postgresConnection.GetRecordsAsync<TradeOrderPostgresEntity>(sql, new { traderId, accountId });
        }

        public async Task<IEnumerable<ITradeOrder>> GetPendingOrdersAsync(string traderId, string accountId)
        {
            var sql = $"SELECT * FROM {Views.PendingOrders} WHERE traderid=@traderId AND accountid=@accountId";
            return await _postgresConnection.GetRecordsAsync<TradeOrderPostgresEntity>(sql, new { traderId, accountId });
        }

        #region Instruments used

        public async Task<IEnumerable<string>> GetActivePositionsInstruments()
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PositionsActive}";
            return await _postgresConnection.GetRecordsAsync<string>(sql);
        }

        public async Task<IEnumerable<string>> GetActivePositionsInstrumentsByTradingGroup(string groupId)
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PositionsActive} WHERE accountid IN
                        (SELECT id FROM {Tables.AccountsTableName} WHERE tradinggroup=@groupId)";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { groupId });
        }

        public async Task<IEnumerable<string>> GetActivePositionsInstrumentsByTradingGroups(List<string> tradingGroups)
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PositionsActive} WHERE accountid IN
                        (SELECT id FROM {Tables.AccountsTableName} WHERE tradinggroup = ANY(@groups))";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { groups = tradingGroups.ToArray() });
        }

        public async Task<IEnumerable<string>> GetActivePositionsInstruments(string traderId, string accountId)
        {
            var sql = $"SELECT DISTINCT instrument FROM {Views.PositionsActive} WHERE traderid=@traderId AND accountid=@accountId";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { traderId, accountId });
        }

        public async Task<IEnumerable<string>> GetPendingOrdersInstruments()
        {
            var sql = $@"SELECT DISTINCT instrument FROM  {Views.PendingOrders}";
            return await _postgresConnection.GetRecordsAsync<string>(sql);
        }

        public async Task<IEnumerable<string>> GetPendingOrdersInstrumentsByTradingGroup(string groupId)
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PendingOrders} WHERE accountid IN
                        (SELECT id FROM {Tables.AccountsTableName} WHERE tradinggroup=@groupId)";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { groupId });
        }

        public async Task<IEnumerable<string>> GetPendingOrdersInstrumentsByTradingGroups(List<string> tradingGroups)
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PendingOrders} WHERE accountid IN
                        (SELECT id FROM {Tables.AccountsTableName} WHERE tradinggroup = ANY(@groups))";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { groups = tradingGroups.ToArray() });
        }

        public async Task<IEnumerable<string>> GetPendingOrdersInstruments(string traderId, string accountId)
        {
            var sql = $"SELECT DISTINCT instrument FROM {Views.PendingOrders} WHERE traderid=@traderId AND accountid=@accountId";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { traderId, accountId });
        }

        public async Task<IEnumerable<string>> GetClosedPositionsInstruments()
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PositionsClosed}";
            return await _postgresConnection.GetRecordsAsync<string>(sql);
        }

        public async Task<IEnumerable<string>> GetClosedPositionsInstrumentsByTradingGroup(string groupId)
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PositionsClosed} WHERE accountid IN
                        (SELECT id FROM {Tables.AccountsTableName} WHERE tradinggroup=@groupId)";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { groupId });
        }

        public async Task<IEnumerable<string>> GetClosedPositionsInstrumentsByTradingGroups(List<string> tradingGroups)
        {
            var sql = $@"SELECT DISTINCT instrument FROM {Views.PositionsClosed} WHERE accountid IN
                        (SELECT id FROM {Tables.AccountsTableName} WHERE tradinggroup = ANY(@groups))";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { groups = tradingGroups.ToArray() });
        }

        public async Task<IEnumerable<string>> GetClosedPositionsInstruments(string traderId, string accountId)
        {
            var sql = $"SELECT DISTINCT instrument FROM {Views.PositionsClosed} WHERE traderid=@traderId AND accountid=@accountId";
            return await _postgresConnection.GetRecordsAsync<string>(sql, new { traderId, accountId });
        }

        #endregion
    }
}