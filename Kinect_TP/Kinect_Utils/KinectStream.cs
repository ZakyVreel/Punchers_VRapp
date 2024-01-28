using CommunityToolkit.Mvvm.ComponentModel;
using Kinect_TP;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kinect_Utils
{
    // Classe abstract pour les kinect streams
    public abstract class KinectStream : ObservableObject
    {
        protected KinectSensor Sensor { get; set; }

        protected KinectManager Manager { get; set; }

        public abstract ImageSource ImageSource { get; }

        public KinectStream(KinectManager manager)
        {
            Sensor = manager.kinectSensor;
            Manager = manager;
        }

        virtual public void Start()
        {
            Manager.StartSensor();
        }
        virtual public void Stop()
        {
            Manager.StopSensor();
        }
    }
}
