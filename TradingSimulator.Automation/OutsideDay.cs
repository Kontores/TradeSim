using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class OutsideDay : Strategy
    {
        public override void Run(Bar[] bars, Action doEvent)
        {
            for (var i = 4; i < bars.Length - 600; i++)
            {
                if (Position == null)
                {
                    if(
                        bars[i].Date.Month != 8
                        && bars[i].Date.Month != 4
                        )
                        Position = Entry.ShortAtStop(bars[i], bars[i].Open - (bars[i - 1].High - bars[i - 1].Low));
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
