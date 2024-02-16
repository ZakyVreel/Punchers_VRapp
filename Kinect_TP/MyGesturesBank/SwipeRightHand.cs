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
    /// <summary>
    /// Représente la gestuelle de balayage vers la droite avec la main.
    /// </summary>
    public class SwipeRightHandGesture : Gesture
    {
        private CameraSpacePoint previousRightHandPosition;


        /// <summary>
        /// Initialise une nouvelle instance de la classe SwipeRightHandGesture.
        /// </summary>
        public SwipeRightHandGesture()
        {
            MinNbOfFrames = 10;
            MaxNbOfFrames = 30;
            GestureName = "Swipe Right Hand Gesture";
        }


        /// <summary>
        /// Vérifie les conditions initiales pour reconnaître le geste.
        /// </summary>
        protected override bool TestInitialConditions(Body body)
        {
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint headPosition = body.Joints[JointType.Head].Position;
            CameraSpacePoint hipPosition = body.Joints[JointType.SpineBase].Position;

            // Vérifier si la main droite est entre la tête et le torse sur l'axe Y
            bool isRightHandBetweenHeadAndHip = rightHandPosition.Y <= headPosition.Y && rightHandPosition.Y >= hipPosition.Y;

            // Vérifier si la main droite est à une distance horizontale appropriée de la tête pour initier le geste
            bool isRightHandForwardEnough = rightHandPosition.Z < headPosition.Z - 0.2f;

            // Vérifier si la main droite se déplace vers la gauche
            bool isMovingLeft = rightHandPosition.X < previousRightHandPosition.X;

            // Démarrer la reconnaissance du geste si la main droite est suffisamment avancée vers l'avant et commence à se déplacer vers la gauche
            return isRightHandBetweenHeadAndHip && isRightHandForwardEnough && isMovingLeft;
        }

        /// <summary>
        /// Vérifie la posture de la main pendant le geste.
        /// </summary>
        protected override bool TestPosture(Body body)
        {
            // Obtenir les positions des joints nécessaires
            CameraSpacePoint headPosition = body.Joints[JointType.Head].Position;
            CameraSpacePoint hipPosition = body.Joints[JointType.SpineBase].Position;
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;

            // Vérifier si la main droite est entre la tête et le hip
            bool isHandBetweenHeadAndHip = rightHandPosition.Y <= headPosition.Y && rightHandPosition.Y >= hipPosition.Y;

            return isHandBetweenHeadAndHip;
        }


        /// <summary>
        /// Vérifie si le geste est en cours d'exécution.
        /// </summary>
        protected override bool TestRunningGesture(Body body)
        {
            CameraSpacePoint currentRightHandPosition = body.Joints[JointType.HandRight].Position;

            // Vérifier si la main droite se déplace vers la gauche (position actuelle moins que précédente)
            bool isMovingLeft = currentRightHandPosition.X < previousRightHandPosition.X;

            // Mettre à jour la position précédente de la main droite pour la prochaine frame
            previousRightHandPosition = currentRightHandPosition;

            return isMovingLeft;
        }


        /// <summary>
        /// Vérifie les conditions de fin du geste.
        /// </summary>
        protected override bool TestEndConditions(Body body)
        {
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint leftHipPosition = body.Joints[JointType.HipLeft].Position;

            // Vérifier si la main droite a dépassé la position horizontale du hip gauche
            return rightHandPosition.X > leftHipPosition.X;
        }

    }
}
