using Kinect_Gesture;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGesturesBank
{
    public class SwipeRightHandGesture : Gesture
    {
        private CameraSpacePoint previousRightHandPosition;
        private int mCurrentFrameCount;

        public SwipeRightHandGesture()
        {
            MinNbOfFrames = 10;
            MaxNbOfFrames = 30;
            GestureName = "Swipe Right Hand Gesture";
        }

        public override void TestGesture(Body body)
        {
            if (IsTesting)
            {
                if (TestEndConditions(body))
                {
                    OnGestureRecognized();
                    IsTesting = false;
                }
                else if (!TestRunningGesture(body))
                {
                    // Réinitialiser si les conditions du geste en cours ne sont pas remplies
                    IsTesting = false;
                }
            }
            else if (TestInitialConditions(body))
            {
                IsTesting = true;
                mCurrentFrameCount = 0;

            }
        }

        protected override bool TestInitialConditions(Body body)
        {
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;
            CameraSpacePoint spineMidPosition = body.Joints[JointType.SpineMid].Position;

            // Vérifiez si la main droite est au niveau du torse et fait un balayage vers la gauche
            return rightHandPosition.Y >= spineMidPosition.Y && rightHandPosition.X < previousRightHandPosition.X;
        }

        protected override bool TestPosture(Body body)
        {
            // Aucune exigence spécifique de posture statique pour ce geste
            return true;
        }

        protected override bool TestRunningGesture(Body body)
        {
            CameraSpacePoint rightHandPosition = body.Joints[JointType.HandRight].Position;

            // Vérifiez si la main droite fait un balayage vers la gauche
            bool isSwipingLeft = rightHandPosition.X < previousRightHandPosition.X;

            // Mettre à jour la position précédente de la main pour la prochaine frame
            previousRightHandPosition = rightHandPosition;

            return isSwipingLeft;
        }

        protected override bool TestEndConditions(Body body)
        {
            // Vérifiez si le geste a été effectué pendant le nombre de frames requis
            return mCurrentFrameCount >= MinNbOfFrames && mCurrentFrameCount <= MaxNbOfFrames;
        }
    }
}
