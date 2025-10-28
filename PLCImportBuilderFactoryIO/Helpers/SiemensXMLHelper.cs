using PLCImportBuilderFactoryIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCImportBuilderFactoryIO.Helpers
{
    public static class SiemensXMLHelper
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
        public static string GetHeaderLineXML(string nameTagTable)
        {
            string headerLineXML = "<?xml version='1.0' encoding='utf-8'?>\n" +
                                   $"<Tagtable name='{nameTagTable}'>\n";

            return headerLineXML;
        }

        public static string GetVariableAsXMLLine(PreparedDataSet variable)
        {
            string variableAsXMLLine = $"  <Tag type='{variable.SingleData[2]}' hmiVisible='False' " +
                                       $"hmiWriteable='False' hmiAccessible='False' retain='False' remark='{variable.SingleData[6]}' " +
                                       $"addr='{variable.SingleData[1]}'>{variable.SingleData[0]}</Tag>\n";

            return variableAsXMLLine;
        }

        public static string GetFooterLineXML()
        {
            string footerLineXML = "</Tagtable>";

            return footerLineXML;
        }
        #endregion
    }
}
