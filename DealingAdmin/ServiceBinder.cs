using System;
using System.Collections.Generic;
using DealingAdmin.Abstractions;
using DealingAdmin.MyNoSql;
using Microsoft.Extensions.DependencyInjection;
using MyNoSqlServer.DataReader;

namespace DealingAdmin
{
    public class LiveDemoServices
    {
        public ITradingProfileRepository TradingProfileRepository { get; set; }
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
        private const string AppName = "DealerAdmin";

        public static void InitLiveDemoManager(this IServiceCollection serviceCollection, LiveDemoServiceMapper mapper)
        {
            serviceCollection.AddSingleton(mapper);
        }
        
        public static MyNoSqlTcpClient BindMyNoSql(this IServiceCollection serviceCollection, SettingsModel settingsModel, LiveDemoServiceMapper liveDemoServicesMapper)
        {
            var tcpConnection = new MyNoSqlTcpClient(() => settingsModel.MyNoSqlTcpUrl, AppName);
            

            serviceCollection.AddSingleton(tcpConnection.CreateTickerMyNoSqlReader());
            serviceCollection.AddSingleton(MyNoSqlFactory.CreateTickerMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));
            serviceCollection.AddSingleton(tcpConnection.CreateCrossTickerMyNoSqlReader());
            serviceCollection.AddSingleton(MyNoSqlFactory.CreateCrossTickerMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));
            serviceCollection.AddSingleton(MyNoSqlFactory.CreateInstrumentSubGroupsMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));
            serviceCollection.AddSingleton(MyNoSqlFactory.CreateInstrumentGroupsMyNoSqlRepository(() => settingsModel.MyNoSqlRestUrl));
            
            
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

        public static void BindRestClients(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRouterRestClient>(new RouterRestClient());
        }
    }
}