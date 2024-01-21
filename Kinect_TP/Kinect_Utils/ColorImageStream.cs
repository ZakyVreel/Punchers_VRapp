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
            FrameDescription colorFrameDescription = this.Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            this.bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96, 96, PixelFormats.Bgra32, null);

            // Ouvre le lecteur pour les frames de couleurs
            this.colorFrameReader = this.Sensor.ColorFrameSource.OpenReader();

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

            // Faut-il verrouiller au début?

            // ColorFrame est IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    // Crée un tableau pour stocker les données de couleur
                    // Width * Height = total de pixels dans l'image
                    // BitsPerPixel = rapport bits/pixel
                    byte[] colorData = new byte[colorFrameDescription.Width * colorFrameDescription.Height * colorFrameDescription.BytesPerPixel];

                    // Vérifie les données et copie les nouvelles données de trame couleur dans le tableau
                    if ((colorFrameDescription.Width == this.bitmap.PixelWidth) && (colorFrameDescription.Height == this.bitmap.PixelHeight))
                    {
                        // CopyConvertedFrameDataToArray est utilisé pour obtenir les données de couleur
                        //colorFrame.CopyConvertedFrameDataToArray(colorData, ColorImageFormat.Bgra);

                        //colorFrame.CopyRawFrameDataToIntPtr(colorData, ColorImageFormat.Bgra);  Si ça marche pas, on va utiliser cela

                       
                        if (colorFrame.RawColorImageFormat == ColorImageFormat.Bgra)
                        {
                            colorFrame.CopyRawFrameDataToArray(colorData);
                        }
                        else
                        {
                            colorFrame.CopyConvertedFrameDataToArray(colorData, ColorImageFormat.Bgra);
                        }

                        this.bitmap.Lock();

                        // Écrit les données de couleur dans le bitmap
                        // Int32Rect : zone à l'intérieur du bitmap à mettre à jour => dans ce cas, tout
                        try
                        {
                            this.bitmap.WritePixels(
                                new Int32Rect(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
                                colorData,
                                (int)(colorFrameDescription.Width * colorFrameDescription.BytesPerPixel * colorFrameDescription.Height),
                                0);
                        }
                        finally
                        {
                            this.bitmap.AddDirtyRect(new Int32Rect(0, 0, this.bitmap.PixelWidth, this.bitmap.PixelHeight));
                            this.bitmap.Unlock();
                        }

                    }
                }
            }
        }
    }
}
