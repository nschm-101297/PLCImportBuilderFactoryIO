using PLCImportBuilderFactoryIO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace PLCImportBuilderFactoryIO.Services
{
    public sealed class FactoryIOFileService
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
        public async Task<FactoryIOData> LoadAllSignals(string pathFactoryIOFile)
        {
            XDocument doc = await GetXMLFactoryIOFile(pathFactoryIOFile);

            if (doc == null)
            {
                MessageBox.Show("Fehler beim Dokument laden");
                return null;
            }

            FactoryIOData allSignals = new FactoryIOData
            {
                DigitalInputs = ParseSignals(doc, "BinaryInput", "BitInput", Signaltype.BitInput),
                DigitalOutputs = ParseSignals(doc, "BinaryOutput", "BitOutput", Signaltype.BitOutput),
                AnalogInputs = ParseSignals(doc, "IntInput", "NumericInput", Signaltype.NumericInput, "AnalogueInput"),
                AnalogOutputs = ParseSignals(doc, "IntOutput", "NumericOutput", Signaltype.NumericOutput, "AnalogueOutput")
            };

            return allSignals;

        }
        public async Task<FactoryIOSignalData> GetAllSignalInformation(string pathFactoryIOFile, string selectedConnectionMode)
        {
            XDocument doc = await GetXMLFactoryIOFile(pathFactoryIOFile);

            if (doc == null)
            {
                MessageBox.Show("Fehler beim Dokument laden");
                return null;
            }

            string signalElement = GetConnectionElement(selectedConnectionMode);
            IEnumerable<XElement> readPropertyData = doc.Descendants(signalElement);
            if(readPropertyData == null)
            {
                return new FactoryIOSignalData();
            }
            FactoryIOSignalData iOSignalData = new FactoryIOSignalData
            {
                UseWord = IsWordUsed(readPropertyData.First(), selectedConnectionMode),
                BitInputOffset = GetOffset(readPropertyData.First(), "BitInputOffset"),
                BitOutputOffset = GetOffset(readPropertyData.First(), "BitOutputOffset"),
                NumericInputOffset = GetOffset(readPropertyData.First(), "NumericInputOffset"),
                NumericOutputOffset = GetOffset(readPropertyData.First(), "NumericOutputOffset"),
                IntInputOffset = GetOffset(readPropertyData.First(), "IntInputOffset"),
                IntOutputOffset = GetOffset(readPropertyData.First(), "IntOutputOffset")
            };

            return iOSignalData;
        }
        private async Task<XDocument> GetXMLFactoryIOFile(string path)
        {
            await using var fs = new FileStream(
                                                path,
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.Read,
                                                bufferSize: 4096,
                                                options: FileOptions.Asynchronous);

            var settings = new XmlReaderSettings { Async = true };
            using var reader = XmlReader.Create(fs, settings);
            var doc = await XDocument.LoadAsync(reader, LoadOptions.None, CancellationToken.None)
                                     .ConfigureAwait(false);

            return doc;
        }
        private static ObservableCollection<Signal> ParseSignals(XDocument factoryIOFile, string signalType, string kindOfSignal, Signaltype signaltype, string? secondSignalType = null)
        {
            ObservableCollection<Signal> allSignals = new ObservableCollection<Signal>();

            IEnumerable<XElement> allUsedSignals = null;
            IEnumerable<XElement> allUsedIOSignals = factoryIOFile.Descendants().
                                                                   Where(c => c.Name.LocalName.StartsWith(kindOfSignal)).
                                                                   ToList(); ;

            if(secondSignalType == null)
            {
                allUsedSignals = factoryIOFile.Descendants().
                                   Where(c => c.Name.LocalName.StartsWith(signalType)).
                                   ToList();
            }
            else if(secondSignalType != null)
            {
                allUsedSignals = factoryIOFile.Descendants().
                                   Where(c => c.Name.LocalName.StartsWith(signalType) ||
                                              c.Name.LocalName.StartsWith(secondSignalType)).
                                   ToList();
            }

            if (allUsedSignals == null ||
               allUsedIOSignals == null ||
               allUsedSignals.Count() == 0 ||
               allUsedIOSignals.Count() == 0
                )
            {
                Signal readError = new Signal("Kein Eingang gefunden");
                allSignals.Add(readError);
                return allSignals;
            }

            foreach (XElement element in allUsedSignals)
            {
                string keyValueUsedSignal = element.Attribute("Key")?.Value ?? "";
                if (keyValueUsedSignal == null || keyValueUsedSignal == "")
                {
                    continue;
                }

                XElement foundedIOSignal = allUsedIOSignals.Where(c => c.Attribute("PointIOKey").Value == keyValueUsedSignal).
                                                                                          FirstOrDefault();
                
                if (foundedIOSignal == null)
                {
                    continue;
                }
                Signal signal = new Signal
                {
                    SignalName = element.Attribute("Name")?.Value ?? string.Empty,
                    Key = keyValueUsedSignal,
                    IOName = foundedIOSignal.Name.LocalName,
                    IONumber = GetIONumber(foundedIOSignal.Name.LocalName),
                    Signaltype = signaltype
                };

                allSignals.Add(signal);
            }

            return allSignals;

        }
        private static int GetIONumber(string ioNameRawValue)
        {
            string extractedIONumber = new string(ioNameRawValue.Where(Char.IsDigit).ToArray());
            int convertedIONumber = -1;
            int.TryParse(extractedIONumber, out convertedIONumber);

            return convertedIONumber;
        }
        private static string GetConnectionElement(string selectedConnectionMode)
        {
            string nameCollectionElement = string.Empty;

            switch (selectedConnectionMode)
            {
                case "Siemens S7-200/300/400":
                    nameCollectionElement = "SiemensS7300S7400TCP";
                    break;
                case "Siemens S7-1200/1500":
                    nameCollectionElement = "SiemensS71200S71500TCP";
                    break;
                case "Siemens S7-PLCSIM":
                    nameCollectionElement = "SiemensS7PLCSIM";
                    break;
                case "Modbus TCP/IP Client":
                    nameCollectionElement = "ModbusTCPClient";
                    break;
                case "Modbus TCP/IP Server":
                    nameCollectionElement = "ModbusTCPServer";
                    break;
                case "OPC Client DA/UA":
                    nameCollectionElement = "OPCClientDA";
                    break;
            }
            
            return nameCollectionElement;
        }
        private static bool IsWordUsed(XElement connectionElement, string selectedConnectionMode)
        {
            if (!selectedConnectionMode.Contains("Siemens"))
            {
                return false;
            }
            bool? isWordUsed = null;
            string namePropertyElement = string.Empty;

            switch (selectedConnectionMode)
            {
                case "Siemens S7-200/300/400":
                    namePropertyElement = "S7Properties";
                    break;
                case "Siemens S7-1200/1500":
                    namePropertyElement = "S7Properties";
                    break;
                case "Siemens S7-PLCSIM":
                    namePropertyElement = "Properties";
                    break;
            }

            IEnumerable<XElement> propertiesFactoryIOFile = connectionElement.Descendants(namePropertyElement);
            if(propertiesFactoryIOFile == null)
            {
                return false;
            }

            string readValue = propertiesFactoryIOFile.First()?.Attribute("UseWords")?.Value ?? "False";

            return Convert.ToBoolean(readValue);
        }
        private static int GetOffset(XElement connectionElement, string nameOffsetOne)
        {
            IEnumerable<XElement> offsetElement = connectionElement.Descendants("PointIOOffset");

            if(offsetElement == null)
            {
                return -1;
            }

            string readValueOffsetOne = offsetElement.First()?.Attribute(nameOffsetOne)?.Value ?? "-1";

            return Convert.ToInt32(readValueOffsetOne);
        }
        #endregion
    }
}
