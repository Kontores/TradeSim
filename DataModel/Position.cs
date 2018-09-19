using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSimulator.DataModel
{
    /// <summary>
    /// Position represents current state of the trader - out of market (null or non-active), 
    /// buy-side (long) or sell-side (short)
    /// </summary>
    public class Position
    {
        public DateTime EntryDate { get;}
        public decimal EntryPrice { get;}    
        public TradeType TradeType { get;}
        public bool Active { get; set; }
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }

        public Position(TradeType tradeType, DateTime entryDate, decimal entryPrice)
        {
           TradeType = tradeType;
           EntryDate = entryDate;
           EntryPrice = entryPrice;
           Active = true;
        }

        /// <summary>
        /// Closing Position - creates finished trade
        /// </summary>
        /// <param name="exitDate">date when trade is finished</param>
        /// <param name="exitPrice">trade  closing price</param>
        /// <returns>trade object</returns>
        public Trade Close(DateTime exitDate, decimal exitPrice)
        {           
            Active = false;
            return new Trade(new TradeSpec
                {
                    TradeType = TradeType,
                    EntryDate = EntryDate,
                    EntryPrice = EntryPrice,
                    ExitDate = exitDate,
                    ExitPrice = exitPrice
                }
            );
        }
    }

}
