using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradingSimulator;
using TradingSimulator.Processing;
using TradingSimulator.DataModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace TradingSimulator.GUI
{
    public class GUIAdapter
    {
        private MainForm _form;
        private TradeHandler _handler;
        private AdvancedChart _chart;
        private Scrolling _scrolling;
        private string _message;
        public GUIAdapter(MainForm form)
        {
            _form = form;          
            SetTexBoxes(this, null);
            _handler = new TradeHandler();
            _chart = new AdvancedChart(_form.stockChart);
            Initialize();
            _scrolling = new Scrolling(ButtonNext_onClick);
            _message = String.Empty;       
        }

        private void Initialize()
        {
            SetButtonFuncs();
            SetCheckBoxes();
            ControlsToStartPosition(true);
            ReadQuotes();                     
        }
      
        private void SetCheckBoxes()
        {
            _form.checkBoxMkt.CheckedChanged += CheckBoxMarket_CheckedChanged;
            _form.checkBoxSL.CheckedChanged += CheckBoxStopLoss_CheckedChanged;
            _form.checkBoxTP.CheckedChanged += CheckBoxTakeProfit_CheckedChanged;        
            _form.listQuotes.SelectedValueChanged += ListQuotes_onChange;
            _form.checkBoxAScrl.CheckedChanged += CheckBoxAutoScroll_CheckhedChanged;
        }

        private void SetButtonFuncs()
        {
            _form.buttonBuy.Click += ButtonBuy_onClick;
            _form.buttonSell.Click += ButtonSell_onClick;
            _form.buttonNext.Click += ButtonNext_onClick;
            _form.buttonStart.Click += ButtonStart_onClick;
            _form.buttonFinish.Click += ButtonFinish_onClick;
            _form.buttonZIn.Click += ButtonZoomIn_onClick;
            _form.buttonZOut.Click += ButtonZoomOut_onClick;
            _form.buttonSpDown.Click += Button_SpeedDown_onClick;
            _form.buttonSpUp.Click += Button_SpeedUp_onClick;
        }

        private void ButtonStrRun_onClick(object sender, EventArgs e)
        {
            InitializeStats();            
            ShowStatistics();
        }

        private void SetTexBoxes(object sender, EventArgs e)
        {                     
            _form.txEntryPrice.KeyPress += DigitalInputOnly ;
            _form.txStopLoss.KeyPress += DigitalInputOnly ;
            _form.txTakeProfit.KeyPress += DigitalInputOnly;        
        }

        private void CheckBoxAutoScroll_CheckhedChanged(object sender, EventArgs e)
        {
            var chBox = sender as CheckBox;
            _scrolling.Timer.Enabled = chBox.Checked;
        }
     
        private void CheckBoxMarket_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            _form.txEntryPrice.Enabled = !checkBox.Checked;
            if (!_form.txEntryPrice.Enabled) _form.txEntryPrice.Text = string.Empty;
        }

        private void CheckBoxStopLoss_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            _form.txStopLoss.Enabled = checkBox.Checked;
            if (!_form.txStopLoss.Enabled) _form.txStopLoss.Text = string.Empty;
        }

        private void CheckBoxTakeProfit_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            _form.txTakeProfit.Enabled = checkBox.Checked;
            if (!_form.txTakeProfit.Enabled) _form.txTakeProfit.Text = string.Empty;
        }

        /// <summary>
        /// Enabling and disabling the controls on the form
        /// depending of current program state
        /// </summary>
        /// <param name="isTrue">simulation is started or not</param>
        private void ControlsToStartPosition(bool isTrue)
        {
            var startControls = new Control[]
            {
               _form.buttonStart,
               _form.listQuotes
            };

           foreach (var ctrl in startControls) ctrl.Enabled = isTrue;
                 
           var gamingControls = new Control[] 
           {
             _form.buttonBuy,
             _form.buttonSell,
             _form.buttonNext,
             _form.buttonFinish,
             _form.buttonSpDown,
             _form.buttonSpUp,       
             _form.checkBoxAScrl   
           };

           foreach (var ctrl in gamingControls) ctrl.Enabled = !isTrue;

            _form.checkBoxAScrl.Checked = false;         
        }

        private void ReadQuotes()
        {
            foreach (var filePath in FileReader.ReadFileList())
            {
                _form.listQuotes.Items.Add(filePath);
            }
        }
      
        private void ButtonBuy_onClick(object sender, EventArgs e)
        {
            Update();          
            _handler.EnterOrClose(TradeType.Long);
            if (_handler.Position.Active)
            {
                _message = $"Buy  at {_handler.Position.EntryPrice}";
            }
            else
            {
                _message = $"Cover at {_handler.Performance.Trades.Last().ExitPrice}";
            }
            _form.lblChartTitle.Text = _message;
            Update();                
        }

        private void ButtonSell_onClick(object sender, EventArgs e)
        {
            Update();
            _handler.EnterOrClose(TradeType.Short);
            if (_handler.Position.Active)
            {
                _message = $"Short  at {_handler.Position.EntryPrice}";
            }
            else
            {
                _message = $"Sell at {_handler.Performance.Trades.Last().ExitPrice}";
            }
            _form.lblChartTitle.Text = _message;
            Update();
        }

        private void ButtonStart_onClick(object sender, EventArgs e)
        {
            if(_form.listQuotes.SelectedItem == null)
            {
                MessageBox.Show("Please choose Quotes before start");
                return;
            }          
            ControlsToStartPosition(false);
            _handler = new TradeHandler();
            ListQuotes_onChange(_form.listQuotes, null);
            InitializeStats();
            Update();      
        }    
        private void ButtonFinish_onClick(object sender, EventArgs e)
        {
            ControlsToStartPosition(true);
            ShowStatistics();      
        }

        private void ButtonNext_onClick(object sender, EventArgs e)
        {
            _handler.Quotes.MoveToNextBar();
            _chart.Draw(_handler.Quotes.VisibleBars.ToArray());       
            Update();
        }
        private void ButtonZoomIn_onClick(object sender, EventArgs e)
        {
            _handler.Quotes.Rescale(_handler.Quotes.CountOfVisibleBars / 2);
            _chart.Draw(_handler.Quotes.VisibleBars.ToArray());
        }


        private void ButtonZoomOut_onClick(object sender, EventArgs e)
        {
            _handler.Quotes.Rescale(_handler.Quotes.CountOfVisibleBars * 2);
            _chart.Draw(_handler.Quotes.VisibleBars.ToArray());
        }

        private void Button_SpeedUp_onClick(object sender, EventArgs e)
        {
            _scrolling.IncreaseSpeed();
            SetSpeedLabel(_form.labelSpeedVal);        
        }
     
        private void Button_SpeedDown_onClick(object sender, EventArgs e)
        {
            _scrolling.DecreaseSpeed();
            SetSpeedLabel(_form.labelSpeedVal);
        }

        private void SetSpeedLabel(Label label)
        {
            switch (_scrolling.Timer.Interval)
            {             
                case (int)IntervalLimit.Maximum:
                    label.Text = "Maximal";
                    break;
                case (int)IntervalLimit.Slow:
                    label.Text = "Slow";
                    break;
                case (int)IntervalLimit.Medium:
                    label.Text = "Medium";
                    break;
                case (int)IntervalLimit.Fast:
                    label.Text = "Fast";
                    break;
                case (int)IntervalLimit.Minimal:
                    label.Text = "Minimal";
                    break;
            }
        }

        private void ListQuotes_onChange(object sender, EventArgs e)
        {
            var src = (ComboBox)sender;
            if (FileReader.Read(src.SelectedItem.ToString()) == null)
            {
                MessageBox.Show(FileReader.ReaderExMessage);
                return;
            }   
                             
            _handler.Quotes.Load(FileReader.Read(src.SelectedItem.ToString()));
            _chart.Draw(_handler.Quotes.VisibleBars.ToArray());
            ButtonStrRun_onClick(null, null);
        }
        private void Update()
        {
            UpdateHandler();
            UpdateChart();
            UpdateCurrentResults();                                                            
        }

        private void UpdateHandler()
        {
            if (_form.txEntryPrice.Text != string.Empty) _handler.UpdatePending(decimal.Parse(_form.txEntryPrice.Text));
            else _handler.PendingPrice = default(decimal);
            if (_form.txStopLoss.Text != string.Empty) _handler.UpdateStopLoss(decimal.Parse(_form.txStopLoss.Text));
            else _handler.StopLoss = default(decimal);
            if (_form.txTakeProfit.Text != string.Empty) _handler.UpdateTakeProfit(decimal.Parse(_form.txTakeProfit.Text));
            else _handler.TakeProfit = default(decimal);            
        }
        private void UpdateChart()
        {
            _chart.UpdateLine
                (_chart.EntryLine,
                    (_handler.Position == null || !_handler.Position.Active) ? default(decimal)
                    : (_handler.PendingPrice == default(decimal) ? _handler.Position.EntryPrice : _handler.PendingPrice)
                );
            _chart.UpdateLine(_chart.StopLine, _handler.StopLoss);
            _chart.UpdateLine(_chart.LimitLine, _handler.TakeProfit);
        }

        
        /// <summary>
        /// Allows to only put the digits ant separators at the textboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DigitalInputOnly(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == ',') e.Handled = false;
            else e.Handled = true;
        }       

        /// <summary>
        /// Show current result in the menu at the right-down corner of the application
        /// </summary>
        private void UpdateCurrentResults()
        {
            _form.lblProfit.Text = _handler.Performance.NetProfit().ToString();
            _form.lblPos.Text = _handler.Performance.PositionType(_handler.Position);
            _form.lblTTrades.Text = _handler.Performance.Trades.Count.ToString();
            _form.lblVarMargin.Text = _handler.Performance.VariableMargin(_handler.Position, _handler.CurrentBar.Close).ToString();
            _form.lblVarMargin.ForeColor = _handler.Performance.VariableMargin(_handler.Position, _handler.CurrentBar.Close) == 0 ? 
                System.Drawing.Color.White :
                _handler.Performance.VariableMargin(_handler.Position, _handler.CurrentBar.Close) > 0 ? 
                System.Drawing.Color.Lime : System.Drawing.Color.Red;
            _form.lblWinPrc.Text = _handler.Performance.WinPercent().ToString();
            _form.lblPF.Text = _handler.Performance.ProfitFactor().ToString();
        }

        private void ShowStatistics()
        {
            _form.lbGrossPrVal.Text = _handler.Performance.GrossProfit().ToString();
            _form.lbGrossLVal.Text = _handler.Performance.GrossLoss().ToString();
            _form.lbNetPrVal.Text = _handler.Performance.NetProfit().ToString();
            _form.lbTotalTrVal.Text = _handler.Performance.Trades.Count.ToString();
            _form.lbWinningTrVal.Text = _handler.Performance.WinningTrades().ToString();
            _form.lbLosingTrVal.Text = _handler.Performance.LosingTrades().ToString();
        
            _form.lbWinPercVal.Text = _handler.Performance.WinPercent().ToString();
            _form.lbAvgWinVal.Text = _handler.Performance.AvgWin().ToString();
            _form.lbAvgLosVal.Text = _handler.Performance.AvgLoss().ToString();
            _form.lbPFactorVal.Text = _handler.Performance.ProfitFactor().ToString();
            _form.lbPayoffVal.Text = _handler.Performance.Payoff().ToString();
            _form.lbDrawdVal.Text = _handler.Performance.DrawDown().ToString();
            _form.lbRecFVal.Text = _handler.Performance.RecoveryF().ToString();

            // drawing equity chart

            for (var i = 0; i < _handler.Performance.Equity().Length; i++)
                _form.chartEquity.Series[0].Points.Add(new System.Windows.Forms.DataVisualization.Charting.DataPoint
                    (i, (double)_handler.Performance.Equity()[i])
                { Color = _handler.Performance.Equity()[i] >= 0 ? System.Drawing.Color.Lime : System.Drawing.Color.Red });

            // drawing buy&hold line
            if(_handler.Performance.Trades.Count > 0)
                      foreach (var bar in _handler.Quotes.Bars)
                if (bar.Date >= _handler.Performance.Trades.FirstOrDefault().EntryDate && bar.Date <= _handler.Performance.Trades.LastOrDefault().ExitDate)
                    _form.chartEquity.Series[1].Points.Add(new DataPoint(_form.chartEquity.Series[1].Points.Count, (double)bar.Close));                        

            // adding trades list

            foreach (var trade in _handler.Performance.Trades) _form.TradeList.Rows.Add
                    (new object[] { trade.Type, trade.EntryDate, trade.EntryPrice, trade.ExitDate, trade.ExitPrice, trade.Profit });


            ShowSeasonals();
        }

        private void ShowSeasonals()
        {

            _form.chartNPTDW.Series[0].Points.Clear();
            foreach (var point in _handler.Performance.NetProfitByDayOfWeek())
                _form.chartNPTDW.Series[0].Points.AddXY(point.Key, point.Value);           
            foreach (var point in _form.chartNPTDW.Series[0].Points) point.Color = point.YValues[0] >= 0 ? System.Drawing.Color.Lime : System.Drawing.Color.Red;

            _form.chartNPM.Series[0].Points.Clear();
            foreach (var point in _handler.Performance.NetProfitByMonth())
                _form.chartNPM.Series[0].Points.AddXY(point.Key, point.Value);
            foreach (var point in _form.chartNPM.Series[0].Points) point.Color = point.YValues[0] >= 0 ? System.Drawing.Color.Lime : System.Drawing.Color.Red;

            _form.chartNPTDM.Series[0].Points.Clear();
            foreach (var point in _handler.Performance.NetProfitByDayOfMonth())
                _form.chartNPTDM.Series[0].Points.AddXY(point.Key, point.Value);
           
            foreach (var point in _form.chartNPTDM.Series[0].Points) point.Color = point.YValues[0] >= 0 ? System.Drawing.Color.Lime : System.Drawing.Color.Red;
        }
        private void InitializeStats()
        {
            _form.lbGrossPrVal.Text = "0";
            _form.lbGrossLVal.Text = "0";
            _form.lbNetPrVal.Text = "0";
            _form.lbTotalTrVal.Text = "0";
            _form.lbWinningTrVal.Text = "0";
            _form.lbLosingTrVal.Text = "0";
            _form.lbWinPercVal.Text = "0";
            _form.lbAvgWinVal.Text = "0";
            _form.lbAvgLosVal.Text = "0";
            _form.lbPFactorVal.Text = "0";
            _form.lbPayoffVal.Text = "0";
            _form.lbDrawdVal.Text = "0";
            _form.lbRecFVal.Text = "0";
            _form.chartEquity.Series[0].Points.Clear();
            _form.chartEquity.Series[1].Points.Clear();
            _form.TradeList.Rows.Clear();

            _form.lblPos.Text = "None";
            _form.lblProfit.Text = "0";
            _form.lblVarMargin.Text = "0";
            _form.lblWinPrc.Text = "0";
            _form.lblTTrades.Text = "0";
        }
    }
}
