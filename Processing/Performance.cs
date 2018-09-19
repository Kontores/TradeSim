using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;

namespace TradingSimulator.Processing
{
    /// <summary>
    /// Performance calculates data for current and
    /// complete statistics of user trading results
    /// </summary>
    public class Performance       
    {
        public List<Trade> Trades { get; set; }

        public Performance()
        {
            Trades = new List<Trade>();
        }

        /// <summary>
        /// Gross Profit
        /// </summary>
        /// <returns>sum of all winning trades</returns>
        public decimal GrossProfit()
        {
            if (!Trades.Any()) return default(decimal);
            var gProfit = from trade in Trades
                          where trade.Profit > 0
                          select trade.Profit;
            return gProfit.Sum();
        }

        /// <summary>
        /// Gross Loss
        /// </summary>
        /// <returns>Sum of all losing trades</returns>
        public decimal GrossLoss()
        {
            if (!Trades.Any()) return default(decimal);
            var gLoss = from trade in Trades
                        where trade.Profit < 0
                        select trade.Profit;
            return gLoss.Sum();
        }

        /// <summary>
        /// Net Profit
        /// </summary>
        /// <returns>gross profit minus gross loss</returns>
        public decimal NetProfit() => GrossProfit() + GrossLoss();

        public int WinningTrades() => (Trades.Any()) ? Trades.Where(c => c.Profit > 0).Count() : 0;

        public int LosingTrades() => Trades.Count - WinningTrades();      
        public decimal WinPercent() => Trades.Any() ?  Math.Round(((decimal)WinningTrades() / (decimal)Trades.Count) * 100, 2) : 0;

        /// <summary>
        /// Profit factor
        /// </summary>
        /// <returns></returns>
        public decimal ProfitFactor() => (GrossLoss() != 0) ? decimal.Round(GrossProfit() / GrossLoss() * (-1), 4) : Trades.Any() ? 100 : 0;

        /// <summary>
        /// Average winning trade
        /// </summary>
        /// <returns></returns>
        public decimal AvgWin() => (WinningTrades() > 0) ? decimal.Round(GrossProfit() / WinningTrades(), 4) : 0;

        /// <summary>
        /// Average losing trade
        /// </summary>
        /// <returns></returns>
        public decimal AvgLoss() => (LosingTrades() > 0) ? decimal.Round(GrossLoss() / LosingTrades(), 4) : 0;

        /// <summary>
        /// Payoff Ratio
        /// </summary>
        /// <returns>average winning trade divided by average losing</returns>
        public decimal Payoff() => AvgLoss() != 0 ? decimal.Round(AvgWin() / -AvgLoss(), 4) : 0;

        /// <summary>
        /// Equity change
        /// </summary>
        /// <returns>Sum of trade results beginning from zero point </returns>
        public decimal[] Equity()
        {
            var equity = new decimal[Trades.Count + 1];
            for (var i = 0; i < equity.Length; i++) equity[i] = (i == 0) ? 0 : (equity[i - 1] + Trades[i - 1].Profit);
            return equity;
        }
        
        /// <summary>
        /// Drawdown
        /// </summary>
        /// <returns>biggest value of equity moving down</returns>
        public decimal DrawDown()
        {           
            var max = default(decimal);
            var drawDown = default(decimal);
            for(var i = 0; i < Equity().Length; i++)
            {
                if (Equity()[i] > max) max = Equity()[i];
                if (Equity()[i] - max < drawDown ) drawDown = Equity()[i] - max;
            }
            return drawDown;
        }

        /// <summary>
        /// Recovery factor
        /// </summary>
        /// <returns>Net profit divided by drawdown or 100 if drawdown is zero</returns>
        public decimal RecoveryF() => DrawDown() != 0 ? decimal.Round(NetProfit() / -DrawDown()) : 100;

        /// <summary>
        /// string value for current position
        /// </summary>
        /// <param name="pos">position</param>
        /// <returns>string value for gui</returns>
        public string PositionType(Position pos) => 
            (pos == null || !pos.Active) ? "None" : 
            (pos.TradeType == TradeType.Long) ? "Long" : "Short";

        /// <summary>
        /// Variable margin
        /// </summary>
        /// <param name="pos">position</param>
        /// <param name="currentPrice">current price</param>
        /// <returns>difference betwen position open price and current market price</returns>
        public decimal VariableMargin(Position pos, decimal currentPrice) =>
            (pos == null || !pos.Active) ? default(decimal) :
            (pos.TradeType == TradeType.Long) ? decimal.Round(currentPrice - pos.EntryPrice, 4) : 
            decimal.Round(pos.EntryPrice - currentPrice, 4);

        /// <summary>
        /// Dispersion of profit by the days of week
        /// </summary>
        /// <returns>collection of days of week with profit values</returns>
        public Dictionary<string, decimal> NetProfitByDayOfWeek()
        {
            return new Dictionary<string, decimal>()
            {
                { "Md", Trades.Where(c => c.EntryDate.DayOfWeek == DayOfWeek.Monday).Sum(c => c.Profit)},
                { "Tu", Trades.Where(c => c.EntryDate.DayOfWeek == DayOfWeek.Tuesday).Sum(c => c.Profit)},
                { "Wd", Trades.Where(c => c.EntryDate.DayOfWeek == DayOfWeek.Wednesday).Sum(c => c.Profit)},
                { "Th", Trades.Where(c => c.EntryDate.DayOfWeek == DayOfWeek.Thursday).Sum(c => c.Profit)},
                { "Fr", Trades.Where(c => c.EntryDate.DayOfWeek == DayOfWeek.Friday).Sum(c => c.Profit)},
                { "St", Trades.Where(c => c.EntryDate.DayOfWeek == DayOfWeek.Saturday).Sum(c => c.Profit)},
                { "Su", Trades.Where(c => c.EntryDate.DayOfWeek == DayOfWeek.Sunday).Sum(c => c.Profit)},
            };
           
        }

        public Dictionary<int, decimal> NetProfitByDayOfMonth()
        {
            var output = new Dictionary<int, decimal>();

            for(var i = 1; i < 32; i++)
               output.Add(i, Trades.Where(c => c.EntryDate.Day == i).Sum(c => c.Profit));

            return output;
        }

        public Dictionary<int, decimal> NetProfitByMonth()
        {
            var output = new Dictionary<int, decimal>();

            for (var i = 1; i < 13; i++)
                output.Add(i, Trades.Where(c => c.EntryDate.Month == i).Sum(c => c.Profit));
            return output;
        }
    }
}

