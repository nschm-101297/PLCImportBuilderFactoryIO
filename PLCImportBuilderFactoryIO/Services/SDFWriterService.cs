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
    public sealed class SDFWriterService
    {
        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructors

        #endregion

        #region Command-Methods
        public async Task WriteData(string path , ObservableCollection<PreparedDataSet> dataSets)
        {
            string dateTimeNow = DateTime.Now.ToString();
            dateTimeNow = dateTimeNow.Replace('.', '_');
            dateTimeNow = dateTimeNow.Replace(':', '_');
            dateTimeNow = dateTimeNow.Replace(' ', '_');
            string filePath = Path.Combine(path, $"IOImport_{dateTimeNow}.sdf");

            IEnumerable<string> allLines = dataSets.Select(c => c.WholeDataAsLine).ToList();

            await File.AppendAllLinesAsync(filePath, allLines);
        }
        #endregion

        #region Methods

        #endregion
    }
}
