using SimpleTrading.Abstraction.Trading.Positions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IStDataReader
    {
        Task<IEnumerable<ITradeOrder>> GetActivePositionsAsync();
        Task<IEnumerable<ITradeOrder>> GetActivePositionsAsync(string traderId, string accountId);
        Task<IEnumerable<string>> GetActivePositionsInstruments();
        Task<IEnumerable<string>> GetActivePositionsInstruments(string traderId, string accountId);
        Task<IEnumerable<string>> GetActivePositionsInstrumentsByTradingGroup(string groupId);
        Task<IEnumerable<string>> GetActivePositionsInstrumentsByTradingGroups(List<string> tradingGroups);
        Task<IEnumerable<ITradeOrder>> GetClosedPositionsAsync(DateTime dateFrom, DateTime dateTo);
        Task<IEnumerable<ITradeOrder>> GetClosedPositionsAsync(string traderId, string accountId, DateTime dateFrom, DateTime dateTo);
        Task<IEnumerable<string>> GetClosedPositionsInstruments();
        Task<IEnumerable<string>> GetClosedPositionsInstruments(string traderId, string accountId);
        Task<IEnumerable<string>> GetClosedPositionsInstrumentsByTradingGroup(string groupId);
        Task<IEnumerable<string>> GetClosedPositionsInstrumentsByTradingGroups(List<string> tradingGroups);
        Task<IEnumerable<ITradeOrder>> GetPendingOrdersAsync();
        Task<IEnumerable<ITradeOrder>> GetPendingOrdersAsync(string traderId, string accountId);
        Task<IEnumerable<string>> GetPendingOrdersInstruments();
        Task<IEnumerable<string>> GetPendingOrdersInstruments(string traderId, string accountId);
        Task<IEnumerable<string>> GetPendingOrdersInstrumentsByTradingGroup(string groupId);
        Task<IEnumerable<string>> GetPendingOrdersInstrumentsByTradingGroups(List<string> tradingGroups);
    }
}