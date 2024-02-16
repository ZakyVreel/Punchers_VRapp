using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Gesture
{

    /// <summary>
    /// Classe abstraite de base pour la définition des gestes.
    /// </summary>
    public abstract class Gesture : BaseGesture
    {
        // Indique si le test du geste est en cours
        public bool IsTesting;

        // Nombre minimum de trames pour reconnaître un geste
        protected int MinNbOfFrames;

        // Nombre maximum de trames pour reconnaître un geste
        protected int MaxNbOfFrames;

        // Compteur de trames actuel
        protected int mCurrentFrameCount;

        // Méthode abstraite pour tester les conditions initiales du geste
        protected abstract bool TestInitialConditions(Body body);

        // Méthode abstraite pour tester la posture du geste
        protected abstract bool TestPosture(Body body);

        // Méthode abstraite pour tester la continuité du geste
        protected abstract bool TestRunningGesture(Body body);

        // Méthode abstraite pour tester les conditions de fin du geste
        protected abstract bool TestEndConditions(Body body);


        // Indique si le geste est reconnu
        private bool isGestureRecognized = false;


        // Méthode pour tester le geste
        public override void TestGesture(Body body)
        {
            if (IsTesting)
            {
                if (!isGestureRecognized)
                {
                    // Vérifier les conditions de fin du geste
                    if (TestEndConditions(body))
                    {
                        // Si le geste est terminé, déclencher l'événement de reconnaissance du geste
                        OnGestureRecognized();
                        IsTesting = false;
                    }
                    else if (!TestRunningGesture(body) && !TestPosture(body))
                    {
                        // Réinitialiser si les conditions du geste en cours ne sont pas remplies
                        IsTesting = false;
                    }
                }
                
            }
            // Si le test du geste n'est pas en cours et que les conditions initiales sont remplies
            else if (TestInitialConditions(body))
            {
                // Début du test du geste
                IsTesting = true;
                isGestureRecognized = false;

            }
        }
    }
}
