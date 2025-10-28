using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PLCImportBuilderFactoryIO.ViewModel;

namespace PLCImportBuilderFactoryIO.Views
{
    /// <summary>
    /// Interaktionslogik für WorkScreen.xaml
    /// </summary>
    public partial class WorkScreen : Window
    {
        public WorkScreen()
        {
            InitializeComponent();
            this.DataContext = new WorkScreenViewModel();
        }
        public WorkScreen(string selectedTargetSystem)
        {
            InitializeComponent();
            this.DataContext = new WorkScreenViewModel(selectedTargetSystem);
        }
    }
}
