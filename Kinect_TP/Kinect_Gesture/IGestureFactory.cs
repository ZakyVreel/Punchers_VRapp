using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    //Interface factory pour les gestes
    public interface IGestureFactory
    {
        IEnumerable<BaseGesture> CreateGestures();
        BaseGesture this[string gestureName] { get; }
    }
}
