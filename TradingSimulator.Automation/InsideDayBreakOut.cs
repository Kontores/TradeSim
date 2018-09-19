using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class InsideDayBreakOut : Strategy
    {
        List<Position> positions;
        public override void Run(Bar[] bars, Action doEvent)
        {
            positions = new List<Position>();       

            for (var i = 2; i < bars.Length / 2; i++)        
            {
             
                 if (
                        bars[i - 1].High < bars[i - 2].High
                        && bars[i - 1].Low > bars[i - 2].Low                                                   
                    )
                {
                    if (
                        Entry.BuyAtStop(bars[i], bars[i - 1].High) != null                      
                        )
                    {
                        positions.Add(Entry.BuyAtStop(bars[i], bars[i - 1].High));
                        positions.Last().StopLoss = positions.Last().EntryPrice - bars[i - 1].Range();
                    }
                    
                    /*
                    if (
                        Entry.ShortAtStop(bars[i], bars[i - 1].Low) != null
                        )
                    {
                        positions.Add(Entry.ShortAtStop(bars[i], bars[i - 1].Low));
                        positions.Last().StopLoss = positions.Last().EntryPrice + bars[i - 1].Range();
                    }
                    */
                }
                                 
                              
                if(positions.Count > 0)
                {
                    for (var x = 0; x < positions.Count; x++)
                    {                      
                            if (positions[x].TradeType == TradeType.Long)
                            {
                               if (bars[i + 1].Open < positions[x].StopLoss)
                                 {
                                    Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, bars[i + 1].Open));
                                    positions.Remove(positions[x]);
                                 }

                                else if (bars[i + 1].Low <= positions[x].StopLoss)
                                {
                                    Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, positions[x].StopLoss));
                                    positions.Remove(positions[x]);
                                }
                             
                                else if (bars[i + 1].Close >= positions[x].EntryPrice)
                                {
                                     Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, bars[i + 1].Close));
                                     positions.Remove(positions[x]);
                                }
                                else if ((bars[i + 1].Date - positions[x].EntryDate).Days >= 3)
                                {
                                     Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, bars[i + 1].Close));
                                     positions.Remove(positions[x]);
                                }

                            }
                            else if (positions[x].TradeType == TradeType.Short)
                             {
                              if (bars[i + 1].Open > positions[x].StopLoss)
                            {
                                Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, bars[i + 1].Open));
                                positions.Remove(positions[x]);
                            }

                            else if (bars[i + 1].High >= positions[x].StopLoss)
                            {
                                Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, positions[x].StopLoss));
                                positions.Remove(positions[x]);
                            }

                            else if (bars[i + 1].Close <= positions[x].EntryPrice)
                            {
                                Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, bars[i + 1].Close));
                                positions.Remove(positions[x]);
                            }
                            else if ((bars[i + 1].Date - positions[x].EntryDate).Days >= 3)
                            {
                                Perf.Trades.Add(positions[x].Close(bars[i + 1].Date, bars[i + 1].Close));
                                positions.Remove(positions[x]);
                            }

                        }

                    }
                }

            }
        }
    }
}
