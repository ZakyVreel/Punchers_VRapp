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



        public MainWindow()
        {
            MainWindowVM = new MainWindowVM();
            DataContext = MainWindowVM;
            InitializeComponent();
        }

        //Methode que sera appelé lors que la page est chargé
        private void WindowLoad(object sender, EventArgs e)
        {
            MainWindowVM.StartKinectCommand.Execute(null);
            MainWindowVM.StartColorImageStreamCommand.Execute(null);
        }

        //Methode que sera appelé lors que la page est dechargé
        private void WindowUnload(object sender, EventArgs e)
        {
            MainWindowVM.StopKinectCommand.Execute(null);
        }


    }
}
