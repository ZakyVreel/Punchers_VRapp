using MyGesturesBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    public class BoxingGestureFactory : IGestureFactory
    {
        public IEnumerable<BaseGesture> CreateGestures()
        {
            return new List<BaseGesture>
            {
                //new PostureHandUpLeft(),
                //new PostureHandUpRight(),
                new BoxePosture()
            };
        }
    }
}
