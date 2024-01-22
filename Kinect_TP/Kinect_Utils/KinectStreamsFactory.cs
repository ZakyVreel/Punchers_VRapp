using Kinect_TP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Utils
{
    /// <summary>
    /// Classe responsable de la création de différents types de flux pour le capteur Kinect.
    /// </summary>
    public class KinectStreamsFactory
    {

        // Dictionnaire associant les types de flux à leurs fonctions de création correspondantes.
        private Dictionary<KinectStreams, Func<KinectStream>> streamFactory;

        // KinectManager utilisé pour initialiser les flux.
        private KinectManager kinectManager;

        /// <summary>
        /// Constructeur de la classe, prenant un objet KinectManager en paramètre.
        /// </summary>
        public KinectStreamsFactory(KinectManager manager)
        {
            kinectManager = manager;

            // Initialise le dictionnaire des types de flux et de leurs fonctions de création associées.
            streamFactory = new Dictionary<KinectStreams, Func<KinectStream>>
            {
                { KinectStreams.None, () => null }, // Aucun flux
                { KinectStreams.Color, () => new ColorImageStream(kinectManager) },
                { KinectStreams.Depth, () => new DepthImageStream(kinectManager) },
                { KinectStreams.IR, () => new InfraredImageStream(kinectManager) },
                { KinectStreams.Body, () => new BodyStream(kinectManager) }
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
