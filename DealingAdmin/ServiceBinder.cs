using System;
using System.Collections.Generic;
using DealingAdmin.Abstractions;
using DealingAdmin.Services;
using DealingAdmin.Shared.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using MyNoSqlServer.DataReader;
using MyPostgreSQL;
using MyServiceBus.TcpClient;
using ProtoBuf.Grpc.Client;
using Serilog;
using SimpleTrading.Abstraction.BidAsk;
using SimpleTrading.Abstraction.Caches.ActiveOrders;
using SimpleTrading.Abstraction.Candles;
using SimpleTrading.Abstraction.Trading.Settings;
using SimpleTrading.CandlesHistory.AzureStorage;
using SimpleTrading.CandlesHistory.Grpc;
using SimpleTrading.MyNoSqlRepositories;
using SimpleTrading.ServiceBus.PublisherSubscriber.BidAsk;
using SimpleTrading.ServiceBus.PublisherSubscriber.UnfilteredBidAsk;
using SimpleTrading.Common.MyNoSql;
using SimpleTrading.QuotesFeedRouter.Abstractions;
using SimpleTrading.Auth.Grpc;
using SimpleTrading.Engine.Grpc;
using SimpleTrading.Abstraction.Trading;
using SimpleTrading.PersonalData.Grpc;
using DealingAdmin.Validators;
using SimpleTrading.TradeLog.Grpc;
using SimpleTrading.Abstraction.Trading.Instruments;
using SimpleTrading.Abstraction.Trading.InstrumentsGroup;
using SimpleTrading.Abstractions.Common.InstrumentsAvatar;
using SimpleTrading.Abstraction.Trading.Profiles;
using SimpleTrading.Abstraction.Trading.Swaps;
using SimpleTrading.Abstraction.Markups;
using SimpleTrading.Common.Abstractions.DefaultValues;
using SimpleTrading.ClientApi.Services;
using SimpleTrading.Abstraction.Markups.TradingGroupMarkupProfiles;
using SimpleTrading.TickHistory.Grpc;
using SimpleTrading.Admin.Grpc;

namespace DealingAdmin
{
    public class LiveDemoServices
    {
        public ITradingProfileRepository TradingProfileRepository { get; set; }

        public IStDataReader StReader { get; set; }

        public IActiveOrdersCacheReader ActiveOrdersReader { get; set; }

        public ISimpleTradingEngineApi EngineApi { get; set; }

        public ITradingGroupsRepository TradingGroupsRepository { get; set; }

        public ITradingGroupsMarkupProfilesRepository TradingGroupsMarkupProfilesRepository { get; set; }

        public ITradingGroupMarkupProfilePropertiesRepository TradingGroupMarkupProfilePropertiesRepository { get; set; }
    }

    public class LiveDemoServiceMapper
    {
        private readonly Dictionary<bool, LiveDemoServices> _services = new();

        public void InitService(bool isLive, Action<LiveDemoServices> func)
        {
            if (!_services.ContainsKey(isLive))
                _services.Add(isLive, new LiveDemoServices());

            func(_services[isLive]);
        }

        public LiveDemoServices GetContext(bool isLive)
        {
            return _services[isLive];
        }
    }

    public class AdminAppSettings
    {
        public string ChangeBalanceApiKey { get; set; }
        
        public string AdminCrudApiKey { get; set; }
    }

    public static class ServiceBinder
    {
        private const string AppName = "DealingAdmin";
        private const string EnvInfo = "ENV_INFO";
        private static string AppNameWithEnvMark => $"{AppName}-{GetEnvInfo()}";

        public static void BindServices(this IServiceCollection services, SettingsModel settingsModel)
        {
            services.AddScoped<IUserMessageService, UserMessageService>();

            services.AddScoped<IFileService, FileService>();

            services.AddSingleton(new CandlesServiceSettings()
            {
                CandlesSaveChunkSize = settingsModel.CandlesSaveChunkSize,
                CandlesExpiresMinutes = settingsModel.CandlesExpiresMinutes,
                CandlesExpiresHours = settingsModel.CandlesExpiresHours
            });

            services.AddSingleton(new AdminAppSettings()
            {
                ChangeBalanceApiKey = settingsModel.ChangeBalanceApiKey,
                AdminCrudApiKey = settingsModel.AdminCrudApiKey,
            });

            services.AddScoped<StateManager>();
            services.AddScoped<ICandlesService, CandlesService>();
            services.AddScoped<ISwapProfileUploadService, SwapProfileUploadService>();            
            services.AddSingleton<IPriceAggregator, PriceAggregator>();
            services.AddSingleton<IPriceRetriever, PriceRetriever>();
            services.AddSingleton<IAccountTypeFilter, AccountTypeFilter>();
            services.AddSingleton<IQuoteSourceService, QuoteSourceService>();
            services.AddSingleton<ITraderSearchService, TraderSearchService>();
            services.AddSingleton<IAccountNewTradingGroupValidator, AccountNewTradingGroupValidator>();
            services.AddSingleton<ITradingProfileNewInstrumentsValidator, TradingProfileNewInstrumentsValidator>();
        }

