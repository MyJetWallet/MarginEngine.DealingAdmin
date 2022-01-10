using DealingAdmin.Abstractions.Models;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IAccountNewTradingGroupValidator
    {
        Task<string> ValidateAccountNewTradingGroup(UpdateAccountTradingGroupModel request);
    }
}