using Kinect_TP;
using Microsoft.Kinect;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kinect_Utils
{
    /// <summary>
    /// Classe représentant un flux d'image infrarouge pour la Kinect.
    /// </summary>
    public class InfraredImageStream : KinectStream
    {
        private const float InfraredSourceValueMaximum = (float)ushort.MaxValue; //Valeur Max que peut retourner le InfraredFrame
        private const float InfraredOutputValueMinimum = 0.01f; //Valeur Min pour l'affichage quand le infrared data va être normal
        private const float InfraredOutputValueMaximum = 1.0f; //Valeur Max pour l'affichage quand le infrared data va être normal
        private const float InfraredSourceScale = 0.75f; //La valeur du scale pour la source


        private FrameDescription infraredFrameDescription = null;
        private WriteableBitmap infraredBitmap = null;
        private KinectSensor kinectSensor = null;
        private InfraredFrameReader infraredFrameReader = null;

        /// <summary>
        /// Obtient la source d'image de la classe.
        /// </summary>
        public override ImageSource ImageSource
        {
            get
            {
                return this.infraredBitmap;
            }
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe InfraredImageStream.
        /// </summary>
        public InfraredImageStream(KinectManager manager) : base(manager)
        {
            this.infraredFrameDescription = this.Sensor.InfraredFrameSource.FrameDescription;

            //On on prends les pixelWidht, pixelHeight, le DPIHorizontale et DPIVerticale , ensuite le format qu'on veut
            this.infraredBitmap = new WriteableBitmap(this.infraredFrameDescription.Width, this.infraredFrameDescription.Height, 96, 96, PixelFormats.Gray32Float, null);
        }

        /// <summary>
        /// Démarre la lecture du flux infrarouge.
        /// </summary>
        public override void Start()
        {
            if (this.Sensor != null)
            {
                this.infraredFrameReader = this.Sensor.InfraredFrameSource.OpenReader();

                this.infraredFrameReader.FrameArrived += this.Reader_InfraredFrameArrived;
                
            }
        }

        /// <summary>
        /// Arrête la lecture du flux infrarouge.
        /// </summary>
        public override void Stop()
        {
            if (this.infraredFrameReader != null)
            {
                this.infraredFrameReader.FrameArrived -= this.Reader_InfraredFrameArrived;
                this.infraredFrameReader.Dispose();
                this.infraredFrameReader = null;
            }
        }

        /// <summary>
        /// Méthode appelée lors de l'arrivée d'un nouveau frame infrarouge.
        /// </summary>
        private void Reader_InfraredFrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            using (InfraredFrame infraredFrame = e.FrameReference.AcquireFrame())
            {
                if (infraredFrame != null)
                {
                    using (KinectBuffer infraredBuffer = infraredFrame.LockImageBuffer())
                    {
                        if (((this.infraredFrameDescription.Width * this.infraredFrameDescription.Height) == (infraredBuffer.Size / this.infraredFrameDescription.BytesPerPixel)) &&
                            (this.infraredFrameDescription.Width == this.infraredBitmap.PixelWidth) && (this.infraredFrameDescription.Height == this.infraredBitmap.PixelHeight))
                        {
                            this.ProcessInfraredFrameData(infraredBuffer.UnderlyingBuffer, infraredBuffer.Size);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Accède directement au tampon d'image sous-jacent du InfraredFrame pour créer une bitmap affichable.
        /// </summary>
        /// <param name="infraredFrameData">Pointeur vers les données d'image InfraredFrame</param>
        /// <param name="infraredFrameDataSize">Taille des données d'image InfraredFrame</param>
        private unsafe void ProcessInfraredFrameData(IntPtr infraredFrameData, uint infraredFrameDataSize)
        {
            // Les données du frame infrarouge sont des valeurs sur 16 bits
            ushort* frameData = (ushort*)infraredFrameData;

            // On lock la bitmap
            this.infraredBitmap.Lock();

            // Obtenir le pointeur vers le backfuffer de la bitmap
            float* backBuffer = (float*)this.infraredBitmap.BackBuffer;

            // Traiter les données infrarouges
            for (int i = 0; i < (int)(infraredFrameDataSize / this.infraredFrameDescription.BytesPerPixel); ++i)
            {
                // Étant donné que nous affichons l'image comme une image en niveaux de gris normalisée,
                // nous devons convertir les données ushort qui ont été fournis par le InfraredFrame
                // en une valeur comprise entre [InfraredOutputValueMinimum, InfraredOutputValueMaximum]
                backBuffer[i] = Math.Min(InfraredOutputValueMaximum, (((float)frameData[i] / InfraredSourceValueMaximum * InfraredSourceScale) * (1.0f - InfraredOutputValueMinimum)) + InfraredOutputValueMinimum);
            }

            // Marquer l'ensemble de la bitmap comme nécessitant d'être dessiné
            this.infraredBitmap.AddDirtyRect(new Int32Rect(0, 0, this.infraredBitmap.PixelWidth, this.infraredBitmap.PixelHeight));

            // On unlock la bitmap
            this.infraredBitmap.Unlock();
        }
    }
}
