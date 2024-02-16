using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    public class MapLeftHandToMouse : BaseMapping<Point>
    {
        public MapLeftHandToMouse(BaseGesture toggleGesture) : base(toggleGesture)
        {
        }

        public MapLeftHandToMouse(BaseGesture startGesture, BaseGesture endGesture) : base(startGesture, endGesture)
        {
        }

        protected override Point Mapping(Body body)
        {
            return new Point((int)body.Joints[JointType.HandLeft].Position.X, (int)body.Joints[JointType.HandLeft].Position.Y);
        }
    }
}
