using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
    
namespace TradingSimulator.GUI
{
    /// <summary>
    /// Class for auto-scrolling
    /// </summary>
    public class Scrolling
    {
        public Timer Timer { get; }

        private int _defaultInterval = 256;
       
        public Scrolling(EventHandler action)
        {
            Timer = new Timer();
            Timer.Interval = _defaultInterval;
            Timer.Tick += action;
        }
        public void IncreaseSpeed()
        {
              if (Timer.Interval <= (int)IntervalLimit.Maximum) return;
               Timer.Interval /= 2;         
        }
        public void DecreaseSpeed()
        {
            if (Timer.Interval >= (int)IntervalLimit.Minimal) return;
            Timer.Interval *= 2;
        }    
    }
}
