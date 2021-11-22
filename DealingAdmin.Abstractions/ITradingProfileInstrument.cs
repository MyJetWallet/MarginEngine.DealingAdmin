namespace DealingAdmin.Abstractions
{
    public interface ITradingProfileInstrument
    {
        string Id { get; }

        double MinOperationVolume { get; }

        double MaxOperationVolume { get; }

        double MaxPositionVolume { get; }

        int OpenPositionMinDelayMs { get; }

        int OpenPositionMaxDelayMs { get; }

        bool TpSlippage { get; }

        bool SlSlippage { get; }

        bool IsTrending { get; }

        bool OpenPositionSlippage { get; }

        int[] Leverages { get; }

        double? StopOutPercent { get; }
    }
}