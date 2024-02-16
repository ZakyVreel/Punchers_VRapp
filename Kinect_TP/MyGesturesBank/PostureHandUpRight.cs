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
    public class PostureHandUpRight : Posture
    {
        
        public PostureHandUpRight()
        {
            GestureName = "HandUpRight";
        }

        protected override bool TestPosture(Body body)
        {
            // Obtenir les positions des joints de la main droite et de l'épaule droite
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint rightShoulderPosition = body.Joints[JointType.ShoulderRight].Position;

            // Définir une marge de 10 pixels
            float margin = 10f;

            // Tester si la main droite est dans une plage de 10 pixels autour de l'épaule droite en Y
            // Abs: valeur abslout : Si cette différence est inférieure ou égale à la marge définie (10 pixels), la condition est vraie
            //return Math.Abs(rightHandPosition.Y - rightShoulderPosition.Y) <= margin;
            return body.Joints[JointType.HandRight].Position.Y > body.Joints[JointType.ShoulderRight].Position.Y;
        }
    }
}
