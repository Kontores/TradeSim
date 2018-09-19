using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.DataModel
{
    /// <summary>
    /// Specification class for Trade object
    /// </summary>
    public class TradeSpec
    {
        public TradeType TradeType { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }
    }
}
