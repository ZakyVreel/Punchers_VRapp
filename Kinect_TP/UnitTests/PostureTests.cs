using Kinect_TP;
using Microsoft.Kinect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyGesturesBank;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class PostureTests
    {

        [TestMethod]
        public void TestPostureHandUpRight()
        {
            var posture = new PostureHandUpRight();

            //marche pas car System.NotSupportedException: Type to mock (Body) must be an interface, a delegate, or a non-sealed, non-static class
            var mockBody = new Mock<Body>();

            var joints = new Dictionary<JointType, Joint>
        {
            { JointType.HandRight, new Joint { JointType = JointType.HandRight, Position = new CameraSpacePoint { X = 0, Y = 10, Z = 0 } } },
            { JointType.ShoulderRight, new Joint { JointType = JointType.ShoulderRight, Position = new CameraSpacePoint { X = 0, Y = 0, Z = 0 } } }
        };

            mockBody.Setup(b => b.Joints.GetEnumerator()).Returns(joints.GetEnumerator());


            bool gestureRecognized = false;
            // on abonne une lambda à l'event pour set à true le bool si l'event est declenché
            posture.GestureRecognized += (sender, args) => gestureRecognized = true;

            posture.TestGesture(mockBody.Object);

            Assert.IsTrue(gestureRecognized);
        }
    }
}
