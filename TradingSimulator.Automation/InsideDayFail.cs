using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Automation
{
    public class InsideDayFail : Strategy
    {
        List<Bar> _yesterday;
        List<Bar> _dbYesterday;
        bool triggerLong;
        bool triggerShort;
        public override void Run(Bar[] bars, Action doEvent)
        {
           

            for (var i = 600; i < bars.Length; i++)
            {
                if (Position == null)
                {
                    if(bars[i - 1].Date.DayOfYear != bars[i].Date.DayOfYear)
                    {
                        triggerLong = default(bool);
                        triggerShort = default(bool);
                        if (bars[i].Date.DayOfWeek != DayOfWeek.Monday)
                        {
                            _yesterday = bars.Where(c => c.Date.DayOfYear == bars[i].Date.DayOfYear - 1 && c.Date.Year == bars[i].Date.Year).ToList();
                            if (_yesterday.Count > 0)
                                _dbYesterday = bars.Where(c => c.Date.DayOfYear == _yesterday.First().Date.DayOfYear - 1 && c.Date.Year == _yesterday.First().Date.Year).ToList();
                            else
                            {
                                _yesterday = null;
                                _dbYesterday = null;
                            }
                        }
                        else
                        {
                            _yesterday = bars.Where(c => c.Date.DayOfYear == bars[i].Date.DayOfYear - 3 && c.Date.Year == bars[i].Date.Year).ToList();
                            if (_yesterday.Count > 0)
                                _dbYesterday = bars.Where(c => c.Date.DayOfYear == _yesterday.First().Date.DayOfYear - 1 && c.Date.Year == _yesterday.First().Date.Year).ToList();
                            else
                            {
                                _yesterday = null;
                                _dbYesterday = null;
                            }
                        }
                    }

                    if(_yesterday !=null && _yesterday.Count > 0 && _dbYesterday != null && _dbYesterday.Count > 0)
                    {
                       //  if (_yesterday.Max(c => c.High) < _dbYesterday.Max(c => c.High) && _yesterday.Min(c => c.Low) > _dbYesterday.Min(c => c.Low))
                        if(_yesterday.Max(c => c.High) - _yesterday.Min(c => c.Low) < _dbYesterday.Max(c => c.High) - _dbYesterday.Min(c => c.Low))
                        {

                            if (bars[i].High >= _yesterday.Max(c => c.High))
                                triggerShort = true;
                            if (bars[i].Low <= _yesterday.Min(c => c.Low) && !triggerShort)
                                triggerLong = true;


                              if(triggerLong)
                              {
                                  var entryPrice = (_yesterday.Last().Close > _yesterday.First().Open) ? _yesterday.First().Open : _yesterday.Last().Close;
                                  Position = Entry.BuyAtStop(bars[i], entryPrice);
                              }                                                    
                            
                        }
                            
                    }
                            
                }
                else
                {
                    triggerLong = default(bool);
                    triggerShort = default(bool);

                    if (bars[i].Date.Day != bars[i - 1].Date.Day)
                    {
                        Perf.Trades.Add(Exit.CloseAtClose(Position, bars[i - 1]));
                        Position = null;
                        _yesterday = null;
                        _dbYesterday = null;
                    }
                }
            }
            

        }
         

    }
}
