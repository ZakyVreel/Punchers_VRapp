using Kinect_TP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Utils
{
    public class DepthImageStream : KinectStream
    {
        public DepthImageStream(KinectManager manager) : base(manager)
        {
        }
    }
}
