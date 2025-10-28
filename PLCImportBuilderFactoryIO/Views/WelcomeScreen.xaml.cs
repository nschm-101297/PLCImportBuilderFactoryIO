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
    /// Interaktionslogik für WelcomeScreen.xaml
    /// </summary>
    public partial class WelcomeScreen : Window
    {
        public WelcomeScreen()
        {
            InitializeComponent();
            this.DataContext = new WelcomeScreenViewModel(this);
        }
    }
}
