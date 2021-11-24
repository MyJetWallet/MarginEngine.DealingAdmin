using System.Collections.Concurrent;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DeakingAdmin.Abstractions;
using DeakingAdmin.Abstractions.Models;
using DotNetCoreDecorators;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using SimpleTrading.Abstraction.Candles;
using SimpleTrading.Abstraction.Trading.Settings;
using SimpleTrading.CandlesHistory.AzureStorage;
using SimpleTrading.CandlesHistory.Grpc;
using SimpleTrading.CandlesHistory.Grpc.Contracts;
using SimpleTrading.CandlesHistory.Grpc.Models;
using SimpleTrading.GrpcTemplate;
using SimpleTrading.ServiceBus.Contracts;

namespace DeakingAdmin.Services
{
    public class CandlesServiceSettings
    {
        public int CandlesSaveChunkSize { get; set; }

        public string CandlesExpiresMinutes { get; set; }

        public string CandlesExpiresHours { get; set; }
    }

    public class CandlesService : ICandlesService
    {
        private const string ValidFileExtention = ".csv";

        private readonly CandlesServiceSettings _settings;

        private readonly ICandlesPersistentStorage _candlesPersistentStorage;

        private readonly GrpcServiceClient<ISimpleTradingCandlesHistoryGrpc> _candlesHistoryService;

        public static IPublisher<UpdateCandlesHistoryServiceBusContract> _candlesUpdatePublisher;

        private readonly IInstrumentsCache _instrumentsCache;

        private readonly Logger _logger;

        public CandlesService(
            GrpcServiceClient<ISimpleTradingCandlesHistoryGrpc> candlesHistoryService,
            ICandlesPersistentStorage candlesPersistentStorage,
            IInstrumentsCache instrumentsCache,
            IPublisher<UpdateCandlesHistoryServiceBusContract> candlesUpdatePublisher,
            CandlesServiceSettings serviceSettings,
            Logger logger)
        {
            _candlesHistoryService = candlesHistoryService;
            _candlesPersistentStorage = candlesPersistentStorage;
            _instrumentsCache = instrumentsCache;
            _candlesUpdatePublisher = candlesUpdatePublisher;
            _settings = serviceSettings;
        }

        public async Task<List<CandleModel>> ParseCandlesByType(
          CandleType candleType,
          IBrowserFile csvFile)
        {
            if (csvFile == null)
                throw new ArgumentException("Bad file request");

            var candles = new List<CandleModel>();

            if (candleType == CandleType.Minute
                || candleType == CandleType.Hour)
            {
                var recordsCsv = await ParseCandlesWithTime(csvFile);
                candles = recordsCsv.Select(x => x.ToStorageModel()).ToList();
                recordsCsv = null;
            }
            else if (candleType == CandleType.Day
                || candleType == CandleType.Month)
            {
                var recordsCsv = await ParseCandles(csvFile);
                candles = recordsCsv;
            }
            else
            {
                throw new NotImplementedException("Unhandled Candle Type");
            }

            return candles;
        }

        public static async Task<List<CandleWithTime>> ParseCandlesWithTime(IBrowserFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { TrimOptions = TrimOptions.None, Delimiter = "\t" };
            using var csv = new CsvReader(reader, config);
            var hasRecords = await csv.ReadAsync();

            if (!hasRecords)
            {
                throw new Exception("File contains no record");
            }

            csv.Context.RegisterClassMap<CandleWithTimeMap>();

            var recordsCsv = csv.GetRecords<CandleWithTime>().ToList();
            return recordsCsv;
        }

        public static async Task<List<CandleModel>> ParseCandles(IBrowserFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { TrimOptions = TrimOptions.None, Delimiter = "\t" };
            using var csv = new CsvReader(reader, config);
            var hasRecords = await csv.ReadAsync();

            if (!hasRecords)
            {
                throw new Exception("File contains no record");
            }

            csv.Context.RegisterClassMap<CandleMap>();

            var recordsCsv = csv.GetRecords<CandleModel>().ToList();
            return recordsCsv;
        }

