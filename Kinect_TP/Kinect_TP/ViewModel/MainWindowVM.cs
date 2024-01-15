using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinect_Utils;

namespace Kinect_TP.ViewModel
{ 
    public class MainWindowVM
    {

        public ICommand StartKinectCommand { get; set; }
        public ICommand StopKinectCommand { get; set; }
        public ICommand StartColorImageStreamCommand {  get; set; }
        public KinectManager KinectManager { get; set; }
        public ColorImageStream ColorImageStream { get; set; }

        public MainWindowVM()
        {
            KinectManager = new KinectManager();
            ColorImageStream = new ColorImageStream(KinectManager);
            StartKinectCommand = new RelayCommand(Start);
            StopKinectCommand = new RelayCommand(Stop);
            StartColorImageStreamCommand = new RelayCommand(StartColorImageStream);
        }

        private void Start()
        {
            KinectManager.StartSensor();
        }

        private void Stop()
        {
            KinectManager.StopSensor();
        }

        private void StartColorImageStream()
        {
            ColorImageStream.Start();
        }

    }
}
