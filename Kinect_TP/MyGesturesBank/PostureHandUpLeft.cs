using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{

    public class PostureHandUpLeft : Posture
    {
        public PostureHandUpLeft()
        {
            GestureName = "HandUpLeft";
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
            // Tester si la main gauche est au même niveau de l'epoule
            return body.Joints[JointType.HandLeft].Position.Y == body.Joints[JointType.ShoulderLeft].Position.Y;
        }

    }
}
