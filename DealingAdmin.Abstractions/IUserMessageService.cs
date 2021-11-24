using System;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IUserMessageService
    {
        Task SuccessAsync(string text);
        
        Task WarningAsync(string text);
        
        Task ErrorAsync(string text, Exception ex = null);
    }
}