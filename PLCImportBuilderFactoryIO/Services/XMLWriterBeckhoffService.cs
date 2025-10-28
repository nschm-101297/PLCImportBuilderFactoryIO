using PLCImportBuilderFactoryIO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PLCImportBuilderFactoryIO.Services
{
    public sealed class XMLWriterBeckhoffService
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
            string destinationPath = GetDestinationPath(path);
            if (!CopyPatternFile(destinationPath))
            {
                return;
            }

            XDocument xmlImportDocument = await GetXMLFactoryIOFile(destinationPath);
            await WriteNameTagTable(xmlImportDocument);

            string contentDeclaration = String.Empty;
            foreach (PreparedDataSet dataSet in dataSets)
            {
                await CreateNewVariable(xmlImportDocument, dataSet);
                contentDeclaration += $"\t{dataSet.SingleData[0]}\t:\t{dataSet.SingleData[2].ToUpper()};\t//{dataSet.SingleData[6]}\n";
            }
            await CreateText(xmlImportDocument, contentDeclaration);
            await SaveAllChanges(xmlImportDocument, destinationPath);
        }
        private string GetDestinationPath(string pathDestinationDirectory)
        {
            string pathPatternFile = @"F:\Sicherung-D_Desktop-PC\Programmierung\C#\GVLPattern.xml";
            string nameAndEndingFile = pathPatternFile.Substring(pathPatternFile.LastIndexOf('\\') + 1);
            string destinationPath = Path.Combine(pathDestinationDirectory, nameAndEndingFile);
            return destinationPath;
        }
        private bool CopyPatternFile(string destinationPath)
        {
            string pathPatternFile = @"F:\Sicherung-D_Desktop-PC\Programmierung\C#\GVLPattern.xml";

            try
            {
                File.Copy(pathPatternFile, destinationPath, true);
                return true;
            }
            catch 
            {
                return false;
            }
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
        private static async Task WriteNameTagTable(XDocument document)
        {
            IEnumerable<XElement> contentHeaders = document.Descendants(document.Root.GetDefaultNamespace() + "globalVars");
            if (contentHeaders == null)
            {
                return;
            }

            XElement firstContentHeader = contentHeaders.First();
            if (firstContentHeader == null)
            {
                return;
            }
            firstContentHeader.Attribute("name").SetValue("Signalmapping");
        }
        private static async Task CreateNewVariable(XDocument document, PreparedDataSet dataSet)
        {
            XNamespace ns = document.Root.GetDefaultNamespace();
            XElement elementGlobalVars = document.Descendants(ns + "globalVars").
                                                  FirstOrDefault();
            if (elementGlobalVars == null)
            {
                Console.WriteLine("Auslesen von Global-Vars hat nicht geklappt!");
                return;
            }

            var newVariable = new XElement(ns + "variable",
                              new XAttribute("name", dataSet.SingleData[0]),
                              new XElement(ns + "type",
                                  new XElement(ns + "baseType",
                                      new XElement(ns + dataSet.SingleData[2].ToUpper())
                                  )
                              ),
            new XElement(ns + "documentation",
                new XElement(XNamespace.Get("http://www.w3.org/1999/xhtml") + "xhtml",
                    dataSet.SingleData[6]
                )
            )
            );

            elementGlobalVars.AddFirst(newVariable);
            Console.WriteLine("Neue Variable geschrieben!");
        }

        private static async Task CreateText(XDocument document, string content)
        {
            XNamespace xhtmlNs = "http://www.w3.org/1999/xhtml";
            XElement xhtmlElement = document.Descendants(xhtmlNs + "xhtml")
                                                         .Where(html => html.Parent.Name.LocalName == "InterfaceAsPlainText")
                                                         .FirstOrDefault();

            string readValue = xhtmlElement?.Value;

            if (readValue == null)
            {
                return;
            }

            readValue = readValue.Replace("END_VAR", content + "\nEND_VAR");
            xhtmlElement.Value = readValue;
        }
        private static async Task SaveAllChanges(XDocument document, string path)
        {
            await using var fs = new FileStream(path,
                                                FileMode.Create,
                                                FileAccess.Write,
                                                FileShare.None,
                                                4096,
                                                FileOptions.Asynchronous);

            var wSettings = new XmlWriterSettings
            {
                Async = true,
                Indent = true,
                IndentChars = "  ",
                NewLineHandling = NewLineHandling.Replace,
                NewLineChars = "\n",
                OmitXmlDeclaration = false
            };

            using var writer = XmlWriter.Create(fs, wSettings);
            //await document.SaveAsync(writer, CancellationToken.None);
            document.Save(writer);
            await writer.FlushAsync();
        }
        #endregion
    }
}
