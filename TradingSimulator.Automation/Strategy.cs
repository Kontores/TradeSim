using System;
using System.Collections.Generic;
using System.Text;
using TradingSimulator.DataModel;
using TradingSimulator.Processing;

namespace TradingSimulator.Automation
{
    public abstract class Strategy
    {
        public Strategy()
        {
            Perf = new Performance();
        }
        public Performance Perf { get;}
        public Position Position { get; set; }
        public abstract void Run(Bar[] bars, Action doEvents);

    }
}
