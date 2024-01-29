using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Utils
{
    // Enumeration permetant de choisir le imageStream qu'on veut dans la factory
    public enum KinectStreams
    {
        None,
        Color,
        Depth,
        IR,
        Body
    }
}
