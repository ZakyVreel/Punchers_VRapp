using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class BoxePosture : Posture
    {
        public BoxePosture()
        {
            GestureName = "BoxePosture";
        }
        protected override bool TestPosture(Body body)
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
    }
}
