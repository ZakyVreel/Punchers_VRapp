using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_TP
{
    public class KinectManager
    {
        public string? StatusText;
        public bool Status;

        public KinectSensor kinectSensor = KinectSensor.GetDefault();

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

        }
    }
}
