using DealingAdmin.Abstractions.Models;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ITradingProfileNewInstrumentsValidator
    {
        Task<string> ValidateTradingProfileNewInstruments(TradingProfileModel request, bool isLive);
    }
}