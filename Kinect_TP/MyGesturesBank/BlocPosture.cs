using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class BlocPosture : Posture
    {
        public BlocPosture()
        {
            GestureName = "BlocPosture";
        }

        protected override bool TestPosture(Body body)
        {
            // Obtenir les positions des joints nécessaires
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint leftHandPosition = body.Joints[JointType.HandLeft].Position;
            CameraSpacePoint rightShoulderPosition = body.Joints[JointType.ShoulderRight].Position;
            CameraSpacePoint leftShoulderPosition = body.Joints[JointType.ShoulderLeft].Position;

            // Vérifier si la main gauche est au-dessus de l'épaule droite
            bool leftHandAboveRightShoulder = leftHandPosition.Y > rightShoulderPosition.Y;
            bool testX = leftHandPosition.X >= rightShoulderPosition.X - 0.1 && leftHandPosition.X <= rightShoulderPosition.X + 0.1;

            // Vérifier si la main droite est au-dessus de l'épaule gauche
            bool rightHandAboveLeftShoulder = rightHandPosition.Y > leftShoulderPosition.Y;
            bool testX2 = rightHandPosition.X >= leftShoulderPosition.X - 0.1 && rightHandPosition.X <= leftShoulderPosition.X + 0.1;

            // Retourner true si la main gauche est au-dessus de l'épaule droite et si la main droite est au-dessus de l'épaule gauche
            return leftHandAboveRightShoulder && rightHandAboveLeftShoulder && testX && testX2;
        }
    }
}
