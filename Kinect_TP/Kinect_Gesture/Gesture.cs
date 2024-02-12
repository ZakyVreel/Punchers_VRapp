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

        private int mCurrentFrameCount;

        protected abstract bool TestInitialConditions(Body body);

        protected abstract bool TestPosture(Body body);

        protected abstract bool TestRunningGesture(Body body);

        protected abstract bool TestEndConditions(Body body);
    }
}
