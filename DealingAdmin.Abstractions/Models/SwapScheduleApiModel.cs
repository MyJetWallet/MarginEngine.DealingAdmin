using System;
using System.ComponentModel.DataAnnotations;
using SimpleTrading.Abstraction.Trading.Swaps;

namespace DealingAdmin.Abstractions.Models
{
    public class SwapScheduleModel : ISwapSchedule
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required]
        public string Time { get; set; }
        TimeSpan ISwapSchedule.Time => TimeSpan.Parse(Time);
        
        [Required]
        public int Amount { get; set; }


        public static SwapScheduleModel Create(ISwapSchedule src)
        {
            return new SwapScheduleModel
            {
                Id = src.Id,
                Amount = src.Amount,
                Time = src.Time.ToString("c"),
                DayOfWeek = src.DayOfWeek
            };
        } 
    }
}