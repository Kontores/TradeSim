using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.DataModel
{
    /// <summary>
    /// Model that contains data for bar parsing from the string.
    /// Contains the chars for column, decimals, date and time separators and position indexes for
    /// price data with already set default values
    /// </summary>
    public class BarBuildModel
    {
        public char ColumnSeparator { get; set; } = ',';
        public char DateSeparator { get; set; } = '/';
        public char TimeSeparator { get; set; } = ':';
        public char DecimalSeparator { get; set; } = '.';

        public int DatePosition { get; set; } = 0;
        public int TimePosition { get; set; } = 1;
        public int OpenPosition { get; set; } = 2;
        public int HighPosition { get; set; } = 3;
        public int LowPosition { get; set; } = 4;
        public int ClosePosition { get; set; } = 5;
        public int VolumePosition { get; set; } = 6;
        
    }
}
