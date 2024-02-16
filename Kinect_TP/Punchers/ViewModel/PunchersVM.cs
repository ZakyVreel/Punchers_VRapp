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

    /// <summary>
    /// ViewModel pour la fenêtre principale du jeu Punchers.
    /// </summary>
    public class PunchersVM : ObservableObject
    {
        // Gestionnaire Kinect
        public KinectManager KinectManager { get; private set; }

        // Fabrique de gestes
        public IGestureFactory GestureFactory { get; set; }

        // Fabrique de gestes pour la boxe
        public BoxingGestureFactory BoxingGestureFactory { get; private set; }

        // Minuterie pour le changement d'image de l'adversaire
        private readonly DispatcherTimer enemyChangeTimer = new DispatcherTimer();

        // Générateur de nombres aléatoires
        private readonly Random random = new Random();

        // Chemins des images de l'adversaire
        private readonly string[] enemyImagePaths = { "/images/enemy-punch1.png", "/images/enemy-punch2.png", "/images/enemy-block.png" };

        // Points de vie de l'adversaire
        private int enemyLife = 100;
        public int EnemyLife
        {
            get { return enemyLife; }
            set { SetProperty(ref enemyLife, value); }
        }


        // Points de vie du boxeur
        private int boxerLife = 100;
        public int BoxerLife
        {
            get { return boxerLife; }
            set { SetProperty(ref boxerLife, value); }
        }

        // Commandes pour démarrer et arrêter l'acquisition de trames Kinect
        public ICommand StartAcqueringFramesCommand { get; private set; }
        public ICommand StopAcqueringFramesCommand { get; private set; }


        // Chemin de l'image de l'adversaire
        private string enemyImagePath = "/images/enemy-stand.png";
        public string EnemyImagePath
        {
            get { return enemyImagePath; }
            private set { SetProperty(ref enemyImagePath, value); }
        }

        // Chemin de l'image du boxeur
        private string boxerImagePath = "/images/boxer-stand.png";
        public string BoxerImagePath
        {
            get { return boxerImagePath; }
            private set { SetProperty(ref boxerImagePath, value); }
        }

        // Texte de démarrage du jeu
        private string textStart = "Boxe posture to start the game";
        public string TextStart
        {
            get { return textStart; }
            private set { SetProperty(ref textStart, value); }
        }

        // Visibilité du texte de démarrage
        public Visibility startTextVisibility;
        public Visibility StartTextVisibility
        {
            get { return startTextVisibility; }
            private set
            {
                SetProperty(ref startTextVisibility, value);
            }
        }

        // Visibilité de l'adversaire
        public Visibility enemyVisibility;
        public Visibility EnemyVisibility
        {
            get { return enemyVisibility; }
            private set
            {
                SetProperty(ref enemyVisibility, value);
            }
        }

        // Visibilité du boxeur
        public Visibility boxerVisibility;
        public Visibility BoxerVisibility
        {
            get { return boxerVisibility; }
            private set
            {
                SetProperty(ref boxerVisibility, value);
            }
        }

        // Constructeur de la classe
        public PunchersVM() {

            // Initialisation du gestionnaire Kinect
            KinectManager = new KinectManager();
            BoxingGestureFactory = new BoxingGestureFactory();
            this.GestureFactory = BoxingGestureFactory;
            GestureManager.AddGestures(BoxingGestureFactory);

            // Initialisation des commandes
            StartAcqueringFramesCommand = new RelayCommand(StartAcqueringFrames);
            StopAcqueringFramesCommand = new RelayCommand(StopAcqueringFrames);

            // Configuration de la minuterie pour le changement d'image de l'adversaire
            enemyChangeTimer.Interval = TimeSpan.FromSeconds(4); // Changer toutes les 4 secondes
            enemyChangeTimer.Tick += EnemyAttack_Tick;
            enemyChangeTimer.Start();

            GestureManager.GestureRecognized += GestureManager_GestureReco;

            // Configuration des visibilités par défaut
            StartTextVisibility = Visibility.Visible;
            EnemyVisibility = Visibility.Collapsed;
            BoxerVisibility = Visibility.Collapsed;
        }

        // Méthode pour démarrer l'acquisition de trames Kinect
        private void StartAcqueringFrames()
        {
            GestureManager.StartAcquiringFrames(KinectManager);
        }

        // Méthode pour arrêter l'acquisition de trames Kinect
        private void StopAcqueringFrames()
        {
            GestureManager.StopAcquiringFrame();
        }

        // Méthode pour mettre à jour l'état de vie des joueurs
        private void UpdateLifeStatus()
        {
            if (EnemyLife <= 0)
            {
                TextStart = "Vous avez gagné ! Boxe posture pour rejouer";

                //On resete le jeu
                StartTextVisibility = Visibility.Visible;
                EnemyVisibility = Visibility.Collapsed;
                BoxerVisibility = Visibility.Collapsed;
                boxerLife = 100;
                enemyLife = 100;

            }
            else if (BoxerLife <= 0)
            {
                TextStart = "L'adversaire a gagné ! Boxe posture pour rejouer";

                //On resete le jeu
                StartTextVisibility = Visibility.Visible;
                EnemyVisibility = Visibility.Collapsed;
                BoxerVisibility = Visibility.Collapsed;
                boxerLife = 100;
                enemyLife = 100;
            }
        }

        // Méthode pour gérer l'attaque de l'adversaire
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

        // Méthode pour gérer l'attaque du boxeur
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

            // Réinitialiser l'image du boxeur après un certain délai et mettre à jour l'état de vie
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


        // Gestionnaire d'événement pour la reconnaissance des gestes
        private void GestureManager_GestureReco(object sender, GestureRecognizedEventArgs e)
        {
            switch (e.GestureName)
            {
                case "BoxePosture":
                    // Mettre à jour la visibilité du texte et des joueurs
                    StartTextVisibility = Visibility.Collapsed;
                    EnemyVisibility = Visibility.Visible;
                    BoxerVisibility = Visibility.Visible;
                    break;

                case "Swipe Right Hand Gesture":
                    // Gérer l'attaque du boxeur
                    BoxerAttack_Tick();
                    break;
                case "BlocPosture":
                    // Gérer la posture de blocage du boxeur
                    Block_Tick();
                    break;
            }
        }
    }
}
