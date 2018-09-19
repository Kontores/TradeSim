using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class RangeBreakOut : Strategy
    {
        public override void Run(Bar[] bars, Action doEvent)
        {
           for (var i = 2; i < bars.Length; i++)
        
            {
                if (Position == null)
                {
                    if (bars[i].Date.DayOfWeek == DayOfWeek.Monday
                        || bars[i].Date.DayOfWeek == DayOfWeek.Thursday)
                    {
                        if(bars[i - 1].Date.DayOfWeek != DayOfWeek.Sunday)
                            Position = Entry.BuyAtStop(bars[i], bars[i].Open + bars[i - 1].Range());
                        else
                            Position = Entry.BuyAtStop(bars[i], bars[i].Open + bars[i - 2].Range());
                    }
                    
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
