using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Processing
{
    /// <summary>
    /// Main trading simulation handler class
    /// </summary>
    public class TradeHandler
    {
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public decimal PendingPrice { get; set; }
        public TradeType PendingType { get; set; }
        public Performance Performance { get; set; }
        public Quotes Quotes { get; set; }
        public Bar CurrentBar { get { return Quotes.Bars[Quotes.IndexOfCurrentBar]; }  }
        public Position Position { get; set; }       
                   
        public TradeHandler()
        {         
            Performance = new Performance();
            Quotes = new Quotes();
            StopLoss = default(decimal);
            TakeProfit = default(decimal);
            PendingPrice = default(decimal);
        }
        
        /// <summary>
        /// open new trading position by current bar close price
        /// </summary>
        /// <param name="bar">current market bar</param>
        /// <param name="type">position type</param>
        /// <returns></returns>
        private Position EntryAtMarket(Bar bar, TradeType type) => new Position(type, bar.Date, bar.Close);    

        /// <summary>
        /// Tracking the stop loss order data
        /// </summary>
        public void UpdateStopLoss(decimal value)
        {
            if (Position == null || !Position.Active) return;
            if ((Position.TradeType == TradeType.Long && value < CurrentBar.Close) ||
            (Position.TradeType == TradeType.Short && value > CurrentBar.Close))
            StopLoss = value;

            if (StopLoss != default(decimal))
            {
                if ((Position.TradeType == TradeType.Long && CurrentBar.Low <= StopLoss)
                    ||
                    (Position.TradeType == TradeType.Short && CurrentBar.High >= StopLoss))
                {
                    Performance.Trades.Add(Position.Close(CurrentBar.Date, StopLoss));
                    StopLoss = default(decimal);
                }
            }   
        }

        /// <summary>
        /// Tracking the take profit order data
        /// </summary>
        public void UpdateTakeProfit(decimal value)
        {
            if (Position == null || !Position.Active) return;
            if ((Position.TradeType == TradeType.Long && value > CurrentBar.Close) ||
            (Position.TradeType == TradeType.Short && value < CurrentBar.Close))
            TakeProfit = value;

            if (TakeProfit != default(decimal))
            {
                if ((Position.TradeType == TradeType.Long && CurrentBar.High >= TakeProfit)
                    ||
                    (Position.TradeType == TradeType.Short && CurrentBar.Low <= TakeProfit))
                {
                    Performance.Trades.Add(Position.Close(CurrentBar.Date, TakeProfit));
                    TakeProfit = default(decimal);
                }
            }
        }

        /// <summary>
        /// Tracking the pending entry order data
        /// </summary>
        public void UpdatePending(decimal value)
        {
            if (Position != null && Position.Active) return;
            PendingPrice = value;
            if (PendingPrice == default(decimal)) return;        
            if ((CurrentBar.Open < PendingPrice && CurrentBar.High >= PendingPrice) ||
                (CurrentBar.Open > PendingPrice && CurrentBar.Low <= PendingPrice))
            {
                Position = new Position(PendingType, CurrentBar.Date, PendingPrice);
                PendingPrice = default(decimal);
            }
        }

        /// <summary>
        /// Open new Position or close curent
        /// </summary>
        /// <param name="type">type of position</param>
        public void EnterOrClose(TradeType type)
        {           
            if ((Position == null || Position.Active == false))
            {
                if (PendingPrice == default(decimal))
                {
                    Position = EntryAtMarket(CurrentBar, type);                  
                }
                else
                {
                    PendingType = type;                  
                }
            }
            else if (Position.TradeType != type)
            {
                Performance.Trades.Add(Position.Close(CurrentBar.Date, CurrentBar.Close));
            }
        }               
    }
}
