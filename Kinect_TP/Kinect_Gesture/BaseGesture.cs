using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Kinect_Gesture
{
    /// <summary>
    /// La classe BaseGesture.
    /// </summary>
    public abstract class BaseGesture
    {
        /// <summary>
        /// Le constructeur
        /// </summary>
        public BaseGesture() { }

        /// <summary>
        /// Le nom du geste.
        /// </summary>
        public string GestureName { get; set; }

        /// <summary>
        /// L'event pour identifier le geste.
        /// </summary>
        public EventHandler<GestureRecognizedEventArgs> GestureRecognized { get; set; }

        /// <summary>
        /// Les teste gesture.
        /// </summary>
        /// <param name="body"></param>
        public abstract void TestGesture(Body body);


        /// <summary>
        /// Le OnRecognized pour le geste.
        /// </summary>
        protected void OnGestureRecognized()
        {
            GestureRecognized?.Invoke(this, new GestureRecognizedEventArgs(GestureName));
        }
    }
}
