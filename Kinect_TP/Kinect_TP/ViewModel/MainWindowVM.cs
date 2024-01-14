using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Kinect_TP.ViewModel
{ 
    public class MainWindowVM
    {
        public ICommand StartCommand { get; set; }
        public KinectManager KinectManager { get; set; }

        public MainWindowVM()
        {
            KinectManager = new KinectManager();
            StartCommand = new RelayCommand(Start);
        }

        private void Start()
        {
            KinectManager.StartSensor();
        }

    }
}
