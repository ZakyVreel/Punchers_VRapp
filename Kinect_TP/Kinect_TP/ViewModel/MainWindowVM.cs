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
        public ICommand ColorImageStreamCommand {  get; set; }
        public ICommand BodyImageStreamCommand { get; set; }
        public ICommand IRImageStreamCommand { get; set; }
        public ICommand DepthImageStreamCommand { get; set; }
        public KinectManager KinectManager { get; set; }

        /// <summary>
        /// Le Kinect streams factory pour la creation des streams.
        /// </summary>
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

            //Factory
            KinectStreamsFactory = new KinectStreamsFactory(KinectManager);

            StartKinectCommand = new RelayCommand(Start);
            StopKinectCommand = new RelayCommand(Stop);

            ColorImageStreamCommand = new RelayCommand(ColorImageStream);
            BodyImageStreamCommand = new RelayCommand(BodyImageStream);
            IRImageStreamCommand = new RelayCommand(IRImageStream);
            DepthImageStreamCommand = new RelayCommand(DepthImageStream);
        }

        /// <summary>
        /// Méthode qui va lancer le kinect, sert à savoir si le Kinect est disponible ou non
        /// </summary>
        private void Start()
        {
            KinectManager.StartSensor();
        }

        private void Stop()
        {
            KinectManager.StopSensor();
        }

        private void ColorImageStream()
        {
            if (KinectStream != null)
            {
                KinectStream.Stop();
            }
            KinectStream = KinectStreamsFactory[KinectStreams.Color];
            KinectStream.Start();
        }

        private void BodyImageStream()
        {
            if (KinectStream != null)
            {
                KinectStream.Stop();
            }
            KinectStream = KinectStreamsFactory[KinectStreams.Body];
            KinectStream.Start();
        }

        private void IRImageStream()
        {
            if (KinectStream != null)
            {
                KinectStream.Stop();
            }
            KinectStream = KinectStreamsFactory[KinectStreams.IR];
            KinectStream.Start();
        }

        private void DepthImageStream()
        {
            if (KinectStream != null)
            {
                KinectStream.Stop();
            }
            KinectStream = KinectStreamsFactory[KinectStreams.Depth];
            KinectStream.Start();
        }

    }
}
