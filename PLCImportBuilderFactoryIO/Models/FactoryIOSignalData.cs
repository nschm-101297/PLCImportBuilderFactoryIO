using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCImportBuilderFactoryIO.Models
{
    public sealed class FactoryIOSignalData
    {
        #region Properties
        public bool UseWord { get; set; }
        public int BitInputOffset { get; set; }
        public int BitOutputOffset { get; set; }
        public int NumericInputOffset { get; set; }
        public int NumericOutputOffset { get; set; }
        public int IntInputOffset { get; set; }
        public int IntOutputOffset { get; set; }
        #endregion

        #region Events

        #endregion

        #region Constructors
        public FactoryIOSignalData()
        {
            UseWord = false;
            BitInputOffset = 0;
            BitOutputOffset = 0;
            NumericInputOffset = 0;
            NumericOutputOffset = 0;
            IntInputOffset = 0;
            IntOutputOffset = 0;
        }
        public FactoryIOSignalData(bool useWord, int bitInputOffset, int bitOutputOffset, int numericInputOffset, int numericOutputOffset, int intInputOffset, int intOutputOffset)
        {
            UseWord = useWord;
            BitInputOffset = bitInputOffset;
            BitOutputOffset = bitOutputOffset;
            NumericInputOffset = numericInputOffset;
            NumericOutputOffset = numericOutputOffset;
            IntInputOffset = intInputOffset;
            IntOutputOffset = intOutputOffset;
        }
        #endregion

        #region Command-Methods

        #endregion

        #region Methods

        #endregion
    }
}
