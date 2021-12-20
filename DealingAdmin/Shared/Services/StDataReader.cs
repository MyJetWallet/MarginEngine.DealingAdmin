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
    }
}