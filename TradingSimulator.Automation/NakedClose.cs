using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class NakedClose : Strategy
    {
        List<Position> positions;
        public override void Run(Bar[] bars, Action doEvent)
        {
            positions = new List<Position>();

            for (var i = 5; i < bars.Length - 600; i++)
            //   for (var i = bars.Length - 600; i < bars.Length - 1; i++)
            //    for (var i = 20; i < bars.Length - 1; i++)
                {
                var condition = true;
                for (var x = 1; x < 6; x++)
                {
                    if (bars[i - x].Close < bars[i - x].Open)
                        condition = default(bool);
                }
                    if(condition)
                        positions.Add(Entry.BuyAtMarket(bars[i]));


                    if (positions.Count > 0)
                    for (var x = 0; x < positions.Count; x++)
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(positions[x], bars[i]));
                        positions.Remove(positions[x]);                        
                    }
                }
        
          }
      }
}
