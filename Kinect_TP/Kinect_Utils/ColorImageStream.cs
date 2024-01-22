using Kinect_TP;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Kinect_Utils
{

    /// <summary>
    /// Représente un flux d'image couleur pour le capteur Kinect.
    /// </summary>
    public class ColorImageStream : KinectStream, IDisposable
    {
        // Le bitmap pour le binding dans MainWindow.xaml
        private WriteableBitmap bitmap = null;
        public WriteableBitmap Bitmap
        {
            get { return bitmap; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref bitmap, value);
                }
            }
        }

        public ImageSource ImageSource { get { return this.bitmap; } }

        //ColorFrameReader va lire les trames de couleurs arrivent du kinect
        private ColorFrameReader colorFrameReader = null;


        /// <summary>
        /// Constructeur de la classe prenant un objet KinectManager en paramètre.
        /// </summary>
        public ColorImageStream(KinectManager kinectSensor) : base(kinectSensor)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Méthode surchargée de la classe de base, démarrant le flux d'image couleur.
        /// </summary>
        override public void Start()
        {
            // Ouvre le lecteur pour les frames de couleurs
            this.colorFrameReader = this.Sensor.ColorFrameSource.OpenReader();
            //Faut commenter ici ++++
            FrameDescription colorFrameDescription = this.Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            this.bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96, 96, PixelFormats.Bgra32, null);

            // S'abonne à l'événement
            this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;

            //base.Start(); Cela, on est d'accord, va simplement lancer la kinect?
        }

        /// <summary>
        /// Méthode surchargée de la classe de base, arrêtant le flux d'image couleur.
        /// </summary>
        override public void Stop()
        {
            if (this.colorFrameReader != null)
            {
                this.colorFrameReader.FrameArrived -= this.Reader_ColorFrameArrived;

                // Dispose du lecteur pour libérer les ressources.
                // Si nous ne le faisons pas manuellement, le GC le fera pour nous, mais nous ne savons pas quand.
                this.colorFrameReader.Dispose();
                this.colorFrameReader = null;

                //base.Stop(); Cela, on est d'accord, va simplement arrêter la kinect?
            }
        }


        /// <summary>
        /// Méthode appelée lorsqu'une nouvelle trame de couleur arrive.
        /// </summary>
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {

            // ColorFrame est IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        this.bitmap.Lock();

                        // verify data and write the new color frame data to the display bitmap
                        if ((colorFrameDescription.Width == this.bitmap.PixelWidth) && (colorFrameDescription.Height == this.bitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.bitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, this.bitmap.PixelWidth, this.bitmap.PixelHeight));
                        }

                        this.bitmap.Unlock();
                    }
                }
            }
        }
    }
}
