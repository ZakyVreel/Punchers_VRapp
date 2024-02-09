using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinect_Gesture;
using Kinect_TP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Punchers.ViewModel
{
    public class PunchersVM : ObservableObject
    {
        public KinectManager KinectManager { get; private set; }
        public BoxingGestureFactory BoxingGestureFactory { get; private set; }

        public ICommand StartAcqueringFramesCommand { get; private set; }
        public ICommand StopAcqueringFramesCommand { get; private set; }




        public PunchersVM() {
            KinectManager = new KinectManager();
            BoxingGestureFactory = new BoxingGestureFactory();

            GestureManager.AddGestures(BoxingGestureFactory);
            StartAcqueringFramesCommand = new RelayCommand(StartAcqueringFrames);
            StopAcqueringFramesCommand = new RelayCommand(StopAcqueringFrames);

        }

        private void StartAcqueringFrames()
        {
            GestureManager.StartAcquiringFrames(KinectManager);
        }

        private void StopAcqueringFrames()
        {
            GestureManager.StopAcquiringFrame();
        }
    }
}
