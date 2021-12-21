using System;
using System.Collections.Generic;
using DealingAdmin.Abstractions;
using DealingAdmin.MyNoSql;
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

namespace DealingAdmin
{
    public class LiveDemoServices
    {
        public ITradingProfileRepository TradingProfileRepository { get; set; }

        public IStDataReader StReader { get; set; }

        public IActiveOrdersCacheReader ActiveOrdersReader { get; set; }
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

    public static class ServiceBinder
    {
        private const string AppName = "DealingAdmin";
        private const string EnvInfo = "ENV_INFO";
        private static string AppNameWithEnvMark => $"{AppName}-{GetEnvInfo()}";

        public static void BindServices(this IServiceCollection services, SettingsModel settingsModel)
        {
            services.AddScoped<IUserMessageService, UserMessageService>();

            services.AddSingleton(new CandlesServiceSettings()
            {
                CandlesSaveChunkSize = settingsModel.CandlesSaveChunkSize,
                CandlesExpiresMinutes = settingsModel.CandlesExpiresMinutes,
                CandlesExpiresHours = settingsModel.CandlesExpiresHours
            });

            services.AddScoped<ICandlesService, CandlesService>();
            services.AddSingleton<IPriceAggregator, PriceAggregator>();
            services.AddSingleton<IPriceRetriever, PriceRetriever>();
        }

        public static void InitLiveDemoManager(this IServiceCollection services, LiveDemoServiceMapper mapper)
        {
            services.AddSingleton(mapper);
        }

        public static MyNoSqlTcpClient BindMyNoSql(
            this IServiceCollection services,
            SettingsModel settingsModel,
            LiveDemoServiceMapper liveDemoServicesMapper)
        {
            var tcpConnection = new MyNoSqlTcpClient(
                () => SimpleTrading.SettingsReader.SettingsReader.ReadSettings<SettingsModel>().PricesMyNoSqlServerReader,
                AppName);

            services.AddSingleton(tcpConnection.CreateTickerMyNoSqlReader());
            services.AddSingleton(MyNoSqlFactory.CreateTickerMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));
            services.AddSingleton(tcpConnection.CreateCrossTickerMyNoSqlReader());
            services.AddSingleton(MyNoSqlFactory.CreateCrossTickerMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));
            services.AddSingleton(MyNoSqlFactory.CreateInstrumentSubGroupsMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));
            services.AddSingleton(MyNoSqlFactory.CreateInstrumentGroupsMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));

            services.AddSingleton<IBidAskCache>(tcpConnection.CreateBidAskMyNoSqlCache());
            services.AddSingleton<IInstrumentsCache>(tcpConnection.CreateInstrumentsMyNoSqlReadCache());

            liveDemoServicesMapper.InitService(true,
               services => services.ActiveOrdersReader = MyNoSqlServerFactory.CreateActiveOrdersCacheReader(tcpConnection, true));

            liveDemoServicesMapper.InitService(false,
               services => services.ActiveOrdersReader = MyNoSqlServerFactory.CreateActiveOrdersCacheReader(tcpConnection, false));

            var liveTradingProfileRepository =
                 MyNoSqlFactory.CreateTradingProfilesMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl, true);

            var demoTradingProfileRepository =
                MyNoSqlFactory.CreateTradingProfilesMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl, false);

            liveDemoServicesMapper.InitService(true,
                services => services.TradingProfileRepository = liveTradingProfileRepository);

            liveDemoServicesMapper.InitService(false,
                services => services.TradingProfileRepository = demoTradingProfileRepository);

            return tcpConnection;
        }

        public static void BindPostgresRepositories(
            this IServiceCollection services,
            SettingsModel settingsModel,
            LiveDemoServiceMapper liveDemoServicesMapper)
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

        public static void BindGrpcServices(this IServiceCollection app, SettingsModel settings)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            app.AddSingleton(GrpcChannel
                .ForAddress(settings.CandlesHistoryServiceUrl)
                .CreateGrpcService<ISimpleTradingCandlesHistoryGrpc>());
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