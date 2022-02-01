using DealingAdmin.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ITraderSearchService
    {
        Task<List<TraderBrandSearchModel>> GetTraderDataByAnyId(string phrase);

        Task<List<TraderBrandModel>> GetTraderDataByEmail(string email);
    }
}