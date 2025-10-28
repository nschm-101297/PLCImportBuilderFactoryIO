using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCImportBuilderFactoryIO.Models;

namespace PLCImportBuilderFactoryIO.Helpers
{
    public static class CalculateAddressHelper
    {
        #region Properties

        #endregion

        #region Events

        #endregion

        #region Constructors

        #endregion

        #region Command-Methods
        public static string GetAddressOfSignal(Signal toConvertingSignal, int addressOffset, bool isWordUsed)
        {
            string finishedAddressed = String.Empty;
            int bitNumber = -1;
            int byteNumber = -1;

            string kindOfSignal = GetKindOfSignal(toConvertingSignal.Signaltype, isWordUsed);

            if (kindOfSignal.Contains('W') || kindOfSignal.Contains('D'))
            {
                byteNumber = GetByteOfSignalBool(toConvertingSignal.IONumber, addressOffset);
                finishedAddressed = String.Concat("%", kindOfSignal, byteNumber.ToString());
            }
            else
            {
                bitNumber = GetBitOfSignal(toConvertingSignal.IONumber);
                byteNumber = GetByteOfSignalBool(toConvertingSignal.IONumber, addressOffset, bitNumber);
                finishedAddressed = String.Concat("%", kindOfSignal, byteNumber.ToString(), ".", bitNumber.ToString());
            }
            return finishedAddressed;
        }
        private static string GetKindOfSignal(Signaltype signaltype, bool isWordUsed)
        {
            string signalType = String.Empty;

            if (signaltype == Signaltype.BitInput)
            {
                signalType = "I";
            }
            else if (signaltype == Signaltype.BitOutput)
            {
                signalType = "Q";
            }
            else if (signaltype == Signaltype.NumericInput && isWordUsed)
            {
                signalType = "IW";
            }
            else if (signaltype == Signaltype.NumericInput && !isWordUsed)
            {
                signalType = "ID";
            }
            else if (signaltype == Signaltype.NumericOutput && isWordUsed)
            {
                signalType = "QW";
            }
            else if (signaltype == Signaltype.NumericOutput && !isWordUsed)
            {
                signalType = "QD";
            }

            return signalType;
        }
        private static int GetBitOfSignal(int signalNumber)
        {
            return signalNumber % 8;
        }
        private static int GetByteOfSignalBool(int signalNumber, int byteOffset, int? bitNumber = null)
        {
            int calculatedByteNumber = -1;
            if (bitNumber == null)
            {
                calculatedByteNumber = signalNumber * 2 + byteOffset;
            }
            else
            {
                calculatedByteNumber = ((signalNumber - (int)bitNumber) % 7) + byteOffset ;
            }

            return calculatedByteNumber;
        }
        #endregion

        #region Methods

        #endregion
    }
}
