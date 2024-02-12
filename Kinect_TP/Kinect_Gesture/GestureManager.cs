using Kinect_TP;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    public static class GestureManager
    {

        private static bool isAcquiringFrame;
        public static KinectManager KinectManager { get; set; }

        public static List<BaseGesture> KnownGestures { get; private set; } = new List<BaseGesture>();

        private static event EventHandler<GestureRecognizedEventArgs> gestureRecognized;
        public static event EventHandler<GestureRecognizedEventArgs> GestureRecognized
        {
            add
            {
                if (gestureRecognized == null || !gestureRecognized.GetInvocationList().Contains(value))
                {
                    gestureRecognized += value;
                }
            }
            remove
            {
                gestureRecognized -= value;
            }
        }

        private static BodyFrameReader bodyFrameReader;

        public static void AddGestures(IGestureFactory factory)
        {
            /*foreach (BaseGesture gesture in factory.CreateGestures())
            {
                AddGesture(gesture);
            }*/
            var gestures = factory.CreateGestures().ToList();
            KnownGestures.AddRange(gestures);
        }

        public static void AddGestures(params BaseGesture[] baseGestures)
        {
            KnownGestures.AddRange(baseGestures);
        }

        public static void AddGesture(BaseGesture baseGesture)
        {
            //verifier si élément déjà présent ou non 
            KnownGestures.Add(baseGesture);
            baseGesture.GestureRecognized += Gesture_GestureRecognized;
        }

        public static void RemoveGesture(BaseGesture baseGesture)
        {
            KnownGestures.Remove(baseGesture);
            baseGesture.GestureRecognized -= Gesture_GestureRecognized;
        }

        public static void StartAcquiringFrames(KinectManager manager)
        {
            if (!isAcquiringFrame)
            {
                KinectManager = manager;
                KinectManager.StartSensor();
                bodyFrameReader = KinectManager.KinectSensor.BodyFrameSource.OpenReader();
                bodyFrameReader.FrameArrived += Reader_FrameArrivedBody;
                isAcquiringFrame = true;
            }
        }

        public static void StopAcquiringFrame()
        {
            if (isAcquiringFrame)
            {
                KinectManager.StopSensor();
                bodyFrameReader.FrameArrived -= Reader_FrameArrivedBody;
                bodyFrameReader?.Dispose();
                bodyFrameReader = null;
                isAcquiringFrame = false;
            }
        }

        private static void Reader_FrameArrivedBody(object sender, BodyFrameArrivedEventArgs e)
        {
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    foreach (var body in bodies)
                    {
                        if (body != null && body.IsTracked)
                        {
                            foreach (BaseGesture currentGesture in KnownGestures)
                            {
                                currentGesture.TestGesture(body);
                            }
                        }
                    }
                }
            }
        }
        private static void Gesture_GestureRecognized(object sender, GestureRecognizedEventArgs e)
        {
            gestureRecognized?.Invoke(sender, e);
        }

    }
}