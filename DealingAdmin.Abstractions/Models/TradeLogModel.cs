using System;
using System.Text;
using SimpleTrading.Abstraction.Trading.TradingLog;

namespace DealingAdmin.Abstractions.Models
{
    public class TradeLogModel
    {
        public object Data { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public string ProcessId { get; set; }
        
        public string Component { get; set; }
        
        public string Message { get; set; }
        
        public string ObjectId { get; set; }

        private static string EncodeData(string jsonData)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(jsonData);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return jsonData;
            }
        }
        
        public static TradeLogModel Create(ITradeLog src)
        {
            return new TradeLogModel
            {
                DateTime = src.DateTime,
                ProcessId = src.ProcessId,
                Component = src.Component,
                Message = src.Message,
                Data =  EncodeData(src.JsonData),
                ObjectId = src.ObjectId ?? string.Empty
            };
        }
    }
}