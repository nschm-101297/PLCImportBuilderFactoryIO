using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCImportBuilderFactoryIO.Models;
using OfficeOpenXml;

namespace PLCImportBuilderFactoryIO.Services
{
    public sealed class ExcelWriterService
    {
        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructors

        #endregion

        #region Command-Methods

        #endregion

        #region Methods
        public async Task WriteData(string path, ObservableCollection<PreparedDataSet> dataSets)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Max Mustermann");

            string dateTimeNow = DateTime.Now.ToString();
            dateTimeNow = dateTimeNow.Replace('.', '_');
            dateTimeNow = dateTimeNow.Replace(':', '_');
            dateTimeNow = dateTimeNow.Replace(' ', '_');
            string filePath = Path.Combine(path, $"IOImport_{dateTimeNow}.xlsx");

            using (ExcelPackage package = new ExcelPackage(filePath))
            {
                int rowIndex = 2;
                ExcelWorksheet tagWorksheet = package.Workbook.Worksheets.Add("PLC Tags");

                PrepareSubtitels(tagWorksheet);
                foreach (PreparedDataSet dataSet in dataSets)
                {
                    WriteDataSetToFile(tagWorksheet, dataSet, rowIndex);
                    rowIndex++;
                }
                await package.SaveAsync();
            }
        }
        private void PrepareSubtitels(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Path";
            worksheet.Cells[1, 3].Value = "Data Type";
            worksheet.Cells[1, 4].Value = "Logical Address";
            worksheet.Cells[1, 5].Value = "Comment";
            worksheet.Cells[1, 6].Value = "Hmi Visible";
            worksheet.Cells[1, 7].Value = "Hmi Accessible";
            worksheet.Cells[1, 8].Value = "Hmi Writeable";
            worksheet.Cells[1, 9].Value = "Typeobject ID";
            worksheet.Cells[1, 10].Value = "Version ID";
        }
        private void WriteDataSetToFile(ExcelWorksheet worksheet, PreparedDataSet dataSet, int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = $"{dataSet.SingleData[0]}";
            worksheet.Cells[rowIndex, 2].Value = "IO";
            worksheet.Cells[rowIndex, 3].Value = $"{dataSet.SingleData[2]}";
            worksheet.Cells[rowIndex, 4].Value = $"{dataSet.SingleData[1]}";
            worksheet.Cells[rowIndex, 5].Value = $"{dataSet.SingleData[6]}";
            worksheet.Cells[rowIndex, 6].Value = "False";
            worksheet.Cells[rowIndex, 7].Value = "False";
            worksheet.Cells[rowIndex, 8].Value = "False";
        }
        #endregion
    }
}
