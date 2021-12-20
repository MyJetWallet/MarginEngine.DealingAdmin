using System;
using System.Linq;
using SimpleTrading.Abstraction.Trading.Positions;

namespace DealingAdmin.Abstractions.Models
{
    public class ActivePositionEntity
    {
        public long Id { get; set; }

        public string AccountId { get; set; }

        public string TraderId { get; set; }

        public DateTime? CloseDate { get; set; }

        public ClosePositionReason CloseReason { get; set; }

        public DateTime OpenDate { get; set; }

        public string Instrument { get; set; }

        public double Profit { get; set; }

        public double OpenPrice { get; set; }

        public double DesiredPrice { get; set; }

        public double ClosePrice { get; set; }

        public double Volume { get; set; }

        public double OpenSwaps { get; set; }

        public int PositionOrderType { get; set; }
        
        public int Operation { get; set; }
        
        public int Leverage { get; set; }
        
        public double InvestmentAmount { get; set; }

        public double? TakeProfitRate { get; set; }

        public double? StopLossRate { get; set; }

        public double? TakeProfitInCurrency { get; set; }

        public double? StopLossInCurrency { get; set; }

        public DateTime? Created { get; set; }

        public double? ToppingUpPercent { get; set; }

        public double? RefillSum { get; set; }

        public double? BurnBonus { get; set; }

        public static ActivePositionEntity Create(ITradeOrder src)
        {
            return new ActivePositionEntity
            {
                Id = src.Id,
                AccountId = src.AccountId,
                TraderId = src.TraderId,
                CloseDate = src.CloseDate,
                CloseReason = src.CloseReason,
                OpenDate = src.OpenDate,
                Instrument = src.Instrument,
                Profit = src.Profit,
                OpenPrice = src.OpenPrice,
                DesiredPrice = src.DesiredPrice,
                ClosePrice = src.ClosePrice,
                Volume = src.Volume,
                OpenSwaps = src.Swaps?.Sum(x => x.Profit) ?? 0,
                PositionOrderType = (int)src.PositionOrderType,
                Operation = (int)src.Operation,
                Leverage = src.Leverage,
                InvestmentAmount = src.InvestmentAmount,
                TakeProfitRate = src.TakeProfitRate,
                StopLossRate = src.StopLossRate,
                TakeProfitInCurrency = src.TakeProfitInCurrency,
                StopLossInCurrency = src.StopLossInCurrency,
                Created = src.Created,
                ToppingUpPercent = src.ToppingUpPercent,
                RefillSum = src.ReservedFundsForToppingUp,
                BurnBonus = src.BurnBonus
            };
        }
    }
}
           