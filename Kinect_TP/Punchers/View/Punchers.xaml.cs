using Kinect_Gesture;
using Punchers.ViewModel;
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

namespace Punchers.View
{
    /// <summary>
    /// Logique d'interaction pour Punchers.xaml
    /// </summary>
    public partial class Punchers : Window
    {
        public PunchersVM PunchersVM { get; set; }

        public Punchers()
        {
            PunchersVM = new PunchersVM();
            InitializeComponent();
            GestureManager.GestureRecognized += GestureManager_GestureReco;
            DataContext = PunchersVM;
        }

        //Methode que sera appelé lors que la page est chargé
        private void WindowLoad(object sender, EventArgs e)
        {
            PunchersVM.StartAcqueringFramesCommand.Execute(null);
        }

        //Methode que sera appelé lors que la page est fermé
        private void WindowUnload(object sender, EventArgs e)
        {
            PunchersVM.StopAcqueringFramesCommand.Execute(null);
        }

        private void GestureManager_GestureReco(object sender, GestureRecognizedEventArgs e)
        {
            switch (e.GestureName)
            {
                case "BoxePosture":
                    break;
            }
        }
    }
}
