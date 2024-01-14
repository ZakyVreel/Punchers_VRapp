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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kinect_TP.ViewModel;

namespace Kinect_TP.View
{
    public partial class MainWindow : Window
    {
        public MainWindowVM MainWindowVM { get; set; }


        //Methode qui fait partie de la classe Window, elle est appellé pour créer la fentre WPF.
        public override void BeginInit()
        {
            base.BeginInit();
            MainWindowVM.StartCommand.Execute(null);
        }

        public MainWindow()
        {
            MainWindowVM = new MainWindowVM();
            DataContext = MainWindowVM;
            InitializeComponent();
        }

    }
}
