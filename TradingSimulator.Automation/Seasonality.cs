using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class Seasonality : Strategy
    {
        List<Position> positions;
        public override void Run(Bar[] bars, Action doEvent)
        {
            positions = new List<Position>();

            for (var i = 20; i < bars.Length - 1; i++)
           

      
            // buying stocks 1 day of month
            {

                   if 
                    (
                    bars[i + 1].Date.Month != bars[i].Date.Month
                    && (bars[i].Date.Month > 8
                    || bars[i].Date.Month < 5)
                    )
                       positions.Add(Entry.BuyAtMarket(bars[i]));                                                   

                   if(positions.Count > 0)
                   for(var x = 0; x < positions.Count; x++)
                   {
                           Perf.Trades.Add(Exit.CloseAtClose(positions[x], bars[i + 4]));
                           positions.Remove(positions[x]);
                   }

               } 

            // break out

        /*    {

                if (bars[i - 1].Close > bars[i - 2].High)
                    if(Entry.BuyAtStop(bars[i], bars[i - 1].Close + bars[i - 1].Range()) != null)
                    positions.Add(Entry.BuyAtStop(bars[i], bars[i - 1].Close + bars[i - 1].Range()));

                if (positions.Count > 0)
                    for (var x = 0; x < positions.Count; x++)
                    {
                       
                            Perf.Trades.Add(Exit.CloseAtClose(positions[x], bars[i + 2]));
                            positions.Remove(positions[x]);
                        
                    }

            } */
        }





      }
}
