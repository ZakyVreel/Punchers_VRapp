using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class RightPunchGesture : Gesture
    {
        private CameraSpacePoint lastRightHandPosition;
        private CameraSpacePoint lastRightElbowPosition;

        public RightPunchGesture()
        {
            MinNbOfFrames = 10;
            MaxNbOfFrames = 30;
        }

        public override void TestGesture(Body body)
        {
            if (TestPosture(body))
            {
                Console.WriteLine("Gesture reconnu, Right Punch");
                Thread.Sleep(1000);

                OnGestureRecognized();
            }
        }

        protected override bool TestEndConditions(Body body)
        {
            // Obtenir les positions des joints de la main et l'épaule droite, la main et l'épaule gauche
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint rightShoulderPosition = body.Joints[JointType.ShoulderRight].Position;
            CameraSpacePoint leftHandPosition = body.Joints[JointType.HandLeft].Position;
            CameraSpacePoint leftShoulderPosition = body.Joints[JointType.ShoulderLeft].Position;
            // Définir une marge de tolérance pour l'alignement vertical
            float toleranceMarginY = 0.1f; // Ajustez cette valeur selon vos besoins
            float toleranceMarginX = 0.05f;

            // Vérifier si la coordonnée Y de la main droite est dans une plage autour de l'épaule droite
            float minRightY = rightShoulderPosition.Y - toleranceMarginY;
            float maxRightY = rightShoulderPosition.Y + toleranceMarginY;

            float minRightX = rightShoulderPosition.X - toleranceMarginX;
            float maxRightX = rightShoulderPosition.X + toleranceMarginX;

            // Vérifier si la coordonnée Y de la main gauche est dans une plage autour de l'épaule gauche
            float minLeftY = leftShoulderPosition.Y - toleranceMarginY;
            float maxLeftY = leftShoulderPosition.Y + toleranceMarginY;

            float minLeftX = leftShoulderPosition.X - toleranceMarginX;
            float maxLeftX = leftShoulderPosition.X + toleranceMarginX;

            // Vérifier si la main droite est dans la plage autour de l'épaule droite
            bool handRightInToleranceRange = rightHandPosition.Y >= minRightY && rightHandPosition.Y <= maxRightY;

            // Vérifier si la main gauche est dans la plage autour de l'épaule gauche
            bool handLeftInToleranceRange = leftHandPosition.Y >= minLeftY && leftHandPosition.Y <= maxLeftY;

            // Vérifier si la main droite est suffisamment proche de l'épaule droite en termes de coordonnée X
            // bool handRightInXRange = Math.Abs(rightHandPosition.X - rightShoulderPosition.X) <= toleranceMargin;
            bool handRightInXRange = rightHandPosition.X >= minRightX && rightHandPosition.X <= maxRightX;

            // Vérifier si la main droite est suffisamment proche de l'épaule droite en termes de coordonnée X
            // bool handLeftInXRange = Math.Abs(leftHandPosition.X - leftShoulderPosition.X) <= toleranceMargin;
            bool handLeftInXRange = leftHandPosition.X >= minLeftX && leftHandPosition.X <= maxLeftX;

            // Retourner true uniquement si la main droite est dans la plage autour de l'épaule droite et suffisamment proche en termes de coordonnée X
            return handRightInToleranceRange && handLeftInToleranceRange && handRightInXRange && handLeftInXRange;
        }

        protected override bool TestInitialConditions(Body body)
        {
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint rightShoulderPosition = body.Joints[JointType.ShoulderRight].Position;

            // Vérifier si la main droite est au même niveau que l'épaule droite
            bool isHandAtShoulderLevel = Math.Abs(rightHandPosition.Y - rightShoulderPosition.Y) < 0.1f;

            // Vérifier si le bras droit est étendu
            bool isArmExtended = rightHandPosition.X > rightShoulderPosition.X;

            return isHandAtShoulderLevel && isArmExtended;
        }

        protected override bool TestPosture(Body body)
        {
            // Récupérer les positions des joints nécessaires
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint rightShoulderPosition = body.Joints[JointType.ShoulderRight].Position;

            // Vérifier si le bras droit est tendu (la main est plus avancée que l'épaule)
            bool isArmExtended = rightHandPosition.Z < rightShoulderPosition.Z;

            // Vérifier si la main droite est au même niveau que l'épaule droite selon l'axe Y
            bool isHandAtShoulderLevel = Math.Abs(rightHandPosition.Y - rightShoulderPosition.Y) < 0.1f;

            // Le geste de coup de poing est considéré comme valide si le bras est tendu et que la main est au même niveau que l'épaule
            return isArmExtended && isHandAtShoulderLevel;
        }


        protected override bool TestRunningGesture(Body body)
        {
            CameraSpacePoint currentRightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint currentRightElbowPosition = body.Joints[JointType.ElbowRight].Position;

            // Déterminez si la main droite se déplace vers l'avant par rapport à sa position précédente
            bool isRightHandMovingForward = currentRightHandPosition.Z < lastRightHandPosition.Z;

            // Déterminez si le coude droit se déplace vers l'avant par rapport à sa position précédente
            bool isRightElbowMovingForward = currentRightElbowPosition.Z < lastRightElbowPosition.Z;

            // Mettez à jour les positions précédentes pour la prochaine frame
            lastRightHandPosition = currentRightHandPosition;
            lastRightElbowPosition = currentRightElbowPosition;

            // Retourne true si la main droite et le coude droit se déplacent vers l'avant, ce qui peut indiquer un coup de poing droit
            return isRightHandMovingForward && isRightElbowMovingForward;
        }
    }
}
