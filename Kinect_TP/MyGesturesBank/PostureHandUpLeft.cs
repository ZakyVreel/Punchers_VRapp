using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    //Cette posture marche, mais n'est pas utilisé.
    public class PostureHandUpLeft : Posture
    {
        public PostureHandUpLeft()
        {
            GestureName = "HandUpLeft";
        }

        protected override bool TestPosture(Body body)
        {
            // Obtenir les positions des joints de la main gauche et de l'épaule gauche
            CameraSpacePoint leftHandPosition = body.Joints[JointType.HandLeft].Position;
            CameraSpacePoint leftShoulderPosition = body.Joints[JointType.ShoulderLeft].Position;

            // Définir une marge de 10 pixels
            float margin = 10f;

            // Tester si la main gauche est dans une plage de 10 pixels autour de l'épaule gauche en Y
            // Abs: valeur abslout : Si cette différence est inférieure ou égale à la marge définie (10 pixels), la condition est vraie
            return Math.Abs(leftHandPosition.Y - leftShoulderPosition.Y) <= margin;
        }

    }
}
