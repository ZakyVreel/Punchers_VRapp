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
      
        public ICommand StartKinectCommand { get; private set; }
        public ICommand StopKinectCommand { get; private set; }
        public ICommand ColorImageStreamCommand {  get; private set; }
        public ICommand BodyImageStreamCommand { get; private set; }
        public ICommand IRImageStreamCommand { get; private set; }
        public ICommand DepthImageStreamCommand { get; private set; }
        public ICommand BodyAndColorImageStreamCommand { get; private set; }
        
        public KinectManager KinectManager { get; private set; }

        private bool isSecondViewboxVisible;
        public bool IsSecondViewboxVisible
        {
            get { return isSecondViewboxVisible; }
            private set
            {
                if (isSecondViewboxVisible != value)
                {
                    isSecondViewboxVisible = value;
                    OnPropertyChanged(nameof(IsSecondViewboxVisible));
                }
            }
        }


        /// <summary>
        /// Le Kinect streams factory pour la creation des streams.
        /// </summary>
        public KinectStreamsFactory KinectStreamsFactory { get; private set; }

        public KinectStream kinectStream;
        public KinectStream kinectStream2; // Pour le BodyColor Ensemble
        public KinectStream KinectStream {
            get { return kinectStream; }
            private set
            {
                if (value != null)
                {
                    SetProperty(ref kinectStream, value);
                }
            }
        }

        public KinectStream KinectStream2
        {
            get { return kinectStream2; }
            private set
            {
                if (value != null)
                {
                    SetProperty(ref kinectStream2, value);
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
            BodyAndColorImageStreamCommand = new RelayCommand(BodyAndColorImageStream);
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
            if (KinectStream2 != null)
            {
                KinectStream2.Stop();
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
            if (KinectStream2 != null)
            {
                KinectStream2.Stop();
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
            if (KinectStream2 != null)
            {
                KinectStream2.Stop();
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
            if (KinectStream2 != null)
            {
                KinectStream2.Stop();
            }
            KinectStream = KinectStreamsFactory[KinectStreams.Depth];
            KinectStream.Start();
        }

        private void BodyAndColorImageStream()
        {
            if (KinectStream != null)
            {
                KinectStream.Stop();
            }
            if (KinectStream2 != null)
            {
                KinectStream2.Stop();
            }
            KinectStream2 = KinectStreamsFactory[KinectStreams.Body];
            KinectStream2.Start();
            
            KinectStream = KinectStreamsFactory[KinectStreams.Color];
            KinectStream.Start();
        }

    }
}
