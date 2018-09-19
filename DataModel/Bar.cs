using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.DataModel
{
    /// <summary>
    /// Bar represents a unit of the price structure, containing
    /// the information of price changing during pre-selected time period, and
    /// the trading volume.
    /// </summary>
    public class Bar
    {
        public DateTime Date { get; }
        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
        public decimal Volume { get; }
     
        public Bar(DateTime date, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public decimal Range() => High - Low;
        public decimal DownShadow() => Close >= Open ? Open - Low : Close - Low;
        public decimal UpperShadow() => Close >= Open ? High - Close : High - Open;
        public decimal Body() => Math.Abs(Close - Open);
    }
}
