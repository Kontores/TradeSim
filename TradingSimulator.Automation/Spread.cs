using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class Spread : Strategy
    {

        public override void Run(Bar[] bars, Action doEvent)
        {
            for (var i = 2; i < bars.Length; i++)
            {

                if (Position == null)
                {
                    if(bars[i].Date.Hour == 23)
                    {
                        if (bars[i].Date.DayOfWeek == DayOfWeek.Friday) continue;
                        if (bars[i - 1].Close < bars[i - 2].High) continue;     
                        Position = Entry.ShortAtLimit(bars[i], bars[i - 1].High + bars[i - 1].Range() *0.05m);
                        if (Position != null)
                        {
                            Position.TakeProfit = bars[i].Low - 0.0001m;
                        }
                    }
                }
                else
                {
                   
                    if (bars[i].Low <= Position.TakeProfit)
                    {
                        Perf.Trades.Add(Position.Close(bars[i].Date, Position.TakeProfit));
                        Position = null;
                    }
              
                    else if (bars[i + 1].Low <= Position.TakeProfit)
                    {
                        Perf.Trades.Add(Position.Close(bars[i + 1].Date, Position.TakeProfit));
                        Position = null;
                    }                 

                    else if (bars[i + 2].Low <= Position.TakeProfit)
                    {
                        Perf.Trades.Add(Position.Close(bars[i + 1].Date, Position.TakeProfit));
                        Position = null;
                    }
                    else
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i + 2]));
                        Position = null;
                    }
                }

                doEvent();

            }
        }
    }
}
