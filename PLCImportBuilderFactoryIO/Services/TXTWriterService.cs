using PLCImportBuilderFactoryIO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCImportBuilderFactoryIO.Services
{
    public sealed class TXTWriterService
    {
        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructors

        #endregion

        #region Command-Methods
        public async Task WriteData(string path, ObservableCollection<PreparedDataSet> dataSets)
        {
            string dateTimeNow = DateTime.Now.ToString();
            dateTimeNow = dateTimeNow.Replace('.', '_');
            dateTimeNow = dateTimeNow.Replace(':', '_');
            dateTimeNow = dateTimeNow.Replace(' ', '_');
            string filePath = Path.Combine(path, $"IOImport_{dateTimeNow}.txt");

            string allLines = String.Empty;
            foreach (var dataSet in dataSets)
            {
                allLines += GetSignalAsLine(dataSet) + "\n";
            }

            allLines.TrimEnd();
            await File.WriteAllTextAsync(filePath, allLines);
        }
        #endregion

        #region Methods
        private string GetSignalAsLine(PreparedDataSet dataSet)
        {
            string signalAsLine = dataSet.SingleData[0] + "\t:\t" + dataSet.SingleData[2].ToUpper() + $";\t//{dataSet.SingleData[6]}";
            return signalAsLine;
        }
        #endregion
    }
}
