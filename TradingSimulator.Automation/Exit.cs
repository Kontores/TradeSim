using System;
using System.Collections.Generic;
using System.Text;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public static class Exit
    {
        public static Trade CloseAtMarket(Position pos, Bar bar) => pos.Close(bar.Date, bar.Open);
        public static Trade CloseAtClose(Position pos, Bar bar) => pos.Close(bar.Date, bar.Close);
    }
}
