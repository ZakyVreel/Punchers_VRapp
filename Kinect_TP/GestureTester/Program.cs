using Kinect_Gesture;
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
        private static KinectSensor kinectSensor;

        private static PostureHandUpRight postureHandUpRight = new PostureHandUpRight();
        private static PostureHandUpLeft postureHandUpLeft = new PostureHandUpLeft();
        static void Main(string[] args)
        {
            
            kinectSensor = KinectSensor.GetDefault();

            // On crée les postures
            postureHandUpRight.GestureRecognized += PostureHandUpRight_GestureRecognized;

            if (kinectSensor != null)
            {
                kinectSensor.Open();

                // Utilisation du bloc using pour bodyFrameReader
                using (var bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader())
                {
                    if (bodyFrameReader != null)
                    {
                        // Abonnement à l'événement FrameArrived
                        bodyFrameReader.FrameArrived += BodyFrameReader_FrameArrived;

                        Console.WriteLine("Lecture des données du corps en cours... Appuyez sur une touche pour quitter.");
                        Console.ReadKey();
                    }
                }
            }

            if (kinectSensor != null)
            {
                kinectSensor.Close();
                kinectSensor = null;
            }


        }

        private static void PostureHandUpRight_GestureRecognized(object sender, GestureRecognizedEventArgs e)
        {
            Console.WriteLine("Posture Hand Up Right a été reconnue !");
        }
        private static void BodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(bodies);

                    foreach (Body body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            postureHandUpRight.TestGesture(body);
                        }
                    }
                }
            }
        }

    }
}
