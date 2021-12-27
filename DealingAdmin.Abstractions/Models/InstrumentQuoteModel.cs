namespace DealingAdmin.Abstractions.Models
{
    public class InstrumentQuoteModel
    { 
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public int Digits { get; set; }
        
        public string? GroupId { get; set; }

        public static InstrumentQuoteModel Create(
            SimpleTrading.Abstraction.Trading.Instruments.ITradingInstrument src)
        {
            return new InstrumentQuoteModel
            {
                Id = src.Id,
                Name = src.Name,
                Digits = src.Digits,
                GroupId = src.GroupId,
            };
        }
    }



}