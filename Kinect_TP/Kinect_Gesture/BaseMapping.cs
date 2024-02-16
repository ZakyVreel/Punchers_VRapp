using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{
    public abstract class BaseMapping<T>
    {
        private bool running;
        public event EventHandler<MappingEventArgs<T>> OnMapping;

        protected BaseMapping(BaseGesture startGesture, BaseGesture endGesture)
        {
            startGesture.GestureRecognized += (sender, args) =>
            {
                //if (!running && TestMapping(, out T output))
                //{
                //    running = true;
                //    OnMapping?.Invoke(this, new MappingEventArgs<T>(output));
                //}

            };

            endGesture.GestureRecognized += (sender, args) =>
            {
                if (running)
                {
                    running = false;
                }
            };
        }

        protected BaseMapping(BaseGesture toggleGesture)
        {
            toggleGesture.GestureRecognized += (sender, args) =>
            {
                running = !running;
                //if (running && TestMapping(, out T output))
                //{
                //    OnMapping?.Invoke(this, new MappingEventArgs<T>(output));
                //}
            };
        }

        public void SubscribeToStartGesture(BaseGesture gesture)
        {
            gesture.GestureRecognized += (sender, args) => running = true;
        }

        public void SubscribeToEndGesture(BaseGesture gesture)
        {
            gesture.GestureRecognized += (sender, args) => running = false;
        }

        public void SubscribeToToggleGesture(BaseGesture gesture)
        {
            gesture.GestureRecognized += (sender, args) => running = !running;
        }

        protected abstract T Mapping(Body body);
        public virtual bool TestMapping(Body body, out T output)
        {
            output = Mapping(body);
            return true;
        }

        protected virtual void OnBodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    Body[] bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    foreach (var body in bodies)
                    {
                        if (body != null && running)
                        {
                            if (TestMapping(body, out T output))
                            {
                                OnMapping?.Invoke(this, new MappingEventArgs<T>(output));
                            }
                        }
                    } 
                }
            }
        }
    }

    public class MappingEventArgs<T> : EventArgs
    {
        public T MappingValue { get; }

        public MappingEventArgs(T MappingValue)
        {
            this.MappingValue = MappingValue;
        }
    }
}
