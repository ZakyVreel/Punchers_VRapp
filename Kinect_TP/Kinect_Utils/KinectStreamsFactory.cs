using Kinect_TP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Utils
{
    public class KinectStreamsFactory
    {
        private Dictionary<KinectStreams, Func<KinectStream>> streamFactory;

        private KinectManager kinectManager;

        public KinectStreamsFactory(KinectManager manager)
        {
            kinectManager = manager;
            streamFactory = new Dictionary<KinectStreams, Func<KinectStream>>
            {
                { KinectStreams.None, () => null },
                { KinectStreams.Color, () => new ColorImageStream(kinectManager) },
                { KinectStreams.Depth, () => new DepthImageStream(kinectManager) },
                { KinectStreams.IR, () => new InfraredImageStream(kinectManager) }
            };

        }

        public KinectStream? this[KinectStreams streamType]
        {
            get
            {
                if (streamFactory.TryGetValue(streamType, out var createStream))
                {
                    return createStream();
                }

                return null;
            }
        }
    }
}
