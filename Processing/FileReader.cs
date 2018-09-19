using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TradingSimulator.DataModel;

namespace TradingSimulator.Processing
{
    /// <summary>
    /// class for reading the data from text files
    /// </summary>
    public static class FileReader
    {
        public static string ReaderExMessage { get; set; }
        public static BarBuildModel BuildingModel { get; set; } = new BarBuildModel();

        private static char _defaultDateSeparator = '.';

        private static char _defaultDecimalSeparator = ',';

        /// <summary>
        /// Reads all the files placed in the quotes directory
        /// </summary>
        public static string[] ReadFileList() => Directory.GetFiles(Properties.Settings.Default.DefaultDirectory);

        /// <summary>
        /// Creates new bar object from string input
        /// using parameters from building model property
        /// </summary>
        /// <param name="str">input string</param>
        /// <returns>created bar object</returns>
        public static Bar BuildNewBar(string str)
        {
            return new Bar
                (
                    DateTime.Parse(str.Split(BuildingModel.ColumnSeparator)[BuildingModel.DatePosition]
                    .Replace(BuildingModel.DateSeparator, _defaultDateSeparator) + " " + str.Split(BuildingModel.ColumnSeparator)[BuildingModel.TimePosition]),
                    decimal.Parse(str.Split(BuildingModel.ColumnSeparator)[BuildingModel.OpenPosition]
                    .Replace(BuildingModel.DecimalSeparator, _defaultDecimalSeparator)),
                    decimal.Parse(str.Split(BuildingModel.ColumnSeparator)[BuildingModel.HighPosition]
                    .Replace(BuildingModel.DecimalSeparator, _defaultDecimalSeparator)),
                    decimal.Parse(str.Split(BuildingModel.ColumnSeparator)[BuildingModel.LowPosition]
                    .Replace(BuildingModel.DecimalSeparator, _defaultDecimalSeparator)),
                    decimal.Parse(str.Split(BuildingModel.ColumnSeparator)[BuildingModel.ClosePosition]
                    .Replace(BuildingModel.DecimalSeparator, _defaultDecimalSeparator)),
                    decimal.Parse(str.Split(BuildingModel.ColumnSeparator)[BuildingModel.VolumePosition]
                    .Replace(BuildingModel.DecimalSeparator, _defaultDecimalSeparator))
                );
        }
        
        /// <summary>
        /// Reads the data from text files
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <returns>Bar array</returns>
        public static Bar[] Read(string path)
        {
            var index = default(int);

      /*      using (var reader = File.OpenText(path))
            {
                var line = string.Empty;
                var bars = new List<Bar>();
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        bars.Add(BuildNewBar(line));
                    }

                    catch (Exception ex)
                    {
                        ReaderExMessage = $"The File you are trying to load is incorrect\nRead the details below:\n{ex.Message}, string №: {index + 1}";
                        return null;
                    }
                }

                return bars.ToArray();

            }*/

            try
            {
                using (var reader = new StreamReader(path))
                {
                    var lines = reader.ReadToEnd().Split('\n');
                    var bars = new Bar[lines.Length];
                    for (var i = 0; i < bars.Length; i++)
                    {
                        index = i;
                        bars[i] = BuildNewBar(lines[i]);                       
                    }
                    return bars;
                }
            }

            catch (Exception ex)
            {
                ReaderExMessage = $"The File you are trying to load is incorrect\nRead the details below:\n{ex.Message}, string №: {index + 1}";
                return null;
            }
        }
    }
}
