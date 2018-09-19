using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class InsideDayNikkei : Strategy
    {
        decimal stopLoss;
        public override void Run(Bar[] bars, Action doEvent)
        {
             for(var i = 5; i < bars.Length; i++)
        
            {
                if (Position == null)
                {
                    if (bars[i].Date.DayOfWeek == DayOfWeek.Monday|| bars[i].Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        if(bars[i - 1].Date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if (bars[i - 2].High < bars[i - 3].High
                          && bars[i - 2].Low > bars[i - 3].Low)
                                Position = Entry.BuyAtMarket(bars[i]);                         
                        }
                        else if (bars[i - 1].High < bars[i - 2].High
                        && bars[i - 1].Low > bars[i - 2].Low)
                            Position = Entry.BuyAtMarket(bars[i]);
                    }
                        

                }
                else
                {
                    stopLoss = Position.EntryPrice - ATR(bars, 1, i - 1);

                    if (bars[i - 1].Low <= stopLoss)
                    {
                        Perf.Trades.Add(Position.Close(bars[i - 1].Date, stopLoss));
                        Position = null;
                    }
                    else if (bars[i - 1].Close > Position.EntryPrice)
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i - 1]));
                        Position = null;
                    }

                    else if ((bars[i - 1].Date - Position.EntryDate).TotalDays >= 5)
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i - 1]));
                        Position = null;
                    }
                }

            }
        }

        public decimal ATR(Bar[] bars, int period, int currentBarIndex)
        {
            var range = default(decimal);

            for(var i = 1; i <= period; i++)
                range += bars[currentBarIndex - i].Range();

            return range / period;

        }
      
    }
}
