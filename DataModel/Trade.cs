using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.DataModel
{   
    /// <summary>
    /// Represents finished trade operation
    /// </summary>
    public class Trade

    {
        public TradeType Type { get; }
        public DateTime EntryDate { get; }
        public DateTime ExitDate { get; }
        public decimal EntryPrice { get; }
        public decimal ExitPrice { get; }
        public decimal Profit { get; }
      
        public Trade(TradeSpec spec)
        {
            Type = spec.TradeType;
            EntryDate = spec.EntryDate;
            EntryPrice = spec.EntryPrice;
            ExitDate = spec.ExitDate;
            ExitPrice = spec.ExitPrice;
            Profit = (Type == TradeType.Long) ? Math.Round(ExitPrice - EntryPrice, 4) : Math.Round(EntryPrice - ExitPrice, 4);

        }
    }
}
