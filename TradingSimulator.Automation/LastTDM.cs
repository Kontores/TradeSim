using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class LastTDM : Strategy
    {
        public override void Run(Bar[] bars, Action doEvent)
        {
           for(var i = bars.Length - 600; i < bars.Length - 2; i++)
            {
                if(Position == null)
                  {
                    var lastBars = new Bar[20];
                    for(var y = lastBars.Length - 1; y > -1; y--)
                    {
                        lastBars[y] = bars[i - y];
                    }
                    if (bars[i].Date.Month != bars[i + 1].Date.Month && bars[i -  1].Close > lastBars.Max(c => c.Low) )
                                          
                            Position = Entry.BuyAtMarket(bars[i]);                                                                
                  }
                else
                {
                    Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i + 2]));
                    Position = null;
                }
            }
        }
    }
}
