using System.Collections.Concurrent;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Excel;
using DealingAdmin.Abstractions;
using DealingAdmin.Abstractions.Models;
using Microsoft.AspNetCore.Components.Forms;
using Serilog.Core;
using SimpleTrading.Abstraction.Trading.Settings;

namespace DealingAdmin.Services
{
    public class SwapProfileUploadService : ISwapProfileUploadService
    {
        private readonly IInstrumentsCache _instrumentsCache;

        private readonly Logger _logger;

        public SwapProfileUploadService(IInstrumentsCache instrumentsCache, Logger logger)
        {
            _instrumentsCache = instrumentsCache;
            _logger = logger;
        }

        public async Task<List<SwapProfileModel>> ParseSwapItemsFromExcelFile(IBrowserFile file, string profileId = "")
        {
            if (file == null)
                throw new ArgumentException("File is null");

            var swapItems = new List<SwapProfileModel>();

            using (var memStream = new MemoryStream())
            {
                // although file.OpenReadStream is itself a stream,
                // using it directly causes "Synchronous reads are not supported" error
                await file.OpenReadStream().CopyToAsync(memStream);

                // at the end of the copy method, we are at the end of both the input and output stream
                // and need to reset the one we want to work with.
                memStream.Seek(0, SeekOrigin.Begin);

                using var parser = new ExcelParser(memStream, CultureInfo.InvariantCulture);
                parser.Context.RegisterClassMap<SwapItemExcelModelMap>();
                using var reader = new CsvReader(parser);

                var excelItems = reader.GetRecords<SwapItemExcelModel>();

                if (!excelItems.Any())
                {
                    throw new Exception("File contains no record");
                }

                return excelItems.Select(x => x.ToSwapModel(profileId)).ToList();
            }
        }
    }

    public class SwapItemExcelModel
    {
        public string InstrumentId { get; set; }

        public double Long { get; set; }

        public double Short { get; set; }
    }

    public sealed class SwapItemExcelModelMap : ClassMap<SwapItemExcelModel>
    {
        public SwapItemExcelModelMap()
        {
            Map(s => s.InstrumentId).Name("ID");   

            Map(s => s.Long).Name("Swap Long");

            Map(s => s.Short).Name("Swap Short");
        }
    }

    public static class SwapProfileMapper
    {
        public static SwapProfileModel ToSwapModel(this SwapItemExcelModel item, string profileId)
        {
            return new SwapProfileModel
            {
                Id = profileId,
                InstrumentId = item.InstrumentId,
                Long = item.Long,
                Short = item.Short,
            };
        }
    }
}