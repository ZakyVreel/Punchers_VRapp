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
        /// <summary>
        /// Tests pour le posture.
        /// </summary>
        /// <param name="body">Le corps</param>
        protected abstract bool TestPosture(Body body);
    }
}
