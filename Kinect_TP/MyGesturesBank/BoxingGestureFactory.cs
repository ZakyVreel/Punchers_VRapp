using MyGesturesBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    public class BoxingGestureFactory : IGestureFactory
    {
        /// <summary>
        /// Crée tout les baseGesture
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseGesture> CreateGestures()
        {
            //Postures
            PostureHandUpRight postureHandUpRight = new PostureHandUpRight();
            PostureHandUpLeft postureHandUpLeft = new PostureHandUpLeft();
            BoxePosture boxePosture = new BoxePosture();

            //Gesture
            SwipeRightHandGesture swipeRightHandGesture = new SwipeRightHandGesture();

            BaseGesture[] gestures = new BaseGesture[2];
            gestures[0] = swipeRightHandGesture;
            gestures[1] = boxePosture;

            return gestures;
        }
    }
}
