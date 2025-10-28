using PLCImportBuilderFactoryIO.Models;
using PLCImportBuilderFactoryIO.Commands;
using PLCImportBuilderFactoryIO.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Automation.Provider;

namespace PLCImportBuilderFactoryIO.ViewModel
{
    public class WorkScreenViewModel : INotifyPropertyChanged
    {
        #region Properties
        private OpenFolderDialog _destinationFolderExportFile;
        //private OpenFileDialog _selectedFactoryIOFile;

        private bool _signalMappingActivated;
        public bool SignalMappingActivated
        {
            get { return _signalMappingActivated; }
            set 
            { 
                _signalMappingActivated = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SignalMappingActivated)));
            }
        }

        private int _selectedExportFormat;
        public int SelectedExportFormat
        {
            get { return _selectedExportFormat; }
            set 
            { 
                _selectedExportFormat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedExportFormat)));
            }
        }
        private int _selectedModeOfConnection;

        public int SelectedModeOfConnection
        {
            get { return _selectedModeOfConnection; }
            set 
            { 
                _selectedModeOfConnection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedModeOfConnection)));
            }
        }
        private string _selectedTargetSystem;

        public string SelectedTargetSystem
        {
            get { return _selectedTargetSystem; }
            set 
            { 
                _selectedTargetSystem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTargetSystem)));
            }
        }
        private string _pathFactoryIOFile;

        public string PathFactoryIOFile
        {
            get { return _pathFactoryIOFile; }
            set 
            { 
                _pathFactoryIOFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathFactoryIOFile)));
            }
        }
        private string _pathExportFile;

        public string PathExportFile
        {
            get { return _pathExportFile; }
            set 
            { 
                _pathExportFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PathExportFile)));
            }
        }

        private Visibility _visibilitySignalMapping;

        public Visibility VisibilitySignalMapping
        {
            get { return _visibilitySignalMapping; }
            set 
            { 
                _visibilitySignalMapping = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilitySignalMapping)));
            }
        }

        private ObservableCollection<String> _exportFormats;

        public ObservableCollection<String> ExportFormats
        {
            get { return _exportFormats; }
            set { _exportFormats = value; }
        }
        private ObservableCollection<String> _modeOfConnection;

        public ObservableCollection<String> ModeOfConnection
        {
            get { return _modeOfConnection; }
            set { _modeOfConnection = value; }
        }
        private ObservableCollection<Signal> _digitalInputs;

        public ObservableCollection<Signal> DigitalInputs
        {
            get { return _digitalInputs; }
            set 
            { 
                _digitalInputs = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DigitalInputs)));
            }
        }
        private ObservableCollection<Signal> _digitalOutputs;

        public ObservableCollection<Signal> DigitalOutputs
        {
            get { return _digitalOutputs; }
            set 
            { 
                _digitalOutputs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DigitalOutputs)));
            }
        }
        private ObservableCollection<Signal> _analogInputs;

        public ObservableCollection<Signal> AnalogInputs
        {
            get { return _analogInputs; }
            set 
            { 
                _analogInputs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnalogInputs)));
            }
        }
        private ObservableCollection<Signal> _analogOutputs;

        public ObservableCollection<Signal> AnalogOutputs
        {
            get { return _analogOutputs; }
            set 
            { 
                _analogOutputs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnalogOutputs)));
            }
        }

        public ICommand OpenFileDialogFactoryIOFile { get; set; }
        public ICommand OpenFolderDialogExportFile { get; set; }
        public ICommand CreateExportFile { get; set; }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public WorkScreenViewModel()
        {
            _destinationFolderExportFile = InitializeExportFolderDialog();
            SignalMappingActivated = false;
            SelectedExportFormat = -1;
            SelectedModeOfConnection = -1;
            SelectedTargetSystem = "Kein Zielsystem ausgewählt";
            PathFactoryIOFile = String.Empty;
            PathExportFile = String.Empty;
            VisibilitySignalMapping = Visibility.Hidden;
            ExportFormats = LoadExportFormats();
            ModeOfConnection = LoadMoadOfConnections();
            DigitalInputs = new ObservableCollection<Signal>();
            DigitalOutputs = new ObservableCollection<Signal>();
            AnalogInputs = new ObservableCollection<Signal>();
            AnalogOutputs = new ObservableCollection<Signal>();
            OpenFileDialogFactoryIOFile = new AsyncRelayCommand(OpenFileDialogFactoryIOFileExecute, OpenFileDialogFactoryIOFileCanExecute);
            OpenFolderDialogExportFile = new RelayCommand(OpenFolderDialogExportFileExecute, OpenFolderDialogExportFileCanExecute);
            CreateExportFile = new AsyncRelayCommand(CreateExportFileExecute, CreateExportFileCanExecute);
            SetVisibilitySignalExchange();
        }
        public WorkScreenViewModel(string selectedTargetSystem)
        {
            _destinationFolderExportFile = InitializeExportFolderDialog();
            SignalMappingActivated = false;
            SelectedExportFormat = -1;
            SelectedModeOfConnection = -1;
            SelectedTargetSystem = selectedTargetSystem;
            PathFactoryIOFile = String.Empty;
            PathExportFile = String.Empty;
            VisibilitySignalMapping = Visibility.Hidden;
            ExportFormats = LoadExportFormats();
            ModeOfConnection = LoadMoadOfConnections();
            DigitalInputs = new ObservableCollection<Signal>();
            DigitalOutputs = new ObservableCollection<Signal>();
            AnalogInputs = new ObservableCollection<Signal>();
            AnalogOutputs = new ObservableCollection<Signal>();
            OpenFileDialogFactoryIOFile = new AsyncRelayCommand(OpenFileDialogFactoryIOFileExecute, OpenFileDialogFactoryIOFileCanExecute);
            OpenFolderDialogExportFile = new RelayCommand(OpenFolderDialogExportFileExecute, OpenFolderDialogExportFileCanExecute);
            CreateExportFile = new AsyncRelayCommand(CreateExportFileExecute, CreateExportFileCanExecute);
            SetVisibilitySignalExchange();
        }
        #endregion

        #region Command-Methods
        public async void OpenFileDialogFactoryIOFileExecute(object par)
        {
            OpenFileDialog _selectedFactoryIOFile = InitializeFactoryIOFileDialog();
            bool? dialogResult = _selectedFactoryIOFile.ShowDialog() ?? false;
            if (dialogResult == null || dialogResult == false)
            {
                return;
            }
            PathFactoryIOFile = _selectedFactoryIOFile.FileName;

            FactoryIOFileService serviceFactoryIOFile = new FactoryIOFileService();
            FactoryIOData allSignals = await serviceFactoryIOFile.LoadAllSignals(PathFactoryIOFile);
            if (allSignals == null)
            {
                return;
            }
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                DigitalInputs.Clear();
                DigitalOutputs.Clear();
                AnalogInputs.Clear();
                AnalogOutputs.Clear();

                CopyAllElements(DigitalInputs ,allSignals?.DigitalInputs);
                CopyAllElements(DigitalOutputs, allSignals?.DigitalOutputs);
                CopyAllElements(AnalogInputs, allSignals?.AnalogInputs);
                CopyAllElements(AnalogOutputs, allSignals?.AnalogOutputs);
            });
        }
        public bool OpenFileDialogFactoryIOFileCanExecute()
        {
            return true;
        }
        public void OpenFolderDialogExportFileExecute(object par)
        {
            bool? dialogResult = _destinationFolderExportFile.ShowDialog();
            if (dialogResult == null || dialogResult == false)
            {
                return;
            }
            PathExportFile = _destinationFolderExportFile.FolderName;


        }
        public bool OpenFolderDialogExportFileCanExecute(object par)
        {
            return true;
        }
        public async void CreateExportFileExecute(object par)
        {
            FactoryIOFileService serviceFactoryIOFile = new FactoryIOFileService();
            FactoryIOSignalData fileData = await serviceFactoryIOFile.GetAllSignalInformation(PathFactoryIOFile, ModeOfConnection[SelectedModeOfConnection]);

            DataExportPreparationService exportPreparationService = new DataExportPreparationService();
            ObservableCollection<PreparedDataSet> preparedDataSets = exportPreparationService.PrepareData(GetAllSignals(), fileData, SelectedTargetSystem);

            switch (ExportFormats[SelectedExportFormat])
            {
                case "XML":
                    if(SelectedTargetSystem == "Beckhoff")
                    {
                        XMLWriterBeckhoffService xMLWriter = new XMLWriterBeckhoffService();
                        await xMLWriter.WriteData(PathExportFile, preparedDataSets);
                    }
                    else if (SelectedTargetSystem == "Siemens")
                    {
                        XMLWriterSiemensService xMLWriter = new XMLWriterSiemensService();
                        await xMLWriter.WriteData(PathExportFile, preparedDataSets);
                    }
                    break;
                case "SDF":
                    SDFWriterService sdfWriterServive = new SDFWriterService();
                    await sdfWriterServive.WriteData(PathExportFile, preparedDataSets);
                    break;
                case "Xlsx (Excel-Format)":
                    ExcelWriterService excelWriter = new ExcelWriterService();
                    await excelWriter.WriteData(PathExportFile, preparedDataSets);
                    break;
                case "Txt":
                    TXTWriterService txtWriter = new TXTWriterService();
                    await txtWriter.WriteData(PathExportFile, preparedDataSets);
                    break;
            }

            if(SelectedTargetSystem == "Beckhoff" && SignalMappingActivated)
            {
                SignalMappingWriterService signalMappingWriter = new SignalMappingWriterService();
                await signalMappingWriter.WriteSignalMapping(PathExportFile, GetAllSignals());
            }
        }
        public bool CreateExportFileCanExecute()
        {
            return PathFactoryIOFile != String.Empty && PathExportFile != String.Empty &&
                   SelectedExportFormat > -1 && SelectedModeOfConnection > -1;
        }
        #endregion

        #region Methods
        private OpenFolderDialog InitializeExportFolderDialog()
        {
            OpenFolderDialog dialog = new OpenFolderDialog();
            dialog.DefaultDirectory = @"C:\";
            dialog.Multiselect = false;
            dialog.Title = "Ablageort Export Datei auswählen";
            return dialog;
        }
        private OpenFileDialog InitializeFactoryIOFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultDirectory = @"C:\";
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;
            dialog.CheckFileExists = true;
            dialog.Title = "FactoryIO-Datei auswählen";
            dialog.Filter = "FactoryIO-Dateien (*.factoryio)|*.factoryio";
            return dialog;
        }
        private ObservableCollection<String> LoadExportFormats()
        {
            ObservableCollection<String> exportFormats = new ObservableCollection<String>();

            exportFormats.Add("XML");
            if(SelectedTargetSystem == "Siemens")
            {
                exportFormats.Add("SDF");
                exportFormats.Add("Xlsx (Excel-Format)");
            }
            else if(SelectedTargetSystem == "Beckhoff")
            {
                exportFormats.Add("Txt");
            }

            return exportFormats;
        }

        private ObservableCollection<String> LoadMoadOfConnections()
        {
            ObservableCollection<String> modeOfConnections = new ObservableCollection<String>();

            if (SelectedTargetSystem == "Siemens")
            {
                modeOfConnections.Add("Siemens S7-200/300/400");
                modeOfConnections.Add("Siemens S7-1200/1500");
                modeOfConnections.Add("Siemens S7-PLCSIM");
            }
            modeOfConnections.Add("Modbus TCP/IP Client");
            modeOfConnections.Add("Modbus TCP/IP Server");
            modeOfConnections.Add("OPC Client DA/UA");

            return modeOfConnections;
        }
        private void CopyAllElements(ObservableCollection<Signal> windowList, ObservableCollection<Signal> signalsFromFile)
        {
            windowList.Clear();
            var sortedList = signalsFromFile.OrderBy(c => c.IONumber).ToList();
            foreach (var signal in sortedList)
            {
                windowList.Add(signal);
            }
        }
        private ObservableCollection<Signal> GetAllSignals()
        {
            ObservableCollection<Signal> allSignals = new ObservableCollection<Signal>();
            foreach(Signal signal in DigitalInputs)
            {
                if(signal.SignalName != "Kein Eingang gefunden")
                {
                    allSignals.Add(signal);
                }
            }

            foreach (Signal signal in DigitalOutputs)
            {
                if (signal.SignalName != "Kein Eingang gefunden")
                {
                    allSignals.Add(signal);
                }
            }

            foreach (Signal signal in AnalogInputs)
            {
                if (signal.SignalName != "Kein Eingang gefunden")
                {
                    allSignals.Add(signal);
                }
            }

            foreach (Signal signal in AnalogOutputs)
            {
                if (signal.SignalName != "Kein Eingang gefunden")
                {
                    allSignals.Add(signal);
                }
            }

            return allSignals;
        }
        private void SetVisibilitySignalExchange()
        {
            if(SelectedTargetSystem == "Beckhoff")
            {
                VisibilitySignalMapping = Visibility.Visible;
            }
            else
            {
                VisibilitySignalMapping = Visibility.Hidden;
            }
        }
            #endregion
    }
}
