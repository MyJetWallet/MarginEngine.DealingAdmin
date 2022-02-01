using SimpleTrading.Auth.Grpc.Contracts;

namespace DealingAdmin.Abstractions.Models
{
    public class TraderBrandModel
    {
        public string TraderId { get; set; }

        public string Brand { get; set; }

        public static TraderBrandModel FromGrpc(TraderBrandGrpcModel src)
        {
            return new TraderBrandModel
            {
                TraderId = src.TraderId,
                Brand = src.Brand
            };
        }
    }
}