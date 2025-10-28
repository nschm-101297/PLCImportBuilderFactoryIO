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
    public  sealed class SignalMappingWriterService
    {
        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructors

        #endregion

        #region Command-Methods
        public async Task WriteSignalMapping(string path, ObservableCollection<Signal> dataSets)
        {
            string dateTimeNow = DateTime.Now.ToString();
            dateTimeNow = dateTimeNow.Replace('.', '_');
            dateTimeNow = dateTimeNow.Replace(':', '_');
            dateTimeNow = dateTimeNow.Replace(' ', '_');
            string filePath = Path.Combine(path, $"Signalmapping_{dateTimeNow}.txt");

            string codeSignalMapping = String.Empty;

            foreach (Signaltype signaltype in Enum.GetValues(typeof(Signaltype)))
            {
                IEnumerable<Signal> signalTypePreparedDataSets = dataSets.Where(dt => dt.Signaltype == signaltype).
                                                                          ToList();
                if (signalTypePreparedDataSets == null ||
                    signalTypePreparedDataSets.Count() == 0)
                {
                    continue;
                }
                signalTypePreparedDataSets = signalTypePreparedDataSets.OrderBy(dt => dt.IONumber).ToList();
                codeSignalMapping += GetSpecificSignaltypeMapping(signalTypePreparedDataSets, signaltype);
            }

            await File.WriteAllTextAsync(filePath, codeSignalMapping);
        }
        #endregion

        #region Methods
        private string GetSpecificSignaltypeMapping(IEnumerable<Signal> signals, Signaltype signaltype)
        {
            string codeSignalTypeMapping = String.Empty;
            string nameDataBuffer = GetNameSignalBuffer(signaltype);

            codeSignalTypeMapping = GetLeadingComment(signaltype);
            if (signaltype == Signaltype.BitInput ||
                signaltype == Signaltype.NumericInput)
            {
                foreach (Signal signal in signals)
                {
                    codeSignalTypeMapping += signal.VariableNameInControlsystem + "\t:=\t" +
                                             nameDataBuffer + $"[{signal.IONumber}];\n";
                }
            }
            else if(signaltype == Signaltype.BitOutput ||
                signaltype == Signaltype.NumericOutput)
            {
                foreach (Signal signal in signals)
                {
                    codeSignalTypeMapping += nameDataBuffer + $"[{signal.IONumber}]" + "\t:=\t" +
                                             signal.VariableNameInControlsystem + ";\n";
                }
            }

            codeSignalTypeMapping += "\n";

            return codeSignalTypeMapping;
        }
        private string GetLeadingComment(Signaltype commentSignaltype)
        {
            string comment = String.Empty;

            switch (commentSignaltype)
            {
                case Signaltype.BitInput:
                    comment = "=============== Digitale Eingaenge ===============\n";
                    break;
                case Signaltype.BitOutput:
                    comment = "=============== Digitale Ausgaenge ===============\n";
                    break;
                case Signaltype.NumericInput:
                    comment = "=============== Analoge Eingaenge ===============\n";
                    break;
                case Signaltype.NumericOutput:
                    comment = "=============== Analoge Ausgaenge ===============\n";
                    break;
            }

            return comment;
        }
        private string GetNameSignalBuffer(Signaltype signaltype)
        {
            string nameDataBuffer = String.Empty;

            switch (signaltype)
            {
                case Signaltype.BitInput:
                    nameDataBuffer = "GVL.mb_Output_Coils";
                    break;
                case Signaltype.BitOutput:
                    nameDataBuffer = "GVL.mb_Input_Coils";
                    break;
                case Signaltype.NumericInput:
                    nameDataBuffer = "GVL.mb_Output_Registers";
                    break;
                case Signaltype.NumericOutput:
                    nameDataBuffer = "GVL.mb_Input_Registers";
                    break;
            }

            return nameDataBuffer;
        }
        #endregion
    }
}
