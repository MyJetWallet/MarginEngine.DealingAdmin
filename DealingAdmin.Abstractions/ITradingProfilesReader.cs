using System.Collections.Generic;

namespace DealingAdmin.Abstractions
{
    public interface ITradingProfilesReader
    {
        IEnumerable<ITradingProfile> GetAll();
        ITradingProfile Get(string id);
    }
}