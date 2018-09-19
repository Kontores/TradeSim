using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class MonEU : Strategy
    {
        public override void Run(Bar[] bars, Action doEvent)
        {
           
            for (var i = 1; i < bars.Length; i++)
            {              

                if(Position == null)
                {
                    if (bars[i].Open - bars[i - 1].Close > 0.0001m && bars[i].Date.DayOfWeek == DayOfWeek.Monday && (bars[i].Date.Month < 7 || bars[i].Date.Month > 10))
                        Position = Entry.ShortAtMarket(bars[i]); 
                    
                }
                else
                {
                   if(Position.EntryPrice > bars[i - 1].Close || bars[i - 1].Close - Position.EntryPrice >= 0.0100m) 
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i - 1]));
                        Position = null;
                    }
                }
            }
        }
    }
}
