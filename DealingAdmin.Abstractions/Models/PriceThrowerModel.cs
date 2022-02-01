
using System;
using SimpleTrading.Abstraction.BidAsk;

namespace DealingAdmin.Abstractions.Models
{
    public class PriceThrowerModel : IBidAsk
    {
        public string Id { get; set; }
        
        DateTime IBidAsk.DateTime => DateTime.UtcNow;
        
        public double Bid { get; set; }
        
        public double Ask { get; set; }
        
        public string ApiKey { get; set; }
    }    
}