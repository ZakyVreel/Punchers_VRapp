using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kinect_Gesture;
using Kinect_TP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Punchers.ViewModel
{
    public class PunchersVM : ObservableObject
    {
        public KinectManager KinectManager { get; private set; }
        public BoxingGestureFactory BoxingGestureFactory { get; private set; }

        public ICommand StartAcqueringFramesCommand { get; private set; }
        public ICommand StopAcqueringFramesCommand { get; private set; }

        //Le timer pour l'adversaire
        private readonly DispatcherTimer enemyTimer = new DispatcherTimer();
        private bool movingRight = true;

        private double enemyPositionX;

        private string enemyImagePath = "/images/enemy-stand.png";
        public string EnemyImagePath
        {
            get { return enemyImagePath; }
            private set { SetProperty(ref enemyImagePath, value); }
        }

        public Visibility StartTextVisibility { get; private set; }

        public PunchersVM() {
            KinectManager = new KinectManager();
            BoxingGestureFactory = new BoxingGestureFactory();

            GestureManager.AddGestures(BoxingGestureFactory);
            StartAcqueringFramesCommand = new RelayCommand(StartAcqueringFrames);
            StopAcqueringFramesCommand = new RelayCommand(StopAcqueringFrames);

            enemyTimer.Interval = TimeSpan.FromSeconds(1);
            enemyTimer.Tick += EnemyTimer_Tick;
            enemyTimer.Start();

            GestureManager.GestureRecognized += GestureManager_GestureReco;
            StartTextVisibility = Visibility.Visible;


        }

        private void StartAcqueringFrames()
        {
            GestureManager.StartAcquiringFrames(KinectManager);
        }

        private void StopAcqueringFrames()
        {
            GestureManager.StopAcquiringFrame();
        }

        private void EnemyTimer_Tick(object sender, EventArgs e)
        {
            // Déplacer l'adversaire
            if (movingRight)
            {
                enemyPositionX += 10; // Ajustez la vitesse de déplacement ici
            }
            else
            {
                enemyPositionX -= 10; // Ajustez la vitesse de déplacement ici
            }

            // Changer de direction si nécessaire
            if (enemyPositionX >= 800 - 50) // Largeur de la fenêtre - Largeur de l'adversaire
            {
                movingRight = false;
            }
            else if (enemyPositionX <= 0)
            {
                movingRight = true;
            }

            // Appliquez la nouvelle position
            //EnemyImagePath = $"/images/enemy-stand.png"; // Mettez à jour le chemin de l'image de l'adversaire
        }

        private void GestureManager_GestureReco(object sender, GestureRecognizedEventArgs e)
        {
            switch (e.GestureName)
            {
                case "BoxePosture":
                    // Mettre à jour la visibilité du texte
                    StartTextVisibility = Visibility.Hidden;
                    break;
            }
        }
    }
}
