using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PLCImportBuilderFactoryIO.Models
{
    public class Signal : INotifyPropertyChanged
    {
        #region Properties
        private string _signalName;

        public string SignalName
        {
            get { return _signalName; }
            set 
            { 
                _signalName = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SignalName)));
            }
        }
        private string _key;

        public string Key
        {
            get { return _key; }
            set 
            { 
                _key = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Key)));
            }
        }
        private string _iOName;

        public string IOName
        {
            get { return _iOName; }
            set 
            { 
                _iOName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IOName)));
            }
        }
        private string _variableNameInControlsystem;

        public string VariableNameInControlsystem
        {
            get { return _variableNameInControlsystem; }
            set 
            { 
                _variableNameInControlsystem = ReplaceUmlauts(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VariableNameInControlsystem)));
            }
        }
        private string _commentInControlsystem;

        public string CommentInControlsystem
        {
            get { return _commentInControlsystem; }
            set 
            { 
                _commentInControlsystem = ReplaceUmlauts(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CommentInControlsystem)));
            }
        }

        private int _iONumber;

        public int IONumber
        {
            get { return _iONumber; }
            set 
            { 
                _iONumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IONumber)));
            }
        }
        private Signaltype _signalType;

        public Signaltype Signaltype
        {
            get { return _signalType; }
            set 
            { 
                _signalType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Signaltype)));
            }
        }

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public Signal()
        {
            SignalName = String.Empty;
            Key = String.Empty;
            IOName = String.Empty;
            VariableNameInControlsystem = String.Empty;
            CommentInControlsystem = String.Empty;
            IONumber = -1;
        }
        public Signal(string signalName)
        {
            SignalName = signalName;
            Key = String.Empty;
            IOName = String.Empty;
            VariableNameInControlsystem = String.Empty;
            CommentInControlsystem = String.Empty;
            IONumber = -1;
        }
        public Signal(string signalName, string key, string iOName, int iONumber)
        {
            SignalName = signalName;
            Key = key;
            IOName = iOName;
            VariableNameInControlsystem = String.Empty;
            CommentInControlsystem = String.Empty;
            IONumber = iONumber;
        }
        #endregion

        #region Command-Methods

        #endregion

        #region Methods
        private string ReplaceUmlauts(string rawValue)
        {
            string finishedValue = rawValue.Replace("ä", "ae").
                                            Replace("ö", "oe").
                                            Replace("ü", "ue").
                                            Replace("Ä", "Ae").
                                            Replace("Ö", "Oe").
                                            Replace("Ü", "Ue");
            return finishedValue;
        }
        #endregion
    }
}
