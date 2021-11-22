using System.Collections.Generic;

namespace DealingAdmin.Abstractions
{
    public interface ITradingProfile
    {
        string Id { get; }

        double MarginCallPercent { get; }

        double StopOutPercent { get; }

        double PositionToppingUpPercent { get; }

        IEnumerable<ITradingProfileInstrument> Instruments { get; }
    }
}