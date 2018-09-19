using System;
using System.Collections.Generic;
using System.Text;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class SugarLow : Strategy
    {
        public override void Run(Bar[] bars, Action doEvents)
        {          
            for(var i = 3; i < bars.Length - 1; i++)
            {
                if(Position == null)
                {
                    if (
                        bars[i - 1].Close < bars[i - 2].Low &&
                        bars[i - 3].Close > bars[i - 3].Open 
                        )
                    {
                        Position = Entry.ShortAtMarket(bars[i]);
                    }
                }
                else
                {                   
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i - 1]));
                        Position = null;
                    }
                }
            }
        }
    }
}
