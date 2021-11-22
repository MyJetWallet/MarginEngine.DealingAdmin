using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ITicker
    {
        public string Id { get; }
        public int Digits { get; }
        public double TickSize { get; }
        public IList<ITickerSourceSettings> SourceSettings { get; }
    }
    
    public class InstrumentTicker : ITicker
    {
        public string Id { get; set; }
        public int Digits { get; set; }
        public double TickSize { get; set; }
        public IList<ITickerSourceSettings> SourceSettings { get; set; }

        public static InstrumentTicker Create(ITicker src)
        {
            return new InstrumentTicker
            {
                Id = src.Id,
                Digits = src.Digits,
                TickSize = src.TickSize,
                SourceSettings = src.SourceSettings
            };
        }
    }

    public interface ITickerSourceSettings
    {
        public string ExchangeTicker { get; set; }
        public string LiquidityProvider { get; set; }
        public int Weight { get; set; }
    }
    
    public class InstrumentTickerSourceSettings : ITickerSourceSettings
    {
        public string ExchangeTicker { get; set; }
        public string LiquidityProvider { get; set; }
        public int Weight { get; set; }

        public static InstrumentTickerSourceSettings Create(ITickerSourceSettings src)
        {
            return new InstrumentTickerSourceSettings
            {
                ExchangeTicker = src.ExchangeTicker,
                LiquidityProvider = src.LiquidityProvider,
                Weight = src.Weight
            };
        }
    }
    

    public interface ITickerReader
    {
        IEnumerable<ITicker> GetAll();
        ITicker GetById(string ticker);
    }

    public interface ITickerRepository
    {
        Task InsertOrReplaceAsync(ITicker ticker);
    }
    
    public class InstrumentTickerReader: ITickerReader
    {
        public IEnumerable<ITicker> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public ITicker GetById(string ticker)
        {
            throw new System.NotImplementedException();
        }
    }
}