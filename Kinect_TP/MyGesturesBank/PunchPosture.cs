using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class PunchPosture : Posture
    {
        public PunchPosture()
        {
            GestureName = "PunchPosture";
        }
        protected override bool TestPosture(Body body)
        {
            // Obtenir la position des joints nécessaires
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint rightElbowPosition = body.Joints[JointType.ElbowRight].Position;
            CameraSpacePoint rightShoulderPosition = body.Joints[JointType.ShoulderRight].Position;
            HandState rightHandState = body.HandRightState;

            // Vérifier si le bras droit est tendu en vérifiant si la main droite est en avant de l'épaule droite
            // bool rightArmStraight = rightHandPosition.Z > rightElbowPosition.Z;

            // Définir une marge de tolérance pour l'alignement vertical
            float toleranceMarginY = 0.05f; // Ajustez cette valeur selon vos besoins

            // Vérifier si le bras droit est tendu et aligné avec l'épaule
            bool test = rightHandPosition.Y < rightShoulderPosition.Y + toleranceMarginY &&
                                    rightElbowPosition.Y < rightShoulderPosition.Y + toleranceMarginY;

            // Vérifier si le coude est aligné plus ou moins avec l'épaule et la main
            bool elbowAlignedWithShoulderAndHand =
                Math.Abs(rightElbowPosition.Y - rightShoulderPosition.Y) < toleranceMarginY &&
                Math.Abs(rightElbowPosition.Y - rightHandPosition.Y) < toleranceMarginY;



            // Retourner true si le bras droit est tendu et le poing est serré
            return elbowAlignedWithShoulderAndHand && test;
        }
    }
}
