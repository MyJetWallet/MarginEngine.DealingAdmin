namespace DealingAdmin.Abstractions.Models
{
    public class UpdateAccountTradingGroupModel
    {
        public string TraderId { get; set; }
        
        public string AccountId { get; set; }
        
        public string TradingGroupToAssign { get; set; }
        
        public bool IsLive { get; set; }

        public string ApiKey { get; set; }
    }
}