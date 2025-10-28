using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PLCImportBuilderFactoryIO.Models
{
    public sealed class FactoryIOData
    {
        #region Properties
        public ObservableCollection<Signal> DigitalInputs{ get; set; }
        public ObservableCollection<Signal> DigitalOutputs { get; set; }
        public ObservableCollection<Signal> AnalogInputs { get; set; }
        public ObservableCollection<Signal> AnalogOutputs { get; set; }

        #endregion

        #region Events

        #endregion

        #region Constructors
        public FactoryIOData()
        {
            DigitalInputs = new ObservableCollection<Signal>();
            DigitalOutputs = new ObservableCollection<Signal>();
            AnalogInputs = new ObservableCollection<Signal>();
            AnalogOutputs = new ObservableCollection<Signal>();
        }
        #endregion

        #region Command-Methods

        #endregion

        #region Methods

        #endregion
    }
}
