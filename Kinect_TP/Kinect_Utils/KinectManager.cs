using Microsoft.Kinect;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Kinect_TP
{
    /// <summary>
    /// Classe de gestion du capteur Kinect, responsable de l'initialisation, du démarrage et de l'arrêt du capteur Kinect ainsi que pour les events.
    /// </summary>
    public class KinectManager : ObservableObject
    {
        // Le capteur Kinect par défaut
        public KinectSensor kinectSensor = KinectSensor.GetDefault();

        // Le texte d'état du capteur Kinect
        public string? statusText;

        /// <summary>
        /// Propriété représentant le texte d'état du capteur Kinect. Utilisé pour la liaison de données.
        /// </summary>
        public string? StatusText
        {
            get { return statusText; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref statusText, value);
                }
            }
        }

        // Le statut du capteur Kinect
        public bool status;

        /// <summary>
        /// Propriété représentant le statut du capteur Kinect. Utilisé pour la liaison de données.
        /// </summary>
        public bool Status
        {
            get { return status; }
            set
            {
                SetProperty(ref status, value);
            }
        }

        /// <summary>
        /// Démarre le capteur Kinect en s'abonnant à l'événement IsAvailableChanged et en ouvrant le capteur.
        /// </summary>
        public void StartSensor()
        {
            this.kinectSensor.Open();
            this.kinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
        }

        /// <summary>
        /// Arrête le capteur Kinect en le fermant.
        /// </summary>
        public void StopSensor()
        {
            this.kinectSensor.IsAvailableChanged -= this.KinectSensor_IsAvailableChanged;
            this.kinectSensor.Close();
        }

        /// <summary>
        /// Gère l'événement IsAvailableChanged du capteur Kinect, mettant à jour le texte d'état et le statut en conséquence.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement.</param>
        /// <param name="e">Informations sur l'état de disponibilité du capteur Kinect.</param>
        private void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            this.StatusText = this.kinectSensor.IsAvailable ? "Running" : "Kinect Sensor Not Available";
            this.Status = this.kinectSensor.IsAvailable;
        }
    }
}
