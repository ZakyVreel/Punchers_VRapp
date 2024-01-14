using Kinect_TP;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Utils
{
    // make it abstract for a better logic and not instantiate this class ?
    public class KinectStream
    {
        protected KinectSensor Sensor;

        protected KinectManager Manager;

        public KinectStream(KinectManager manager)
        {
            Sensor = manager.kinectSensor;
            Manager = manager;
        }

        public void Start()
        {
            Manager.StartSensor();
        }
        public void Stop()
        {
            Manager.StopSensor();
        }
    }
}
