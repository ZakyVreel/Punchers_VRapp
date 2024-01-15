using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Kinect_TP
{
    public class KinectManager : ObservableObject
    {
        public KinectSensor kinectSensor = KinectSensor.GetDefault();
        public string? statusText;
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

        public bool status;

        public bool Status
        {
            get { return status; }
            set
            {
                SetProperty(ref status, value);
            }
        }


        public void StartSensor()
        {
            this.kinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
            this.kinectSensor.Open();

        }

        public void StopSensor()
        {
            this.kinectSensor.Close();

        }

        private void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            this.StatusText = this.kinectSensor.IsAvailable ? "Running" : "Kinect Sensor Not Available";
            this.Status = this.kinectSensor.IsAvailable ;

        }
    }
}
