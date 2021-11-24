using DeakingAdmin.Abstractions.Models;
using Microsoft.AspNetCore.Components.Forms;
using SimpleTrading.Abstraction.Candles;
using SimpleTrading.CandlesHistory.AzureStorage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeakingAdmin.Abstractions
{
    public interface ICandlesService
    {
        Task BulkWriteCandles(string instrumentId, CandleType candleType, List<CandleModel> candles);
        
        Task<CandleModel> GetCacheCandleValue(string instrumentId, CandleType candleType, DateTime dateTime);
        
        Task<CandleModel> GetDbCandleValue(string instrumentId, CandleType candleType, DateTime dateTime);
        
        Task<List<CandleModel>> ParseCandlesByType(CandleType candleType, IBrowserFile csvFile);
        
        Task SaveCandle(CandleEditContract editContract);
        
        Task<CandlesVerificationResult> VerifyCacheCandles(string instrumentId, List<CandleModel> candles, CandleType candleType, bool stopWithFirstDiff);
        
        Task<CandlesVerificationResult> VerifyDbCandles(string instrumentId, List<CandleModel> candles, CandleType candleType, bool stopWithFirstDiff);
        
        Task WriteCandles(string instrumentId, CandleType candleType, List<CandleModel> candles);
    }
}