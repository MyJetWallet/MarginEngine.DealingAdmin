using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using SimpleTrading.Auth.Grpc;
using SimpleTrading.Auth.Grpc.Contracts;
using SimpleTrading.PersonalData.Grpc;

namespace DealingAdmin.Services
{
    public class TraderSearchService : ITraderSearchService
    {
        private readonly IAuthGrpcService _authGrpcService;

        private readonly ICrmDataReader _crmDataReader;

        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;

        public TraderSearchService(
            IAuthGrpcService authGrpcService,
            ICrmDataReader crmDataReader,
            IPersonalDataServiceGrpc personalDataServiceGrpc)
        {
            _authGrpcService = authGrpcService;
            _crmDataReader = crmDataReader;
            _personalDataServiceGrpc = personalDataServiceGrpc;
        }

        public async Task<List<TraderBrandModel>> GetTraderDataByEmail(string email)
        {
            var traders = new List<TraderBrandModel>();

            var emailResponse = await _authGrpcService.GetIdsByEmailAsync(
                new GetIdsByEmailGrpcRequest { Email = email });

            if (emailResponse?.TraderBrands?.Count() > 0)
            {
                traders.AddRange(emailResponse.TraderBrands.Select(x => TraderBrandModel.FromGrpc(x)));
            }

            return traders;
        }

        public async Task<List<TraderBrandSearchModel>> GetTraderDataByAnyId(string phrase)
        {
            var traders = new List<TraderBrandSearchModel>();

            var traderIdSearchResponse = (await _crmDataReader.GetTraderIdBySearch(phrase));

            if (!string.IsNullOrEmpty(traderIdSearchResponse))
            {
                var pdResponse = await _personalDataServiceGrpc.GetByIdAsync(traderIdSearchResponse);
                var email = pdResponse.PersonalData.Email;
                var emailResponse = await GetTraderDataByEmail(email);

                foreach (var item in emailResponse)
                {
                    traders.Add(TraderBrandSearchModel.FromTraderBrand(
                        item,
                        traderIdSearchResponse == item.TraderId));
                }
            }

            return traders;
        }
    }
}