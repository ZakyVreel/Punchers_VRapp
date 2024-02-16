using Kinect_Gesture;
using Punchers.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Punchers.View
{
    /// <summary>
    /// Logique d'interaction pour Punchers.xaml
    /// </summary>
    public partial class PunchersView : Window
    {
        public PunchersVM PunchersVM { get; set; }

        public PunchersView()
        {
            PunchersVM = new PunchersVM();
            InitializeComponent();
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


    }
}
