using System;
using System.Collections.Generic;
using System.Linq;
using SimpleTrading.Abstraction.Trading.Positions;

namespace DealingAdmin.Abstractions.Models
{
    public class ActivePositionModel
    {
        public long Id { get; set; }
        
        public string TraderId { get; set; }
        
        public string AccountId { get; set; }
        
        public string Instrument { get; set; }
        
        public double InvestmentAmount { get; set; }
        
        public int Leverage { get; set; }
        
        public PositionOperation Operation { get; set; }
        
        public double? TakeProfitInCurrency { get; set; }
        
        public double? StopLossInCurrency { get; set; }
        
        public double? TakeProfitRate { get; set; }
        
        public double? StopLossRate { get; set; }
        
        public DateTime TimeStamp { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime OpenDate { get; set; }
        
        public double OpenPrice { get; set; }
        
        public double DesiredPrice { get; set; }
        
        public BidAskModel OpenPriceBidAsk { get; set; }
        
        public PositionOrderType OpenOrderType { get; set; }
        
        public IEnumerable<PositionSwapModel> Swaps { get; set; }

        public double ReservedFundsForToppingUp { get; set; }

        public double ToppingUpPercent { get; set; }

        public int? RefillsIn { get; set; }

        public int? RefillsOut { get; set; }

        public static ActivePositionModel Create(ITradeOrder src)
        {
            return new ActivePositionModel
            {
                Id = src.Id,
                TraderId = src.TraderId,
                AccountId = src.AccountId,
                Instrument = src.Instrument,
                Leverage = src.Leverage,
                Operation = src.Operation,
                InvestmentAmount = src.InvestmentAmount,
                Created = src.Created,
                OpenDate = src.OpenDate,
                OpenPrice = src.OpenPrice,
                TimeStamp = src.TimeStamp,
                DesiredPrice = src.DesiredPrice,
                OpenOrderType = src.PositionOrderType,
                TakeProfitInCurrency = src.TakeProfitInCurrency,
                StopLossInCurrency = src.StopLossInCurrency,
                TakeProfitRate = src.TakeProfitRate,
                StopLossRate = src.StopLossRate,
                Swaps = src.Swaps.Select(PositionSwapModel.Create),
                OpenPriceBidAsk = src.OpenBidAsk.ToBidAskModel(),
                ReservedFundsForToppingUp = src.ReservedFundsForToppingUp,
                ToppingUpPercent = src.ToppingUpPercent
            };
        }
    }
}