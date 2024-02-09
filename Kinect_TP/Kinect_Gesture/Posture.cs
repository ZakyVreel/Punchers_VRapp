using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    /// <summary>
    /// La classe posture
    /// </summary>
    public abstract class Posture : BaseGesture
    {
        public event EventHandler PostureRecognized;
        public event EventHandler PostureUnrecognized;

        private bool isPostureRecognized = false;

        /// <summary>
        /// Tests pour le posture.
        /// </summary>
        /// <param name="body">Le corps</param>
        protected abstract bool TestPosture(Body body);

        public override void TestGesture(Body body)
        {
            if (TestPosture(body))
            {
                if (!isPostureRecognized)
                {
                    OnGestureRecognized();
                    OnPostureRecognized();
                    isPostureRecognized = true;

                }
            }
            else
            {
                OnPostureUnrecognized();
                isPostureRecognized = false;
            }
        }

        private void OnPostureRecognized()
        {

            PostureRecognized?.Invoke(this, EventArgs.Empty);
            
        }

        private void OnPostureUnrecognized()
        {
            PostureUnrecognized?.Invoke(this, EventArgs.Empty); 
        }
    }
}
