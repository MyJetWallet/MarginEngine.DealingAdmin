using SimpleTrading.Abstraction.BidAsk;
using SimpleTrading.Abstraction.Trading.Positions;
using System;
using System.Collections.Generic;

namespace DealingAdmin.Abstractions.Models
{
    public class EventCreatePositionModel
    {
        public EventPositionModel NewPosition;
    }
    public class EventPositionModel : ITradeOrder
    {
        public long Id  { get; set; }
        public double ClosePrice  { get; set; }
        public ClosePositionReason CloseReason  { get; set; }
        public DateTime CloseDate  { get; set; }
        public double Profit  { get; set; }
        public double ReservedFundsForToppingUp  { get; set; }
        public double ToppingUpPercent  { get; set; }
        public IEnumerable<IPositionSwap> Swaps  { get; set; }
        public IEnumerable<IPositionCommission> Commissions  { get; set; }
        public double Volume  { get; set; }
        public DateTime OpenDate  { get; set; }
        public string ProcessId  { get; set; }
        public DateTime TimeStamp  { get; set; }
        public double? StopLossRate  { get; set; }
        public double? TakeProfitRate  { get; set; }
        public double? StopLossInCurrency  { get; set; }
        public double? TakeProfitInCurrency  { get; set; }
        public PositionOrderType PositionOrderType  { get; set; }
        public PositionOperation Operation  { get; set; }
        public BidAsk OpenBidAsk  { get; set; }
        IBidAsk ITradeOrder.OpenBidAsk => OpenBidAsk;
        public double OpenPrice  { get; set; }
        public double DesiredPrice  { get; set; }
        public DateTime Created  { get; set; }
        public int Leverage  { get; set; }
        public double InvestmentAmount  { get; set; }
        public string Instrument  { get; set; }
        public string TraderId  { get; set; }
        public string AccountId  { get; set; }
        public BidAsk CloseBidAsk  { get; set; }
        IBidAsk ITradeOrder.CloseBidAsk => CloseBidAsk;
        public double BurnBonus  { get; set; }
    }
}