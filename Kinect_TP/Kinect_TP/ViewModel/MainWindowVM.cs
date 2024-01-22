using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinect_Utils;

namespace Kinect_TP.ViewModel
{ 
    public class MainWindowVM : ObservableObject
    {
        /// <summary>
        /// Propriété liée à la commande appelée au démarrage de la page principale
        /// </summary>
        public ICommand StartKinectCommand { get; set; }
        public ICommand StopKinectCommand { get; set; }
        public ICommand StartColorImageStreamCommand {  get; set; }
        public KinectManager KinectManager { get; set; }

        public KinectStreamsFactory KinectStreamsFactory { get; set; }

        public KinectStream kinectStream;
        public KinectStream KinectStream {
            get { return kinectStream; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref kinectStream, value);
                }
            }
        }


        public MainWindowVM()
        {
            KinectManager = new KinectManager();
            KinectManager.StartSensor();

            KinectStreamsFactory = new KinectStreamsFactory(KinectManager);

            KinectStream = KinectStreamsFactory[KinectStreams.Color];
            KinectStream.Start();

            StartKinectCommand = new RelayCommand(Start);
            StopKinectCommand = new RelayCommand(Stop);
            StartColorImageStreamCommand = new RelayCommand(StartColorImageStream);
        }

        /// <summary>
        /// Méthode inicié au lancement de la main window pour savoir si le Kinect est disponible ou non
        /// </summary>
        public void Start()
        {
            KinectManager.StartSensor();
        }

        public void Stop()
        {
            KinectManager.StopSensor();
        }

        public void StartColorImageStream()
        {
            KinectStream = KinectStreamsFactory[KinectStreams.Color];
            KinectStream.Start();
        }

    }
}
