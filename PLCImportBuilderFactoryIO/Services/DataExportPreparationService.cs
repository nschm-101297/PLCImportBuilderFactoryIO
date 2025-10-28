using PLCImportBuilderFactoryIO.Models;
using PLCImportBuilderFactoryIO.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCImportBuilderFactoryIO.Services
{
    public sealed class DataExportPreparationService
    {
        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructors

        #endregion

        #region Command-Methods
        public ObservableCollection<PreparedDataSet> PrepareData(ObservableCollection<Signal> signals, FactoryIOSignalData fileData, string selectedTargetsystem)
        {
            ObservableCollection<PreparedDataSet> preparedDataSets = new ObservableCollection<PreparedDataSet>();

            foreach (Signal signal in signals)
            {
                int signalOffset = GetOffset(signal, fileData);
                string signalAddress = CalculateAddressHelper.GetAddressOfSignal(signal, signalOffset, fileData.UseWord);
                string dataType = SignalTypeHelper.GetDataType(signal, fileData.UseWord, selectedTargetsystem);
                string wholeLine = GetWholeLine(signal, signalAddress, dataType);

                PreparedDataSet preparedData = new PreparedDataSet();
                preparedData.SetData(wholeLine, ",");

                preparedDataSets.Add(preparedData);
            }

            return preparedDataSets;
        }
        private int GetOffset(Signal signal, FactoryIOSignalData fileData)
        {
            int offset = -1;

            switch (signal.Signaltype)
            {
                case Signaltype.BitInput:
                    offset = fileData.BitInputOffset;
                    break;
                case Signaltype.BitOutput:
                    offset = fileData.BitOutputOffset;
                    break;
                case Signaltype.NumericInput:
                    offset = fileData.NumericInputOffset;
                    break;
                case Signaltype.NumericOutput:
                    offset = fileData.NumericOutputOffset;
                    break;
            }

            return offset;
        }
        private string GetWholeLine(Signal signal, string signalAddress, string dataType)
        {
            string wholeLine = String.Concat($"\"{signal.VariableNameInControlsystem}\"", ",", $"\"{signalAddress}\"", ",", $"\"{dataType}\"", ",", "\"False\"", ",",
                                             "\"False\"", ",", "\"False\"", ",", $"\"{signal.CommentInControlsystem}\"", ",", "\"\"", ",", "\"False\"");

            return wholeLine;
        }
        #endregion

        #region Methods

        #endregion
    }
}
