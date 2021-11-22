using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface IRouterRestClient
    {
        Task<IEnumerable<string>> GetActiveLpsList();
    }
    
    public class RouterRestClient : IRouterRestClient
    {
        public async Task<IEnumerable<string>> GetActiveLpsList()
        {
            return new List<string>
            {
                "ftx",
                "binance",
                "kraken"
            }; 
        }
    }
}