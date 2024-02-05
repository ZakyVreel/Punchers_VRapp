using Microsoft.Kinect;
using MyGesturesBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureTester
{
    public class Program
    {
        static void Main(string[] args)
        {
            // On crée les postures
            PostureHandUpRight postureHandUpRight = new PostureHandUpRight();
            PostureHandUpLeft postureHandUpLeft = new PostureHandUpLeft();

            // Simuler un corps avec des positions de joints
            Body simulatedBody = SimulateBody();

            // On simule la posture souhaitée
            postureHandUpRight.TestGesture(simulatedBody);

            // On teste la posture
            // Souscrire à l'événement GestureRecognized
            postureHandUpRight.GestureRecognized += (sender, e) =>
            {
                Console.WriteLine($"La posture de {e.GestureName} a été reconnue.");
            };
        }

        private static Body SimulateBody()
        {
            Body [] body = new Body[1];

            //body[0].Joints[JointType.Head] = new Joint { Position = new CameraSpacePoint { X = 0, Y = 0, Z = 0 } };

            return body[0];
        }
    }
}
