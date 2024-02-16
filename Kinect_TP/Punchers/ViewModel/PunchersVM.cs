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

        public IGestureFactory GestureFactory { get; set; }

        public BoxingGestureFactory BoxingGestureFactory { get; private set; }

        private readonly DispatcherTimer enemyChangeTimer = new DispatcherTimer();
        private readonly Random random = new Random();
        private readonly string[] enemyImagePaths = { "/images/enemy-punch1.png", "/images/enemy-punch2.png", "/images/enemy-block.png" };

        private int enemyLife = 100;
        public int EnemyLife
        {
            get { return enemyLife; }
            set { SetProperty(ref enemyLife, value); }
        }

        private int boxerLife = 100;
        public int BoxerLife
        {
            get { return boxerLife; }
            set { SetProperty(ref boxerLife, value); }
        }

        public ICommand StartAcqueringFramesCommand { get; private set; }
        public ICommand StopAcqueringFramesCommand { get; private set; }



        private string enemyImagePath = "/images/enemy-stand.png";
        public string EnemyImagePath
        {
            get { return enemyImagePath; }
            private set { SetProperty(ref enemyImagePath, value); }
        }

        private string boxerImagePath = "/images/boxer-stand.png";
        public string BoxerImagePath
        {
            get { return boxerImagePath; }
            private set { SetProperty(ref boxerImagePath, value); }
        }

        public Visibility StartTextVisibility { get; private set; }
        public Visibility BarEnemyVisibility { get; private set; }
        public Visibility BarBoxerVisibility { get; private set; }


        public PunchersVM() {
            KinectManager = new KinectManager();
            BoxingGestureFactory = new BoxingGestureFactory();

            this.GestureFactory = BoxingGestureFactory;

            GestureManager.AddGestures(BoxingGestureFactory);
            StartAcqueringFramesCommand = new RelayCommand(StartAcqueringFrames);
            StopAcqueringFramesCommand = new RelayCommand(StopAcqueringFrames);

            enemyChangeTimer.Interval = TimeSpan.FromSeconds(4); // Changer toutes les 4 secondes
            enemyChangeTimer.Tick += EnemyAttack_Tick;
            enemyChangeTimer.Start();

            GestureManager.GestureRecognized += GestureManager_GestureReco;

            StartTextVisibility = Visibility.Visible;
            BarEnemyVisibility = Visibility.Hidden;
            BarBoxerVisibility = Visibility.Hidden;
        }

        private void StartAcqueringFrames()
        {
            GestureManager.StartAcquiringFrames(KinectManager);
        }

        private void StopAcqueringFrames()
        {
            GestureManager.StopAcquiringFrame();
        }

        private async Task<string> ReadAGesture(int ReadTimeInSeconds)
        {
            string gestureRead = "";

            // load all 
            GestureManager.AddGestures(this.GestureFactory);

            // subscirbe to the OnGestureRecognized event 
            foreach (var gesture in GestureManager.KnownGestures)
            {
                gesture.GestureRecognized += (sender, args) =>
                {
                    // If new gesture read, replace th gesture name
                    if (gestureRead != args.GestureName)
                    {
                        gestureRead = args.GestureName;
                    }
                };
            }

            GestureManager.StartAcquiringFrames(GestureManager.KinectManager);
            await Task.Delay(ReadTimeInSeconds * 5000);

            GestureManager.StartAcquiringFrames(GestureManager.KinectManager);
            foreach (var gesture in GestureManager.KnownGestures)
            {
                gesture.GestureRecognized -= (sender, args) => { };
            }

            return gestureRead;
        }


        private void EnemyAttack_Tick(object sender, EventArgs e)
        {
            // Changer le chemin de l'image de l'adversaire aléatoirement
            int index = random.Next(enemyImagePaths.Length);
            EnemyImagePath = enemyImagePaths[index];

            // Vérifier si l'image du boxeur est "boxer_block"
            if (BoxerImagePath.Contains("boxer-block"))
            {
                // Ne déduire aucun point de vie du boxeur
            }
            else
            {
                // Si l'image de l'ennemi est un coup de poing, déduire 20 points de vie du boxerLife
                if (EnemyImagePath.Contains("enemy-punch"))
                {
                    BoxerLife -= 25;
                }
            }

            // Réinitialiser l'image de l'adversaire après un certain délai
            Task.Delay(500).ContinueWith(_ => {
                EnemyImagePath = "/images/enemy-stand.png";
            });
        }

        private void Attack()
        {
            // Changer l'image du boxeur pour l'attaque
            BoxerImagePath = "/images/boxer-right-punch.png";

            // Vérifier si l'image de l'adversaire est "enemy-block"
            if (EnemyImagePath.Contains("enemy-block"))
            {
                // Ne déduire aucun point de vie de l'adversaire
            }
            else
            {
                // Si l'image de l'adversaire est un coup de poing, déduire des points de vie de l'adversaire
                if (EnemyImagePath.Contains("enemy-punch"))
                {
                    EnemyLife -= 25;
                }
            }

            // Réinitialiser l'image du boxeur après un certain délai
            Task.Delay(500).ContinueWith(_ =>
            {
                BoxerImagePath = "/images/boxer-stand.png";
            });
        }


        private void GestureManager_GestureReco(object sender, GestureRecognizedEventArgs e)
        {
            switch (e.GestureName)
            {
                case "BoxePosture":
                    // Mettre à jour la visibilité du texte
                    StartTextVisibility = Visibility.Hidden;
                    break;
                case "Swipe Right Hand Gesture":
                    Attack();
                    break;
            }
        }
    }
}
