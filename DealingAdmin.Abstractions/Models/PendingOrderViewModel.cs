using SimpleTrading.Abstraction.Trading.Positions;
using System;

namespace DealingAdmin.Abstractions.Models
{
    public class PendingOrderViewModel
    {
        public long Id { get; set; }
        
        public string TraderId { get; set; }

        public string AccountId { get; set; }

        public string Instrument { get; set; }

        public int InstrumentDigits { get; set; }

        public double InvestmentAmount { get; set; }
        
        public int Leverage { get; set; }
        
        public DateTime Created { get; set; }
        
        public PositionOperation Operation { get; set; }
        
        public double? TakeProfitInCurrency { get; set; }
        
        public double? StopLossInCurrency { get; set; }
        
        public double? TakeProfitRate { get; set; }
        
        public double? StopLossRate { get; set; }
        
        public double DesiredPrice { get; set; }
        
        public PositionOrderType OpenOrderType { get; set; }

        public double ReservedFundsForToppingUp { get; set; }

        public double ToppingUpPercent { get; set; }

        public static PendingOrderViewModel Create(ITradeOrder src, int instrumentDigits)
        {
            return new PendingOrderViewModel
            {
                Id = src.Id,
                TraderId = src.TraderId,
                AccountId = src.AccountId,
                Instrument = src.Instrument,
                Leverage = src.Leverage,
                Operation = src.Operation,
                InvestmentAmount = src.InvestmentAmount,
                InstrumentDigits = instrumentDigits,
                DesiredPrice = src.DesiredPrice,
                Created = src.Created,
                TakeProfitInCurrency = src.TakeProfitInCurrency,
                StopLossInCurrency = src.StopLossInCurrency,
                TakeProfitRate = src.TakeProfitRate,
                StopLossRate = src.StopLossRate,
                OpenOrderType = src.PositionOrderType,
                ReservedFundsForToppingUp = src.ReservedFundsForToppingUp,
                ToppingUpPercent = src.ToppingUpPercent
            };
        }
        
    }
}