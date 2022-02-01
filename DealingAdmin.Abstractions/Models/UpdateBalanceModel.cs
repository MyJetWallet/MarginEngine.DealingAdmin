using System.ComponentModel.DataAnnotations;
using SimpleTrading.Abstraction.Trading.BalanceOperations;

namespace DealingAdmin.Abstractions.Models
{
    public class UpdateBalanceModel
    {
        [Required]
        public string TraderId { get; set; }
        
        [Required]
        public string AccountId { get; set; }
        
        [Required]
        public double Delta { get; set; }
        
        [Required]
        public string Comment { get; set; }
        
        [Required]
        public BalanceUpdateOperationType OperationType { get; set; }
        
        [Required]
        public string ProcessId { get; set; }
        
        [Required]
        public bool IsLive { get; set; }
        
        [Required]
        public string ChangeBalanceApiKey { get; set; }
    }
}