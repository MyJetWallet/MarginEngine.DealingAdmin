using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ICrossTickerModel
    {
        string Id { get; }
        string BaseTickerId { get; }
        string QuoteTickerId { get; }
    }
    
    public class CrossTickerModel : ICrossTickerModel
    {
        public string Id { get; set; }
        public string BaseTickerId { get; set; }
        public string QuoteTickerId { get; set; }

        public static CrossTickerModel Create(ICrossTickerModel src)
        {
            return new CrossTickerModel
            {
                Id = src.Id,
                BaseTickerId = src.BaseTickerId,
                QuoteTickerId = src.QuoteTickerId
            };
        }
    }

    public interface ICrossTickerReader
    {
        IEnumerable<ICrossTickerModel> GetAll();
    }

    public interface ICrossTickerRepository
    {
        Task InsertOrReplaceAsync(ICrossTickerModel src);
    }
}