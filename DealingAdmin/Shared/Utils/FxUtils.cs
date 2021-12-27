using DealingAdmin.Abstractions.Models;
using SimpleTrading.Abstraction.BidAsk;
using SimpleTrading.Abstraction.Trading.Positions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DealingAdmin
{
    public static class FxUtils
    {
        public static int CalcSpread(BidAskModel bidAsk, int instrumentDigits)
        {
            var multiplier = Math.Pow(10, instrumentDigits);
            return (int)(bidAsk.Ask * multiplier - bidAsk.Bid * multiplier);
        }

        public static int CalcSpread(UnfilteredBidAskModel bidAsk, int instrumentDigits)
        {
            var multiplier = Math.Pow(10, instrumentDigits);
            return (int)(bidAsk.Ask * multiplier - bidAsk.Bid * multiplier);
        }

        public static double CalcSpread(IBidAsk bidAsk, int instrumentDigits)
        {
            var multiplier = Math.Pow(10, instrumentDigits);
            return (bidAsk.Ask * multiplier - bidAsk.Bid * multiplier);
        }

        public static int CalcTicks(double priceBefore, double priceAfter, int instrumentDigits)
        {
            var multiplier = Math.Pow(10, instrumentDigits);
            return (int)(priceAfter * multiplier - priceBefore * multiplier);
        }

        public static double GetOrderPrice(PositionOperation op, BidAskModel bidAsk)
        {
            return op == PositionOperation.Buy
                ? bidAsk.Bid
                : bidAsk.Ask;
        }

        public static double CalcProfitLoss(ITradeOrder order, BidAskModel bidAsk)
        {
            var nowPrice = GetOrderPrice(order.Operation, bidAsk);
            var opSide = order.Operation == PositionOperation.Buy ? 1 : -1;
            return (nowPrice / order.OpenPrice - 1) * order.InvestmentAmount * order.Leverage * opSide;
        }

        public static double CalcProfitLossByPrice(ActivePositionViewModel order, double nowPrice)
        {
            var opSide = order.Operation == PositionOperation.Buy ? 1 : -1;
            return (nowPrice / order.OpenPrice - 1) * order.InvestmentAmount * order.Leverage * opSide;
        }

        public static double CalcFloatProfitLoss(ActivePosition pos, BidAskModel bidAsk)
        {   
            var nowPrice = pos.Operation == 0 ? bidAsk.Bid : bidAsk.Ask;
            var opSide = pos.Operation == 0 ? 1 : -1;
            return (nowPrice / pos.OpenPrice - 1) * pos.InvestmentAmount * pos.Leverage * opSide;
        }

        public static double CalcNetProfit(ActivePosition pos, BidAskModel bidAsk)
        {
            var nowPrice = pos.Operation == 0 ? bidAsk.Bid : bidAsk.Ask;
            var opSide = pos.Operation == 0 ? 1 : -1;
            return (nowPrice / pos.OpenPrice - 1) * pos.InvestmentAmount * pos.Leverage * opSide + pos.OpenSwaps;
        }

        public static double CalcFloatProfitLoss(ActivePositionViewModel pos, double nowPrice)
        {
            var opSide = pos.Operation == 0 ? 1 : -1;
            return (nowPrice / pos.OpenPrice - 1) * pos.InvestmentAmount * pos.Leverage * opSide;
        }

        public static double GetAveragePrice(List<ActivePosition> orders)
        {
            if (!orders.Any())
            {
                return 0;
            }

            var minVolume = orders.OrderBy(x => x.Volume).First().Volume;
            var sumCoeff = orders.Sum(x => x.Volume) / minVolume;
            var avgPrice = orders.Sum(x => x.Volume * x.OpenPrice / minVolume) / sumCoeff;
            return avgPrice;
        }
    }
}
