using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCImportBuilderFactoryIO.Models
{
    public class PreparedDataSet
    {
        #region Properties
        public string WholeDataAsLine { get; private set; }
        public ObservableCollection<string> SingleData { get; private set; }
        #endregion

        #region Events

        #endregion

        #region Constructors
        public PreparedDataSet()
        {
            WholeDataAsLine = string.Empty;
            SingleData = new ObservableCollection<string>();
        }
        #endregion

        #region Command-Methods

        #endregion

        #region Methods
        public bool SetData(string data, string elementSeparator)
        {
            if (!data.Contains(elementSeparator))
            {
                return false;
            }

            WholeDataAsLine = data;

            string[] seperatedElements = data.Split(elementSeparator);
            foreach (string seperatorElement in seperatedElements)
            {
                string rawValue = seperatorElement.Replace("\"", "");
                SingleData.Add(rawValue);
            }
            return true;
        }
        #endregion
    }
}
