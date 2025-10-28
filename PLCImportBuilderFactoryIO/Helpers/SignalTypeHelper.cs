using PLCImportBuilderFactoryIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCImportBuilderFactoryIO.Helpers
{
    public static class SignalTypeHelper
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
        public static string GetDataType(Signal signal, bool isWordUsed, string selectedControlSystem)
        {
            string dataType = String.Empty;

            if(signal.Signaltype == Signaltype.BitInput)
            {
                dataType = "Bool";
            }
            else if (signal.Signaltype == Signaltype.BitOutput)
            {
                dataType = "Bool";
            }
            else if ((signal.Signaltype == Signaltype.NumericInput && isWordUsed) ||
                     selectedControlSystem == "Beckhoff")
            {
                dataType = "Word";
            }
            else if (signal.Signaltype == Signaltype.NumericInput && !isWordUsed)
            {
                dataType = "DWord";
            }
            else if ((signal.Signaltype == Signaltype.NumericOutput && isWordUsed) ||
                     selectedControlSystem == "Beckhoff")
            {
                dataType = "Word";
            }
            else if (signal.Signaltype == Signaltype.NumericOutput && !isWordUsed)
            {
                dataType = "DWord";
            }

            return dataType;
        }
        #endregion
    }
}
