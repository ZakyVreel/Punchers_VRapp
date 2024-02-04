using Kinect_TP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    public static class GestureManager
    {
        // Properties
        public static KinectManager KinectManager { get; set; } = new KinectManager();

        public static IEnumerable<BaseGesture> KnownGestures { get; private set; } = new List<BaseGesture>();

        // <GestureRecognizedEventArgs>
        public static EventHandler GestureRecognized { get; set; }

        public static IGestureFactory Factory { get; set; }

        public static void AddGestures(IGestureFactory factory)
        {
            throw new NotImplementedException();
        }

        public static void AddGestures(BaseGesture[] baseGestures) { throw new NotImplementedException(); }

        public static void RemoveGesture(BaseGesture baseGesture) { throw new NotImplementedException(); }

        public static void StartAcquiringFrames(KinectManager manager) { throw new NotImplementedException(); }

        public static void StopAcquiringFrame(KinectManager manager) { throw new NotImplementedException(); }

    }
}
