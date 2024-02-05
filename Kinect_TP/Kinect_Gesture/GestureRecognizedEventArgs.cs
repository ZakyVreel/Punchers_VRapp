using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    /// <summary>
    /// le gesture recognized event args.
    /// </summary>
    public class GestureRecognizedEventArgs : EventArgs
    {
        /// <summary>
        /// Le nom du geste
        /// </summary>
        public string GestureName { get; set; }

        /// <summary>
        /// Le constructeur <see cref="GestureRecognizedEventArgs"/> class.
        /// </summary>
        public GestureRecognizedEventArgs(string gestureName)
        {
            GestureName = gestureName;
        }
    }
}
