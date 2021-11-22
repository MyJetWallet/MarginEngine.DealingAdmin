namespace DealingAdmin.Abstractions
{
    public interface ISwapProfile
    {
        string Id { get; }

        string InstrumentId { get; }

        double Long { get; }

        double Short { get; }
    }
}