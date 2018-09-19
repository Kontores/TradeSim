using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    /// <summary>
    ///
    /// </summary>
    public class NDPI : Strategy
    {
        List<Position> positions;
        public override void Run(Bar[] bars, Action doEvent)
        {
            positions = new List<Position>();

            for (var i = 0; i < bars.Length - 1; i++)
            //      for (var i = bars.Length - 600; i < bars.Length - 1; i++)
            //    for (var i = 20; i < bars.Length - 1; i++)
            {
                //   if (bars[i].Date.Year < 2016)
                //        continue;
                if (bars[i].Date.Month != 7)
                    continue;
               
                if (positions.Count == 0)
                {
                    if (bars[i].Date.Day < 25 && bars[i + 1].Date.Day >= 25)
                        positions.Add(Entry.ShortAtMarket(bars[i])); 
                    //   if(Entry.ShortAtStop(bars[i], bars[i].Open - bars[i - 1].Range() / 2) != null)
                   // positions.Add(Entry.ShortAtStop(bars[i], bars[i].Open - bars[i - 1].Range() / 2));
                  //  positions.Last().StopLoss = positions.Last().EntryPrice - bars[i - 1].Range();
                 
                }

                if (positions.Count > 0)
                    for (var x = 0; x < positions.Count; x++)
                    {
          //              if (bars[i].Date.Day == 24 || (bars[i].Date.Day == 23 && bars[i + 1].Date.Day == 26) || (bars[i].Date.Day == 22 && bars[i + 1].Date.Day == 25))

                        {
                            Perf.Trades.Add(Exit.CloseAtClose(positions[x], bars[i]));
                            positions.Remove(positions[x]);
                        }                       
                    }
            }
        }
    
    }
}