        public async Task WriteCandles(string instrumentId, CandleType candleType, List<CandleModel> candles)
        {
            try
            {
                var splitSize = candleType switch
                {
                    CandleType.Minute => 350,
                    CandleType.Hour => 250,
                    CandleType.Day => 100,
                    CandleType.Month => 20,
                    _ => throw new NotImplementedException("Unhandled Candle Type")
                };

                var recordsChunks = candles.SplitToChunks(splitSize);

                foreach (var records in recordsChunks)
                {
                    var tasks = new List<Task>();

                    foreach (var candle in records)
                    {
                        tasks.Add(Task.Run(async () => await _candlesPersistentStorage
                            .SaveAsync(instrumentId, true, 5, candleType, candle)));
                    }

                    await Task.WhenAll(tasks);
                }

                await _candlesUpdatePublisher.PublishAsync(
                    new UpdateCandlesHistoryServiceBusContract
                    {
                        InstrumentId = instrumentId,
                        CandleType = candleType,
                        DateFrom = candles.OrderBy(x => x.DateTime).First().DateTime,
                        DateTo = candles.OrderByDescending(x => x.DateTime).First().DateTime,
                        CacheIsUpdated = false
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Write {Enum.GetName(typeof(CandleType), candleType)} candles went wrong");
            }
        }

        public async Task BulkWriteCandles(string instrumentId, CandleType candleType, List<CandleModel> candles)
        {
            try
            {
                // arrange candles by partitions to avoid the data loss during the concurrent writing to DB
                var partitionsDict = new ConcurrentDictionary<string, List<CandleModel>>();

                foreach (var candle in candles)
                {
                    var partitionKey = _candlesPersistentStorage.GetPartitionKey(candle.DateTime, candleType);

                    if (!partitionsDict.ContainsKey(partitionKey))
                        partitionsDict[partitionKey] = new List<CandleModel>();

                    partitionsDict[partitionKey].Add(candle);
                }

                var tasks = new List<Task>();

                // prepare async tasks for writing the candles by partitions
                foreach (var partition in partitionsDict.Keys)
                {
                    if (partitionsDict[partition].Count <= _settings.CandlesSaveChunkSize)
                    {
                        tasks.Add(Task.Run(async () => await _candlesPersistentStorage
                            .BulkSave(instrumentId, true, 5, candleType, partitionsDict[partition])));
                    }
                    else
                    {
                        // split by chunks to keep a consistence of the records in a single partition
                        var partitionCandlesChunks = partitionsDict[partition].SplitToChunks(_settings.CandlesSaveChunkSize);

                        tasks.Add(Task.Run(async () =>
                        {
                            foreach (var chunk in partitionCandlesChunks)
                            {
                                await _candlesPersistentStorage.BulkSave(instrumentId, true, 5, candleType, chunk);
                            }
                        }));
                    }
                }

                await Task.WhenAll(tasks);

                await _candlesUpdatePublisher.PublishAsync(
                    new UpdateCandlesHistoryServiceBusContract
                    {
                        InstrumentId = instrumentId,
                        CandleType = candleType,
                        DateFrom = candles.OrderBy(x => x.DateTime).First().DateTime,
                        DateTo = candles.OrderByDescending(x => x.DateTime).First().DateTime,
                        CacheIsUpdated = false
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception($"Write {Enum.GetName(typeof(CandleType), candleType)} candles went wrong");
            }
        }

        public async Task<CandlesVerificationResult> VerifyDbCandles(
            string instrumentId,
            List<CandleModel> candles,
            CandleType candleType,
            bool stopWithFirstDiff)
        {
            try
            {
                var partitionsDict = new ConcurrentDictionary<string, List<CandleModel>>();

                foreach (var candle in candles)
                {
                    var partitionKey = _candlesPersistentStorage.GetPartitionKey(candle.DateTime, candleType);

                    if (!partitionsDict.ContainsKey(partitionKey))
                        partitionsDict[partitionKey] = new List<CandleModel>();

                    partitionsDict[partitionKey].Add(candle);
                }

                var scannedCount = 0;
                var correspondingRecordsCount = 0;
                var differingRecordsCount = 0;
                var missedRecordsCount = 0;
                var stopScan = false;

                foreach (var partition in partitionsDict.Keys)
                {
                    if (stopScan)
                        break;

                    var dbPartitionCandlesDict = new ConcurrentDictionary<DateTime, ICandleModel>();

                    await foreach (var dbCandle in _candlesPersistentStorage
                            .GetByPartitionKeyAsync(instrumentId, true, partition, candleType))
                    {
                        dbPartitionCandlesDict[dbCandle.DateTime] = dbCandle;
                    }

                    if (dbPartitionCandlesDict.IsEmpty)
                    {
                        missedRecordsCount += partitionsDict[partition].Count;
                        scannedCount += partitionsDict[partition].Count;
                    }
                    else
                    {
                        foreach (var candle in partitionsDict[partition])
                        {
                            scannedCount++;

                            if (dbPartitionCandlesDict.ContainsKey(candle.DateTime))
                            {
                                var dbCandle = dbPartitionCandlesDict[candle.DateTime];

                                if (candle.Open == dbCandle.Open && candle.Close == dbCandle.Close
                                    && candle.High == dbCandle.High && candle.Low == dbCandle.Low)
                                {
                                    correspondingRecordsCount++;
                                }
                                else
                                {
                                    differingRecordsCount++;

                                    if (stopWithFirstDiff)
                                    {
                                        stopScan = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                missedRecordsCount++;

                                if (stopWithFirstDiff)
                                {
                                    stopScan = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                return new CandlesVerificationResult
                {
                    TotalCount = candles.Count,
                    ActualCount = candles.Count,
                    ScannedCount = scannedCount,
                    CorrespondingCount = correspondingRecordsCount,
                    DifferingCount = differingRecordsCount,
                    MissedCount = missedRecordsCount
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.Error(e, e.Message);
                throw new Exception("VerifyDbCandles went wrong");
            }
        }

        public async Task<CandlesVerificationResult> VerifyCacheCandles(
            string instrumentId,
            List<CandleModel> candles,
            CandleType candleType,
            bool stopWithFirstDiff)
        {
            try
            {
                var totalCandlesCount = candles.Count;

                // screen out expired candles for Minute and Hour types
                if (candleType == CandleType.Minute)
                {
                    var minuteCandlesExpirationDate = DateTime.UtcNow - TimeSpan.Parse(_settings.CandlesExpiresMinutes);
                    candles = candles.Where(x => x.DateTime > minuteCandlesExpirationDate).ToList();
                }
                else if (candleType == CandleType.Hour)
                {
                    var hourCandlesExpirationDate = DateTime.UtcNow - TimeSpan.Parse(_settings.CandlesExpiresHours);
                    candles = candles.Where(x => x.DateTime > hourCandlesExpirationDate).ToList();
                }

                var dateFrom = candles.OrderBy(x => x.DateTime).First().DateTime;
                var dateTo = candles.OrderBy(x => x.DateTime).Last().DateTime;

                var cacheRequest = new GetCandlesHistoryGrpcRequestContract
                {
                    Instrument = instrumentId,
                    From = dateFrom,
                    To = dateTo,
                    CandleType = candleType,
                    Bid = true
                };


                var filteredCandlesCount = candles.Count;
                var cacheCandles = new List<CandleGrpcModel>();

                await foreach (var cacheCandle in _candlesHistoryService
                    .Value.GetCandlesHistoryStream(cacheRequest))
                {
                    cacheCandles.Add(cacheCandle);
                }

                var scannedCount = 0;
                var correspondingRecordsCount = 0;
                var differingRecordsCount = 0;
                var missedRecordsCount = 0;

                if (cacheCandles.Count == 0)
                {
                    missedRecordsCount = filteredCandlesCount;
                    scannedCount += filteredCandlesCount;
                }
                else
                {
                    foreach (var candle in candles)
                    {
                        scannedCount++;
                        var cacheCorrespCandles = cacheCandles.Where(x => x.DateTime == candle.DateTime);

                        if (cacheCorrespCandles.Any())
                        {
                            var cacheCandle = cacheCorrespCandles.First();

                            if (candle.Open == cacheCandle.Open && candle.Close == cacheCandle.Close
                                && candle.High == cacheCandle.High && candle.Low == cacheCandle.Low)
                            {
                                correspondingRecordsCount++;
                            }
                            else
                            {
                                differingRecordsCount++;

                                if (stopWithFirstDiff)
                                {
                                    _logger.Warning($"VerifyCacheCandles -> FirstDiff: Diff Date: {cacheCandle.DateTime})");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            missedRecordsCount++;

                            if (stopWithFirstDiff)
                            {
                                _logger.Warning($"VerifyCacheCandles -> FirstDiff: Missed Date: {candle.DateTime})");
                                break;
                            }
                        }
                    }
                }

                return new CandlesVerificationResult
                {
                    TotalCount = totalCandlesCount,
                    ActualCount = filteredCandlesCount,
                    ScannedCount = scannedCount,
                    CorrespondingCount = correspondingRecordsCount,
                    DifferingCount = differingRecordsCount,
                    MissedCount = missedRecordsCount
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.Error(e, e.Message);
                throw new Exception("VerifyCacheCandles went wrong");
            }
        }

        public async Task<CandleModel> GetDbCandleValue(
            string instrumentId,
            CandleType candleType,
            DateTime dateTime)
        {
            try
            {
                return CandleToModel(await _candlesPersistentStorage.GetCandleAsync(instrumentId, true, candleType, dateTime));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.Error(e, e.Message);
                _logger.Error(
                    "Error occured in GetDbCandleValue for {instrumentId} [type-{candleType}] {dateTime}",
                    instrumentId,
                    Enum.GetName(typeof(CandleType), candleType),
                    dateTime.ToString("s"));

                throw new ApplicationException("Can't read candle value from storage");
            }
        }

        public async Task<CandleModel> GetCacheCandleValue(
            string instrumentId,
            CandleType candleType,
            DateTime dateTime)
        {
            try
            {
                var cacheRequest = new GetCandlesHistoryGrpcRequestContract
                {
                    Instrument = instrumentId,
                    From = dateTime,
                    To = dateTime,
                    CandleType = candleType,
                    Bid = true
                };


                var candleGrpcList = await _candlesHistoryService.Value.GetCandlesHistoryAsync(cacheRequest);

                if (candleGrpcList.Any())
                {
                    var candleGrpc = candleGrpcList.First();

                    return new CandleModel
                    {
                        DateTime = candleGrpc.DateTime,
                        Open = candleGrpc.Open,
                        Close = candleGrpc.Close,
                        High = candleGrpc.High,
                        Low = candleGrpc.Low
                    };
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.Error(e, e.Message);
                _logger.Error(
                    "Error occured in GetCacheCandleValue for {instrumentId} [type-{candleType}] {dateTime}",
                    instrumentId,
                    Enum.GetName(typeof(CandleType), candleType),
                    dateTime.ToString("s"));

                throw new ApplicationException("Can't read candle value from cache");
            }
        }

        public async Task SaveCandle(CandleEditContract editContract)
        {
            var instrument = _instrumentsCache.Get(editContract.InstrumentId);

            if (instrument == null)
            {
                throw new ApplicationException($"Instrument {editContract.InstrumentId} was not found in InstrumentsCache");
            }

            _logger.Information(
               "SaveCandle for {instrumentId} [type:{candleType}][Digits:{digits}] {dateTime} - Values[O:{open}][C:{close}][H:{high}][L:{low}]",
                   editContract.InstrumentId,
                   Enum.GetName(typeof(CandleType), editContract.Type),
                   instrument.Digits,
                   editContract.DateTime.ToString("s"),
                   editContract.Open,
                   editContract.Close,
                   editContract.High,
               editContract.Low
               );

            await _candlesPersistentStorage.SaveAsync(
                editContract.InstrumentId,
                true,
                instrument.Digits,
                editContract.Type,
                editContract);

            var updateDateFrom = editContract.Type switch
            {
                CandleType.Minute => editContract.DateTime.AddMinutes(-1),
                CandleType.Hour => editContract.DateTime.AddHours(-1),
                CandleType.Day => editContract.DateTime.AddDays(-1),
                CandleType.Month => editContract.DateTime.AddMonths(-1),
                _ => throw new NotImplementedException("Unhandled Candle Type")
            };

            var updateDateTo = editContract.Type switch
            {
                CandleType.Minute => editContract.DateTime.AddMinutes(1),
                CandleType.Hour => editContract.DateTime.AddHours(1),
                CandleType.Day => editContract.DateTime.AddDays(1),
                CandleType.Month => editContract.DateTime.AddMonths(1),
                _ => throw new NotImplementedException("Unhandled Candle Type")
            };

            await _candlesUpdatePublisher.PublishAsync(
                new UpdateCandlesHistoryServiceBusContract
                {
                    InstrumentId = editContract.InstrumentId,
                    CandleType = editContract.Type,
                    DateFrom = updateDateFrom,
                    DateTo = updateDateTo,
                    CacheIsUpdated = false
                });
        }

        public static CandleModel CandleToModel(ICandleModel src)
        {
            return new CandleModel()
            {
                DateTime = src.DateTime,
                Open = src.Open,
                Close = src.Close,
                High = src.High,
                Low = src.Low
            };
        }
    }

    public class CandleWithTime
    {
        public DateTime Date { get; set; }

        public string Time { get; set; }
        
        public double Open { get; set; }

        public double Close { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public CandleModel ToStorageModel()
        {
            var ts = TimeSpan.Parse(Time);
            return new CandleModel
            {
                DateTime = Date.Add(ts),
                Open = Open,
                Close = Close,
                High = High,
                Low = Low
            };
        }
    }
    
    public sealed class CandleWithTimeMap : ClassMap<CandleWithTime>
    {
        public CandleWithTimeMap()
        {
            Map(p => p.Date).Name("DATE");
            
            Map(p => p.Time).Name("TIME");

            Map(p => p.Open).Name("OPEN");

            Map(p => p.High).Name("HIGH");

            Map(p => p.Low).Name("LOW");

            Map(p => p.Close).Name("CLOSE");
        }
    }
    
    public sealed class CandleMap : ClassMap<CandleModel>
    {
        public CandleMap()
        {
            Map(p => p.DateTime).Name("DATE");
            
            Map(p => p.Open).Name("OPEN");

            Map(p => p.High).Name("HIGH");

            Map(p => p.Low).Name("LOW");

            Map(p => p.Close).Name("CLOSE");
        }
    }
}