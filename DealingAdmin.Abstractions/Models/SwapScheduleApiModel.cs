using System;
using SimpleTrading.Abstraction.Trading.Swaps;

namespace DealingAdmin.Abstractions.Models
{
    public class SwapScheduleModel : ISwapSchedule
    {
        public string Id { get; set; }
        
        public DayOfWeek DayOfWeek { get; set; }
        
        public TimeSpan Time { get; set; }
        
        public int Amount { get; set; }

        public static SwapScheduleModel Create(ISwapSchedule src)
        {
            return new SwapScheduleModel
            {
                Id = src.Id,
                Amount = src.Amount,
                Time = src.Time,
                DayOfWeek = src.DayOfWeek
            };
        } 
    }
}