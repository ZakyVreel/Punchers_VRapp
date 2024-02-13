using Kinect_Gesture;
using Kinect_TP;
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
        private static BoxePosture postureBoxe = new BoxePosture();

        static void Main(string[] args)
        {

            // Créer une instance de la factory de gestes
            IGestureFactory factory = new BoxingGestureFactory();

            // Ajouter les gestes créés par la factory au gestionnaire de gestes
            GestureManager.AddGestures(factory);

            // Abonner à l'événement GestureRecognized pour chaque geste connu
            foreach (var gesture in GestureManager.KnownGestures)
            {
                gesture.GestureRecognized += (sender, arg) =>
                {
                    Console.WriteLine($"Geste reconnu : {arg.GestureName}");
                };
            }

            // Démarrer l'acquisition de trames
            GestureManager.StartAcquiringFrames(GestureManager.KinectManager);

            // Attendre l'appui sur une touche pour quitter
            Console.WriteLine("Appuyez sur une touche pour quitter...");
            Console.ReadKey();

            // Arrêter l'acquisition de trames lorsque l'application se termine
            GestureManager.StopAcquiringFrame();
        }

        private static void TestConsoleGestureManager()
        {
            // Créer une instance de KinectManager 
            KinectManager kinectManager = new KinectManager();

            // Ajouter des gestes à la liste des gestes connus
            GestureManager.AddGestures(new BoxingGestureFactory());

            // Démarrer l'acquisition de trames
            GestureManager.StartAcquiringFrames(kinectManager);

            // Abonnez à l'événement GestureRecognized
            GestureManager.GestureRecognized += GestureManager_GestureRecognized;

            Console.WriteLine("Appuyez sur une touche pour quitter.");
            Console.ReadKey();

            // Arrêter l'acquisition de trames lorsque l'application se termine
            GestureManager.StopAcquiringFrame();
        }

        private static void TestConsolePosture()
        {
            kinectSensor = KinectSensor.GetDefault();

            // On crée les postures
            postureBoxe.PostureRecognized += PostureBoxe_GestureRecognized;

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

       


        private static void PostureBoxe_GestureRecognized(object sender, EventArgs e)
        {
            Console.WriteLine("Posture Boxe a été reconnue !");
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
                            //Decomenter cette ligne pour tester le handUpRight posture et commenter le postureBoxe.TestGesture(body)
                            //postureHandUpRight.TestGesture(body);
                            postureBoxe.TestGesture(body);
                        }
                    }
                }
            }
        }

        private static void GestureManager_GestureRecognized(object sender, GestureRecognizedEventArgs e)
        {
            // Gestionnaire d'événements pour la reconnaissance de gestes
            Console.WriteLine($"Geste reconnu : {e.GestureName}");
        }

    }
}
