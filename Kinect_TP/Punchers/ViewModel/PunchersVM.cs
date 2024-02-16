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

        private string textStart = "Boxe posture to start the game";
        public string TextStart
        {
            get { return textStart; }
            private set { SetProperty(ref textStart, value); }
        }

        public Visibility startTextVisibility;

        public Visibility StartTextVisibility
        {
            get { return startTextVisibility; }
            private set
            {
                SetProperty(ref startTextVisibility, value);
            }
        }
        public Visibility enemyVisibility;
        public Visibility EnemyVisibility
        {
            get { return enemyVisibility; }
            private set
            {
                SetProperty(ref enemyVisibility, value);
            }
        }
        public Visibility boxerVisibility;
        public Visibility BoxerVisibility
        {
            get { return boxerVisibility; }
            private set
            {
                SetProperty(ref boxerVisibility, value);
            }
        }


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
            EnemyVisibility = Visibility.Collapsed;
            BoxerVisibility = Visibility.Collapsed;
        }

        private void StartAcqueringFrames()
        {
            GestureManager.StartAcquiringFrames(KinectManager);
        }

        private void StopAcqueringFrames()
        {
            GestureManager.StopAcquiringFrame();
        }

        private void UpdateLifeStatus()
        {
            if (EnemyLife <= 0)
            {
                TextStart = "Vous avez gagné ! Boxe posture pour rejouer";
                StartTextVisibility = Visibility.Visible;
                EnemyVisibility = Visibility.Collapsed;
                BoxerVisibility = Visibility.Collapsed;
                boxerLife = 100;
                enemyLife = 100;

            }
            else if (BoxerLife <= 0)
            {
                TextStart = "L'adversaire a gagné ! Boxe posture pour rejouer";
                StartTextVisibility = Visibility.Visible;
                EnemyVisibility = Visibility.Collapsed;
                BoxerVisibility = Visibility.Collapsed;
                boxerLife = 100;
                enemyLife = 100;
            }
        }

        private void EnemyAttack_Tick(object sender, EventArgs e)
        {
            // Changer le chemin de l'image de l'adversaire aléatoirement
            int index = random.Next(enemyImagePaths.Length);
            EnemyImagePath = enemyImagePaths[index];

            // Vérifier si l'image du boxeur est "boxer_block"
            if (BoxerImagePath.Contains("boxer-block") || boxerVisibility == Visibility.Collapsed)
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
            Task.Delay(800).ContinueWith(_ => {
                EnemyImagePath = "/images/enemy-stand.png";
                UpdateLifeStatus();
            });
        }

        private void BoxerAttack_Tick()
        {
            // Changer l'image du boxeur pour l'attaque
            BoxerImagePath = "/images/boxer-right-punch.png";

            // Vérifier si l'image de l'adversaire est "enemy-block"
            if (EnemyImagePath.Contains("enemy-block") || enemyVisibility == Visibility.Collapsed)
            {
                // Ne déduire aucun point de vie de l'adversaire
            }
            else
            {
                // Si l'image de l'adversaire est un coup de poing, déduire des points de vie de l'adversaire
                if (BoxerImagePath.Contains("boxer-right-punch") && !EnemyImagePath.Contains("enemy-stand"))
                {
                    EnemyLife -= 25;
                }
            }

            // Réinitialiser l'image du boxeur après un certain délai
            Task.Delay(800).ContinueWith(_ =>
            {
                BoxerImagePath = "/images/boxer-stand.png";
                UpdateLifeStatus();
            });
        }

        private void Block_Tick()
        {
            // Changer l'image du boxeur pour l'attaque
            BoxerImagePath = "/images/boxer-block.png";

            // Réinitialiser l'image du boxeur après un certain délai
            Task.Delay(800).ContinueWith(_ =>
            {
                BoxerImagePath = "/images/boxer-stand.png";
                UpdateLifeStatus();
            });
        }


        private void GestureManager_GestureReco(object sender, GestureRecognizedEventArgs e)
        {
            switch (e.GestureName)
            {
                case "BoxePosture":
                    // Mettre à jour la visibilité du texte
                    StartTextVisibility = Visibility.Collapsed;
                    EnemyVisibility = Visibility.Visible;
                    BoxerVisibility = Visibility.Visible;
                    break;

                case "Swipe Right Hand Gesture":
                    BoxerAttack_Tick();
                    break;
                case "BlockPosture":
                    Block_Tick();
                    break;
            }
        }
    }
}
