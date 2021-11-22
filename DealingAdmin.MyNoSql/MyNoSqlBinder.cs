using System;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;
using MyNoSqlServer.DataWriter;
using DealingAdmin.Abstractions;
using DealingAdmin.MyNoSql.CrossTickers;
using DealingAdmin.MyNoSql.InstrumentGroup;
using DealingAdmin.MyNoSql.InstrumentSubGroup;
using DealingAdmin.MyNoSql.Tickers;
using DealingAdmin.MyNoSql.TradingProfile;

namespace DealingAdmin.MyNoSql
{
    public static class MyNoSqlFactory
    {
        private const string TickersTable = "tickers";
        private const string CrossTickersTable = "cross-tickers";
        private const string TradingProfilesTableName = "tradingprofiles";
        private const string InstrumentSubGroupsTable = "instrumentsubgroups";
        private const string InstrumentGroupsTable = "instrumentsgroups";

        private static string IsLivePrefix(bool isLive)
        {
            return isLive ? "live-" : "demo-";
        }
        
        public static ITickerRepository CreateTickerMyNoSqlRepository(Func<string> getUrl)
        {
            return new TickerMyNoSqlRepository(
                new MyNoSqlServerDataWriter<TickerMyNoSqlModel>(getUrl, TickersTable, true));
        }

        public static ITickerReader CreateTickerMyNoSqlReader(this MyNoSqlTcpClient connection)
        {
            return new TickerMyNoSqlReader(
                connection.ToMyNoSqlReadRepository<TickerMyNoSqlModel>(TickersTable));
        }
        
        public static ICrossTickerRepository CreateCrossTickerMyNoSqlRepository(Func<string> getUrl)
        {
            return new CrossTickerMyNoSqlRepository(
                new MyNoSqlServerDataWriter<CrossTickerMyNoSqlModel>(getUrl, CrossTickersTable, true));
        }

        public static ICrossTickerReader CreateCrossTickerMyNoSqlReader(this MyNoSqlTcpClient connection)
        {
            return new CrossTickerMyNoSqlReader(
                connection.ToMyNoSqlReadRepository<CrossTickerMyNoSqlModel>(CrossTickersTable));
        }
        
        
        public static TradingProfileMyNoSqlReader CreateTradingProfileMyNoSqlReader(this MyNoSqlTcpClient connection,
            bool isLive)
        {
            return new TradingProfileMyNoSqlReader(
                new MyNoSqlReadRepository<TradingProfileMyNoSqlEntity>(connection,
                    IsLivePrefix(isLive) + TradingProfilesTableName));
        }

        public static TradingProfilesMyNoSqlRepository CreateTradingProfilesMyNoSqlRepository(Func<string> getUrl,
            bool isLive)
        {
            return new TradingProfilesMyNoSqlRepository(
                new MyNoSqlServerDataWriter<TradingProfileMyNoSqlEntity>(getUrl,
                    IsLivePrefix(isLive) + TradingProfilesTableName, true));
        }

        public static InstrumentSubGroupsMyNoSqlRepository CreateInstrumentSubGroupsMyNoSqlRepository(
            Func<string> getUrl)
        {
            return new InstrumentSubGroupsMyNoSqlRepository(
                new MyNoSqlServerDataWriter<InstrumentSubGroupMyNoSqlModel>(getUrl, InstrumentSubGroupsTable, true));
        }
        

        public static InstrumentGroupsMyNoSqlRepository CreateInstrumentGroupsMyNoSqlRepository(Func<string> getUrl)
        {
            return new InstrumentGroupsMyNoSqlRepository(
                new MyNoSqlServerDataWriter<InstrumentGroupMyNoSqlModel>(getUrl, InstrumentGroupsTable, true));
        }
    }
    
    public static class MyNoSqlServerUtils
    {
        public static IMyNoSqlServerDataReader<T> ToMyNoSqlReadRepository<T>(this IMyNoSqlSubscriber connection,
            string tableName)
            where T : IMyNoSqlDbEntity
        {
            return new MyNoSqlReadRepository<T>(connection, tableName);
        }
    }
}