using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinect_Utils;

namespace Kinect_TP.ViewModel
{ 
    public class MainWindowVM
    {
        /// <summary>
        /// Propriété liée à la commande appelée au démarrage de la page principale
        /// </summary>
        public ICommand StartKinectCommand { get; set; }
        public ICommand StopKinectCommand { get; set; }
        public ICommand StartColorImageStreamCommand {  get; set; }
        public KinectManager KinectManager { get; set; }

        public KinectStreamsFactory KinectStreamsFactory { get; set; }
        public KinectStream KinectStream { get; set; }


        public MainWindowVM()
        {
            KinectManager = new KinectManager();

            KinectStreamsFactory = new KinectStreamsFactory(new KinectManager());

            KinectStream = KinectStreamsFactory[KinectStreams.Color];

            StartKinectCommand = new RelayCommand(Start);
            StopKinectCommand = new RelayCommand(Stop);
            StartColorImageStreamCommand = new RelayCommand(StartColorImageStream);
        }

        /// <summary>
        /// Méthode inicié au lancement de la main window pour savoir si le Kinect est disponible ou non
        /// </summary>
        private void Start()
        {
            KinectManager.StartSensor();
        }

        private void Stop()
        {
            KinectManager.StopSensor();
        }

        private void StartColorImageStream()
        {
            KinectStream.Start();
        }

    }
}
