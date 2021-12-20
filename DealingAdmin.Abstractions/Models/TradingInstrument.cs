namespace DealingAdmin.Abstractions.Models
{
    public class TradingInstrument
    { 
        public string Id { get; set; }
        public string Name { get; set; }
        public int Digits { get; set; }
        public string Base { get; set; }
        public string Quote { get; set; }
        public double TickSize { get; set; }
        public string SwapScheduleId { get; set; } 
        public string? GroupId { get; set; }
        public string? SubGroupId { get; set; }
        public int? Weight { get; set; }  
        public int? DayTimeout { get; set; }
        public int? NightTimeout { get; set; }
        public bool TradingDisabled { get; set; } 
    }
}