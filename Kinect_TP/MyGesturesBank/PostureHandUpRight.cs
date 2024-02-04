using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{

    public class PostureHandUpRight : Posture
    {

        public PostureHandUpRight()
        {
            GestureName = "HandUpRight";
        }

        public override void TestGesture(Body body)
        {
            if (TestPosture(body))
            {
                OnGestureRecognized();
            }
        }

        protected override bool TestPosture(Body body)
        {
            // Tester si la main droite est au même niveau de l'epoule
            return body.Joints[JointType.HandRight].Position.Y == body.Joints[JointType.ShoulderRight].Position.Y;
        }
    }
}
