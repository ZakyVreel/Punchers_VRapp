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
    /// <summary>
    /// Classe abstract pour les kinect streams
    /// </summary>
    public abstract class KinectStream : ObservableObject
    {
        protected KinectSensor Sensor { get; set; } // Propriété représentant le capteur Kinect associé à ce flux

        protected KinectManager Manager { get; set; } // Propriété représentant le gestionnaire KinectManager associé à ce flux

        public abstract ImageSource ImageSource { get; } // Propriété abstraite pour obtenir la source d'image du flux, à implémenter dans les images streams

        public KinectStream(KinectManager manager)
        {
            Sensor = manager.KinectSensor;
            Manager = manager;
        }

        virtual public void Start() // Méthode virtuelle pour démarrer le flux Kinect
        {
            Manager.StartSensor();
        }
        virtual public void Stop() // Méthode virtuelle pour arrêter le flux Kinect
        {
            Manager.StopSensor();
        }
    }
}
