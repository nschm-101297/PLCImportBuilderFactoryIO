using PLCImportBuilderFactoryIO.Helpers;
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
    public class XMLWriterSiemensService
    {
        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructors
        public async Task WriteData(string path, ObservableCollection<PreparedDataSet> dataSets)
        {
            string dateTimeNow = DateTime.Now.ToString();
            dateTimeNow = dateTimeNow.Replace('.', '_');
            dateTimeNow = dateTimeNow.Replace(':', '_');
            dateTimeNow = dateTimeNow.Replace(' ', '_');
            string filePath = Path.Combine(path, $"IOImport_{dateTimeNow}.xml");

            string contentXMLFile = SiemensXMLHelper.GetHeaderLineXML("_IO");

            foreach (PreparedDataSet dataSet in dataSets)
            {
                contentXMLFile += SiemensXMLHelper.GetVariableAsXMLLine(dataSet);
            }
            contentXMLFile.TrimEnd();
            contentXMLFile += SiemensXMLHelper.GetFooterLineXML();

            File.WriteAllTextAsync(filePath, contentXMLFile);
        }
        #endregion

        #region Command-Methods

        #endregion

        #region Methods

        #endregion
    }
}
