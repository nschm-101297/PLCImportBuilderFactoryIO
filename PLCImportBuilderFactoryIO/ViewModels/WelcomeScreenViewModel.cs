using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PLCImportBuilderFactoryIO.Commands;
using PLCImportBuilderFactoryIO.Views;

namespace PLCImportBuilderFactoryIO.ViewModel
{
    public class WelcomeScreenViewModel
    {
        #region Properties
        private string _selectedTargetControl;
        private WelcomeScreen _screen;
        public ICommand OpenWorkScreen { get; set; }
        public ICommand CloseScreen { get; set; }
        #endregion

        #region Events

        #endregion

        #region Constructors
        public WelcomeScreenViewModel()
        {
            _selectedTargetControl = String.Empty;
            _screen = null;
            OpenWorkScreen = new RelayCommand(OpenWorkScreenExecute, OpenWorkScreenCanExecute);
            CloseScreen = new RelayCommand(CloseWindowExecute, CloseWindowCanExecute);
        }
        public WelcomeScreenViewModel(WelcomeScreen screen)
        {
            _selectedTargetControl = String.Empty;
            _screen = screen;
            OpenWorkScreen = new RelayCommand(OpenWorkScreenExecute, OpenWorkScreenCanExecute);
            CloseScreen = new RelayCommand(CloseWindowExecute, CloseWindowCanExecute);
        }
        #endregion

        #region Command-Methods
        public void OpenWorkScreenExecute(object parameter)
        {
            _selectedTargetControl = parameter?.ToString() ?? "";
            WorkScreen workScreen = new WorkScreen(_selectedTargetControl);
            workScreen.Show();
            _screen.Close();
        }
        public bool OpenWorkScreenCanExecute(object parameter)
        {
            return true;
        }
        public void CloseWindowExecute(object parameter)
        {
            _screen.Close();
        }
        public bool CloseWindowCanExecute(object parameter)
        {
            return true;
        }
        #endregion

        #region Methods

        #endregion
    }
}
