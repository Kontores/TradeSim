using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using TradingSimulator.DataModel;

namespace TradingSimulator.GUI
{
    /// <summary>
    /// Class that represents chart with functions of
    /// scaling data by Y-Axis and drawing the lines of
    /// entry level, stop-loss and take profit orders
    /// </summary>
    public class AdvancedChart
    {
        private Chart _chart;
        public StripLine EntryLine { get; set; }
        public StripLine StopLine { get; set; }
        public StripLine LimitLine { get; set; }
        public AdvancedChart(Chart newChart)
        {
            _chart = newChart;
            SetChartType(SeriesChartType.Candlestick);

            EntryLine = new StripLine();
            StopLine = new StripLine();
            LimitLine = new StripLine();

            SetHorLine(EntryLine, Color.Lime);
            SetHorLine(StopLine, Color.Red);
            SetHorLine(LimitLine, Color.Aqua);
        }    
        public void SetChartType(SeriesChartType type) => _chart.Series[0].ChartType = type;

        /// <summary>
        /// Drawing the candle chart
        /// </summary>
        /// <param name="bars">bars to draw</param>
        public void Draw(Bar[] bars)
        {
            _chart.Series[0].Points.Clear();

            foreach (Bar bar in bars)
            {
                _chart.Series[0].Points
                    .Add(new double[] { (double)bar.High, (double)bar.Low, (double)bar.Open, (double)bar.Close })
                    .Color = bar.Close >= bar.Open ? Color.Lime : Color.Red;                                         
            }

            foreach (DataPoint point in _chart.Series[0].Points)
            {
                point.BorderWidth = 1;
                point.BorderColor = point.Color == Color.Lime ? Color.LimeGreen : Color.DarkRed;
                point.BackSecondaryColor = point.Color == Color.Lime ? Color.Lime : Color.Red;               
            }

            SetYScale(bars);
        }      
        private void SetYScale(Bar[] bars)
        {
          
            _chart.ChartAreas[0].AxisY.Maximum = (double)bars.Max(c => c.High);
            _chart.ChartAreas[0].AxisY.Minimum = (double)bars.Min(c => c.Low);
        }

        /// <summary>
        /// Adds the horizontal line with specified color to the chart.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="color"></param>
        private void SetHorLine(StripLine line, Color color)
        {
            line.BackColor = color;
            line.Interval = 0;
            line.StripWidth = 0.0001;
            line.IntervalOffset = default(double);
            _chart.ChartAreas[0].AxisY.StripLines.Add(line);
        }

        /// <summary>
        /// sets the line to the selected price level
        /// </summary>
        /// <param name="line">horizontal line</param>
        /// <param name="price">price level</param>
        public void UpdateLine(StripLine line, decimal price) => line.IntervalOffset = (double)price;

        /// <summary>
        /// Removing lines from visible area of the chart (usualy when position is closed)
        /// </summary>
        public void RemoveLines()
        {
            foreach (var line in _chart.ChartAreas[0].AxisY.StripLines) line.IntervalOffset = default(double);
        }

    }
}
