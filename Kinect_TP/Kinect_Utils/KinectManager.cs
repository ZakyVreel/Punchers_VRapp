using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_TP
{
    public class KinectManager : INotifyPropertyChanged
    {
        public string? StatusText;


        public bool status;

        public bool Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public KinectSensor kinectSensor = KinectSensor.GetDefault();

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartSensor()
        {
            this.kinectSensor.IsAvailableChanged += KinectSensor_IsAvailableChanged;
            this.kinectSensor.Open();
            this.Status = true;
            this.StatusText = this.kinectSensor.IsAvailable ? "RunningStatusText" : "NoSensorStatusText";

        }

        public void StopSensor()
        {
            this.kinectSensor.Close();
            this.Status = false;
            this.StatusText = this.kinectSensor.IsAvailable ? "RunningStatusText" : "NoSensorStatusText";

        }

        private void KinectSensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            this.StatusText = this.kinectSensor.IsAvailable ? "RunningStatusText" : "NoSensorStatusText";
            this.Status = this.kinectSensor.IsAvailable ;

        }
    }
}
