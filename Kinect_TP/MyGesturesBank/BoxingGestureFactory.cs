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
        private List<BaseGesture> gestures;
        public BoxingGestureFactory()
        {
            //Postures
            PostureHandUpRight postureHandUpRight = new PostureHandUpRight();
            PostureHandUpLeft postureHandUpLeft = new PostureHandUpLeft();
            BoxePosture boxePosture = new BoxePosture();

            //Gesture
            SwipeRightHandGesture swipeRightHandGesture = new SwipeRightHandGesture();

            gestures = new List<BaseGesture>
            {
                swipeRightHandGesture,
                boxePosture
            };
        }

        public BaseGesture this[string gestureName]
        {
            get { return this.gestures.FirstOrDefault(g => g.GestureName == gestureName); }
        }

        /// <summary>
        /// Crée tout les baseGesture
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseGesture> CreateGestures()
        {
            return this.gestures;
        }
    }
}
