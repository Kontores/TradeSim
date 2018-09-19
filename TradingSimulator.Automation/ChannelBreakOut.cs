using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{

    public class ChannelBreakOut : Strategy
    {
        List<Position> positions;
        public override void Run(Bar[] bars, Action doEvent)
        {
            positions = new List<Position>();

            for (var i = 20; i < bars.Length - 600; i++)
           
            {
                for (var x = 5; x < 21; x++)
                    if (Channel(bars, x, i, 0.3m))
                    {
                        if (Entry.BuyAtStop(bars[i], bars[i - x].High) != null)
                        {
                            positions.Add(Entry.BuyAtStop(bars[i], bars[i - x].High));
                            break;
                        }
                        else if (Entry.ShortAtStop(bars[i], bars[i - x].Low) != null)
                        {
                            positions.Add(Entry.ShortAtStop(bars[i], bars[i - x].Low));
                            break;
                        }
                    }

                if (positions.Count > 0)
                    for (var x = 0; x < positions.Count; x++)
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(positions[x], bars[i]));
                        positions.Remove(positions[x]);
                    }
            }
   

                

            
        }

        private bool Channel(Bar[] bars, int count, int currentBar, decimal deviation = 0m)
        {
            var channel = new List<Bar>();
            for (var i = 1; i < count; i++)
                channel.Add(bars[currentBar - i]);
            if (channel.Any(c => c.High - bars[currentBar - count].High > bars[currentBar - count].Range() * deviation)) return false;
            if (channel.Any(c => c.Low - bars[currentBar - count].Low < bars[currentBar - count].Range() * -deviation)) return false;
            return true;
        }
    }
}
