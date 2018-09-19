using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class OopsPattern : Strategy
    {    

        public override void Run(Bar[] bars, Action doEvent)
        {
            for (var i = 1; i < bars.Length; i++)
            {
              
                if (Position == null)
                {                  
                    if (bars[i].Open > bars[i - 1].High)
                        Position = Entry.BuyAtMarket(bars[i]);
               
                    //   if (bars[i].Open < bars[i - 1].Low)
                     //   Position = Entry.ShortAtMarket(bars[i]);
                }
                else
                {                  
                    Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i - 1]));
                    Position = null;
                }
            }
        }
    }
}
