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
    public class ColorImageStream : KinectStream, IDisposable
    {
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

        private ColorFrameReader colorFrameReader = null;


        public ColorImageStream(KinectManager kinectSensor) : base(kinectSensor)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        override public void Start()
        {
            base.Start();
            this.colorFrameReader = this.Sensor.ColorFrameSource.OpenReader();
            this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;
            FrameDescription colorFrameDescription = this.Sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            // 96 = résolution d'écran standard et PixelFormat.Bgra32 signifie que chaque pixel est représenté par 4 octets(8 bits pour bleu, vert, rouge et un octet d'opacité)
            this.bitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96, 96, PixelFormats.Bgra32, null);
        }

        override public void Stop()
        {
            base.Stop();
        }

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {

            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    // Create an array to store color data
                    byte[] colorData = new byte[colorFrameDescription.Width * colorFrameDescription.Height * colorFrameDescription.BytesPerPixel];

                    // Verify data and copy the new color frame data to the array
                    if ((colorFrameDescription.Width == this.bitmap.PixelWidth) && (colorFrameDescription.Height == this.bitmap.PixelHeight))
                    {
                        // CopyConvertedFrameDataToArray is used to get color data
                        //colorFrame.CopyConvertedFrameDataToArray(colorData, ColorImageFormat.Bgra);
                        
                        
                        //colorFrame.CopyRawFrameDataToIntPtr(colorData, ColorImageFormat.Bgra);   ++++ IL FAUT UTILISER CELA


                        this.bitmap.Lock();

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
