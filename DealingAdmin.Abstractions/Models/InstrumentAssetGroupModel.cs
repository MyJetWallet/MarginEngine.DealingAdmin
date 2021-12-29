namespace DealingAdmin.Abstractions.Models
{
    public class TradingInstrumentAssetGroupModel : InstrumentQuoteModel
    {
        public InstrumentAssetGroup AssetGroup { get; set; }

        public string AssetGroupName { get; set; }

        public static new TradingInstrumentAssetGroupModel Create(
            SimpleTrading.Abstraction.Trading.Instruments.ITradingInstrument src)
        {
            var result = new TradingInstrumentAssetGroupModel
            {
                Id = src.Id,
                Name = src.Name,
                Digits = src.Digits,
                GroupId = src.GroupId,
                AssetGroup = GetInstrumentAssetGroup(src)
            };

            result.AssetGroupName = GetInstrumentAssetGroupName(result.AssetGroup);

            return result;
        }

        public static InstrumentAssetGroup GetInstrumentAssetGroup(
            SimpleTrading.Abstraction.Trading.Instruments.ITradingInstrument inst)
        {
            return inst.GroupId switch
            {
                "STOCK" => inst.Quote == "USD"
                    ? InstrumentAssetGroup.UsStocks
                    : InstrumentAssetGroup.EuStocks,
                "COMMODITIES" => InstrumentAssetGroup.Commodities,
                "CRYPTO" => InstrumentAssetGroup.Crypto,
                "ETF" => InstrumentAssetGroup.ETFs,
                "FOREX" => InstrumentAssetGroup.Forex,
                "INDICES" => InstrumentAssetGroup.Indices,
                _ => InstrumentAssetGroup.Other
            };
        }

        public static string GetInstrumentAssetGroupName(InstrumentAssetGroup assetGroup)
        {
            return assetGroup switch
            {
                InstrumentAssetGroup.UsStocks => "US Stocks",
                InstrumentAssetGroup.ETFs => "ETFs",
                InstrumentAssetGroup.Forex => "Forex",
                InstrumentAssetGroup.Crypto => "Crypto",
                InstrumentAssetGroup.EuStocks => "EU Stocks",
                InstrumentAssetGroup.Indices => "Indices",
                InstrumentAssetGroup.Commodities => "Commodities",
                _ => "Other"
            };
        }
    }

    public enum InstrumentAssetGroup
    {
        UsStocks,
        ETFs,
        Forex,
        Crypto,
        EuStocks,
        Indices,
        Commodities,
        Other
    }
}