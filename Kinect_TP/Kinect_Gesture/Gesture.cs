using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    public abstract class Gesture : BaseGesture
    {
        public bool IsTesting;

        protected int MinNbOfFrames;

        protected int MaxNbOfFrames;

        protected int mCurrentFrameCount;

        protected abstract bool TestInitialConditions(Body body);

        protected abstract bool TestPosture(Body body);

        protected abstract bool TestRunningGesture(Body body);

        protected abstract bool TestEndConditions(Body body);

        private bool isGestureRecognized = false;

        public override void TestGesture(Body body)
        {
            if (IsTesting)
            {
                if (!isGestureRecognized)
                {
                    if (TestEndConditions(body))
                    {
                        OnGestureRecognized();
                        IsTesting = false;
                    }
                    else if (!TestRunningGesture(body) && !TestPosture(body))
                    {
                        // Réinitialiser si les conditions du geste en cours ne sont pas remplies
                        IsTesting = false;
                    }
                }
                
            }
            else if (TestInitialConditions(body))
            {
                IsTesting = true;
                isGestureRecognized = false;

            }
        }
    }
}
