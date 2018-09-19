using System;
using System.Collections.Generic;
using System.Text;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public static class Entry
    {
        public static Position BuyAtMarket(Bar bar) => new Position(TradeType.Long, bar.Date, bar.Open);
        public static Position BuyAtClose(Bar bar) => new Position(TradeType.Long, bar.Date, bar.Close);
        public static Position BuyAtStop(Bar bar, decimal stopPrice)
        {
            if (bar.Open < stopPrice && bar.High >= stopPrice)
                return new Position(TradeType.Long, bar.Date, stopPrice);
            return null;
        }
        public static Position BuyAtLimit(Bar bar, decimal limitPrice)
        {
            if (bar.Open > limitPrice && bar.Low <= limitPrice)
                return new Position(TradeType.Long, bar.Date, limitPrice);
            return null;
        }
        public static Position ShortAtMarket(Bar bar) => new Position(TradeType.Short, bar.Date, bar.Open);
        public static Position ShortAtClose(Bar bar) => new Position(TradeType.Short, bar.Date, bar.Close);

        public static Position ShortAtStop(Bar bar, decimal stopPrice)
        {
            if (bar.Open > stopPrice && bar.Low <= stopPrice)
                return new Position(TradeType.Short, bar.Date, stopPrice);
            return null;
        }

        public static Position ShortAtLimit(Bar bar, decimal limitPrice)
        {
            if(bar.Open < limitPrice && bar.High >= limitPrice)
                return new Position(TradeType.Short, bar.Date, limitPrice);
            return null;
        }
    }
}
