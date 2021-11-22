namespace DealingAdmin.Abstractions
{
    public interface IInstrumentGroup
    {
        string Id { get; }

        string Name { get; }

        int Weight { get; }
    }
}