        public static void InitLiveDemoManager(this IServiceCollection services, LiveDemoServiceMapper mapper)
        {
            services.AddSingleton(mapper);
        }

        public static MyNoSqlTcpClient BindMyNoSql(
            this IServiceCollection services,
            LiveDemoServiceMapper liveDemoServicesMapper,
            SettingsModel settingsModel)
        {
            var tcpConnection = new MyNoSqlTcpClient(
                () => SimpleTrading.SettingsReader.SettingsReader.ReadSettings<SettingsModel>().PricesMyNoSqlServerReader,
                AppName);

            // ST Services (to be replaced in the future)
            services.AddSingleton<IBidAskCache>(tcpConnection.CreateBidAskMyNoSqlCache());
            services.AddSingleton<IInstrumentsCache>(tcpConnection.CreateInstrumentsMyNoSqlReadCache());
            services.AddSingleton<ILiquidityProviderReader>(new LiquidityProviderReader(settingsModel.QuoteFeedRouterUrl));
            services.AddSingleton(MyNoSqlServerFactory.CreateInstrumentSourcesMapsNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton((IDefaultValuesRepository)CommonMyNoSqlServerFactory.CreateDefaultValueMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton((IDefaultLiquidityProviderWriter)CommonMyNoSqlServerFactory.CreateDefaultValueMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton((IDefaultMarkupProfileWriter)CommonMyNoSqlServerFactory.CreateDefaultValueMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton<ISwapScheduleWriter>(MyNoSqlServerFactory.CreateSwapScheduleMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton<ISwapProfileWriter>(MyNoSqlServerFactory.CreateSwapProfileMyNoSqlWriter(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton<IMarkupProfilesRepository>(MyNoSqlServerFactory.CreateMarkupProfilesNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));

            liveDemoServicesMapper.InitService(true,
               services => services.ActiveOrdersReader = MyNoSqlServerFactory.CreateActiveOrdersCacheReader(tcpConnection, true));

            liveDemoServicesMapper.InitService(false,
               services => services.ActiveOrdersReader = MyNoSqlServerFactory.CreateActiveOrdersCacheReader(tcpConnection, false));

            services.AddSingleton<ITradingInstrumentsRepository>(MyNoSqlServerFactory.CreateTradingInstrumentsMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton<IInstrumentGroupsRepository>(MyNoSqlServerFactory.CreateInstrumentGroupsMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton<IInstrumentSubGroupsRepository>(MyNoSqlServerFactory.CreateInstrumentSubGroupsMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));
            services.AddSingleton(CommonMyNoSqlServerFactory.CreateTradingInstrumentMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter));

            BindLiveDemoMyNoSql(liveDemoServicesMapper, settingsModel, true);
            BindLiveDemoMyNoSql(liveDemoServicesMapper, settingsModel, false);

            return tcpConnection;
        }

        private static void BindLiveDemoMyNoSql(
            LiveDemoServiceMapper liveDemoServicesMapper,
            SettingsModel settingsModel,
            bool isLive)
        {
            var tradingProfileRepository = MyNoSqlServerFactory.CreateTradingProfilesMyNoSqlRepository(
               () => settingsModel.DictionariesMyNoSqlServerWriter, isLive);

            liveDemoServicesMapper.InitService(isLive, services => services.TradingProfileRepository = tradingProfileRepository);

            var tradingGroupsRepository = MyNoSqlServerFactory.CreateTradingGroupsMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter, isLive);

            liveDemoServicesMapper.InitService(isLive, services => services.TradingGroupsRepository = tradingGroupsRepository);

            var tradingGroupsMarkupProfilesRepository = MyNoSqlServerFactory.CreateTradingGroupsMarkupProfilesNoSqlRepository(
             () => settingsModel.DictionariesMyNoSqlServerWriter, isLive);

            liveDemoServicesMapper.InitService(isLive, services =>
                services.TradingGroupsMarkupProfilesRepository = tradingGroupsMarkupProfilesRepository);

            var tradingGroupMarkupProfilePropertieRepository = MyNoSqlServerFactory.CreateTradingGroupMarkupProfilePropertiesMyNoSqlRepository(
                () => settingsModel.DictionariesMyNoSqlServerWriter, isLive);

            liveDemoServicesMapper.InitService(isLive, services =>
                services.TradingGroupMarkupProfilePropertiesRepository = tradingGroupMarkupProfilePropertieRepository);
        }

        public static void BindPostgresRepositories(
            this IServiceCollection services,
            LiveDemoServiceMapper liveDemoServicesMapper,
            SettingsModel settingsModel)
        {
            var livePostgresConnection = new PostgresConnection(
                settingsModel.PostgresLiveConnectionString,
                AppName,
                settingsModel.PostgresLiveSchema);

            var demoPostgresConnection = new PostgresConnection(
                settingsModel.PostgresDemoConnectionString,
                AppName,
                settingsModel.PostgresDemoSchema);

            var crmDataConnection = new PostgresConnection(
                settingsModel.CrmDataPostgresConnString,
                AppName,
                settingsModel.CrmPostgresSchema);

            services.AddSingleton<ICrmDataReader>(new CrmDataReader(crmDataConnection));

            liveDemoServicesMapper.InitService(true, services => services.StReader = new StDataReader(livePostgresConnection));
            liveDemoServicesMapper.InitService(false, services => services.StReader = new StDataReader(demoPostgresConnection));
        }

        public static void BindRestClients(this IServiceCollection services)
        {
            services.AddSingleton<IRouterRestClient>(new RouterRestClient());
        }

        public static void BindAzureStorage(this IServiceCollection services, SettingsModel settingsModel)
        {
            services.AddSingleton<ICandlesPersistentStorage>(new CandlesPersistentAzureStorage(() =>
                settingsModel.AzureStorageCandlesConnection, () => settingsModel.AzureStorageCandlesConnection));
        }

        public static void BindGrpcServices(
            this IServiceCollection app,
            LiveDemoServiceMapper liveDemoServicesMapper,
            SettingsModel settings)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            app.AddSingleton(GrpcChannel
                .ForAddress(settings.CandlesHistoryServiceUrl)
                .CreateGrpcService<ISimpleTradingCandlesHistoryGrpc>());

            app.AddSingleton(GrpcChannel
                .ForAddress(settings.AuthGrpcServiceUrl)
                .CreateGrpcService<IAuthGrpcService>());

            app.AddSingleton(GrpcChannel
                .ForAddress(settings.PersonalDataGrpcServiceUrl)
                .CreateGrpcService<IPersonalDataServiceGrpc>());

            app.AddSingleton(GrpcChannel
                .ForAddress(settings.TradeLogGrpcServiceUrl)
                .CreateGrpcService<ITradeLogGrpcService>());

            app.AddSingleton(GrpcChannel
                .ForAddress(settings.TickHistoryServiceUrl)
                .CreateGrpcService<ITickHistoryServiceGrpc>());

            liveDemoServicesMapper.InitService(true, services => services.EngineApi = GrpcChannel
                .ForAddress(settings.TradingEngineLiveGrpcServerUrl)
                .CreateGrpcService<ISimpleTradingEngineApi>());

            liveDemoServicesMapper.InitService(false, services => services.EngineApi = GrpcChannel
                .ForAddress(settings.TradingEngineDemoGrpcServerUrl)
                .CreateGrpcService<ISimpleTradingEngineApi>());

            app.AddSingleton(GrpcChannel
                .ForAddress(settings.AdminServerServiceUrl)
                .CreateGrpcService<IAdminGrpc>());
        }

        public static MyServiceBusTcpClient BindServiceBus(this IServiceCollection services, SettingsModel settingsModel)
        {
            var serviceBusTcpClient = new MyServiceBusTcpClient(
                () => settingsModel.PricesMyServiceBusReader,
                AppName);

            services.AddSingleton(new UnfilteredBidAskMyServiceBusSubscriber(
                serviceBusTcpClient,
                AppName,
                MyServiceBus.Abstractions.TopicQueueType.DeleteOnDisconnect,
                false));

            services.AddSingleton(new BidAskMyServiceBusSubscriber(
                serviceBusTcpClient,
                AppName,
                MyServiceBus.Abstractions.TopicQueueType.DeleteOnDisconnect,
                "bidask",
                false));

            services.AddSingleton(new UnfilteredBidAskMyServiceBusPublisher(serviceBusTcpClient));

            services.AddSingleton(new BidAskMyServiceBusPublisher(serviceBusTcpClient));

            services.AddSingleton(new CandlesHistoryMyServiceBusPublisher(serviceBusTcpClient));

            return serviceBusTcpClient;
        }

        public static void BindLogger(this IServiceCollection services, SettingsModel settings)
        {
            var logger = new LoggerConfiguration()
                .Enrich.WithProperty("AppName", AppName)
                .Enrich.WithHttpRequestId()
                .Enrich.WithHttpRequestUrl()
                .WriteTo.Seq(settings.SeqServiceUrl)
                .CreateLogger();

            services.AddSingleton(logger);
        }

        private static string GetEnvInfo()
        {
            var info = Environment.GetEnvironmentVariable(EnvInfo);
            if (string.IsNullOrEmpty(info))
                throw new Exception($"Env Variable {EnvInfo} is not found");

            return info;
        }
    }
}