using SimpleTrading.Abstraction.Candles;
using System;

namespace DeakingAdmin.Abstractions.Models
{
    public class CandleEditContract : ICandleModel
    {
        public string InstrumentId { get; set; }

        public CandleType Type { get; set; }

        public DateTime DateTime { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double High { get; set; }

        public double Low { get; set; }
    }
}