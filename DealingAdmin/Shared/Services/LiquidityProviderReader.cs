using System.Collections.Generic;
using System.Threading.Tasks;
using DealingAdmin.Abstractions;
using Flurl;
using Flurl.Http;

namespace DealingAdmin.Shared.Services
{
    public class LiquidityProviderReader : ILiquidityProviderReader
    {
        private readonly string _feedRouterUrl;

        public LiquidityProviderReader(string quoteFeedRouterUrl)
        {
            _feedRouterUrl = quoteFeedRouterUrl;
        }

        public async Task<IEnumerable<string>> GetLiquidityProviders()
        {
            return await _feedRouterUrl
                .AppendPathSegments("api", "LiquidityProviders", "All")
                .GetJsonAsync<List<string>>();
        }
    }
}