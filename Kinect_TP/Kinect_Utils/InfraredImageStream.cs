using Kinect_TP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Utils
{
    public class InfraredImageStream : KinectStream
    {
        public InfraredImageStream(KinectManager manager) : base(manager)
        {
        }
    }
}
