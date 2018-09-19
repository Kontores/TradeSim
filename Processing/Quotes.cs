using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSimulator.DataModel;


namespace TradingSimulator.Processing
{
    /// <summary>
    /// Storage for price data
    /// </summary>
    public class Quotes
    {
        public Bar[] Bars { get; set; }
        public Queue<Bar> VisibleBars { get; set; }
        public int CountOfVisibleBars { get; set; } = 32;
        public int IndexOfCurrentBar { get; set; } 

        public Quotes()
        {
            IndexOfCurrentBar = CountOfVisibleBars - 1;
        }     
        public void Load(Bar[] newBars)
        {
            if (newBars == null) return;          
            Bars = newBars;
            IndexOfCurrentBar = CountOfVisibleBars - 1;
            Rescale(CountOfVisibleBars);
        }

        /// <summary>
        /// Move to next bar in quotes
        /// </summary>
        public void MoveToNextBar()
        {
            if (IndexOfCurrentBar >= Bars.Length - 1) return;
            VisibleBars.Dequeue();
            VisibleBars.Enqueue(Bars[++IndexOfCurrentBar]);
        }

        /// <summary>
        /// Changing the number of bars to show
        /// </summary>
        /// <param name="newCount"></param>
        public void Rescale(int newCount)
        {
            if (newCount - 1 > IndexOfCurrentBar || newCount < 16) return;

            CountOfVisibleBars = newCount;

            VisibleBars = new Queue<Bar>();

            for (var i = (IndexOfCurrentBar - (CountOfVisibleBars - 1)); i <= IndexOfCurrentBar; i++)
            {
                VisibleBars.Enqueue(Bars[i]);
            }
        }


    }
}
