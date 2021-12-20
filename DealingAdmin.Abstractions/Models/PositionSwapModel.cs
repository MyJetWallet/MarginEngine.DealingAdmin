using SimpleTrading.Abstraction.Trading.Positions;
using System;

namespace DealingAdmin.Abstractions.Models
{
    public class PositionSwapModel 
    {
        public DateTime SwapDt { get; set; }
        
        public DateTime ExecutedDt { get; set; }
        
        public DateTime? QuoteDt { get; set; }
        
        public double Long { get; set; }
        
        public double Short { get; set; }
        
        public int Amount { get; set; }
        
        public double Profit { get; set; }

        public static PositionSwapModel Create(IPositionSwap src)
        {
            return new PositionSwapModel
            {
                Amount = src.Amount,
                Long = src.Long,
                Profit = src.Profit,
                Short = src.Short,
                ExecutedDt = src.ExecutedDt,
                QuoteDt = src.QuoteDt,
                SwapDt = src.SwapDt,
            };
        }
    }
}