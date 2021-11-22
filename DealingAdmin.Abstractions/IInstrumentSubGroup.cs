namespace DealingAdmin.Abstractions
{
    public interface IInstrumentSubGroup
    {
        string Id { get; }

        string Name { get; }

        string GroupId { get; }

        int Weight { get; }
    }
}