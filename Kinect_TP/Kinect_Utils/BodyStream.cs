namespace Kinect_Utils
{
    using Kinect_TP;
    using Microsoft.Kinect;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Classe qui represente le flux d'image du corps pour la Kinect.
    /// </summary>
    public class BodyStream : KinectStream
    {
        //private const double HandSize = 30;
        private const double JointThickness = 3;
        //private const double ClipBoundsThickness = 10;
        private const float InferredZPositionClamp = 0.1f;


        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68)); //Couleur pour l'articulation
        private readonly Brush inferredJointBrush = Brushes.Yellow; //Couleur pour les membres inferieurs

        // regroupe plusieurs objets Drawing
        private DrawingGroup drawingGroup = new DrawingGroup();

        // La classe Body représente un objet qui contient des informations sur un corps humain détecté par le capteur Kinect
        private Body[] bodies;
        private List<Tuple<JointType, JointType>> bones;
        private int displayWidth;
        private int displayHeight;
        private List<Pen> bodyColors;

        //private KinectSensor kinectSensor;
        private CoordinateMapper coordinateMapper;
        private BodyFrameReader bodyFrameReader;
        private DrawingImage drawingImage;

        public event PropertyChangedEventHandler PropertyChanged;

        public override ImageSource ImageSource
        {
            get { return drawingImage; }
        }


        public BodyStream(KinectManager kinectSensor) : base(kinectSensor)
        {
            this.InitializeKinect();
            this.InitializeDrawingObjects();
        }

        /// <summary>
        /// Démarre la lecture du flux de corps.
        /// </summary>
        override public void Start()
        {
            this.StartProcessing();
        }

        /// <summary>
        /// Méthode surchargée de la classe de base, arrêtant le flux d'image couleur.
        /// </summary>
        override public void Stop()
        {
            this.StopProcessing();
        }

        private void InitializeKinect()
        {
            //this.Sensor = KinectSensor.GetDefault();
            // permet de convertir des coordonnées entre différents espaces, notamment entre l'espace de la caméra Kinect et autres
            this.coordinateMapper = this.Sensor.CoordinateMapper;
            // framedescription donne des informations sur une image
            //FrameDescription frameDescription = this.Sensor.DepthFrameSource.FrameDescription;
            FrameDescription frameDescription = this.Sensor.DepthFrameSource.FrameDescription;
            //changer le depth pour l'accorder au body stream et avoir un color + body stream propre
            this.displayWidth = frameDescription.Width;
            this.displayHeight = frameDescription.Height;
            // BodyFrameSource donne des données relatives au suivi du corps -> source de cadres avec des informations sur les mouvements / la posture des personnes détectées
            this.bodyFrameReader = this.Sensor.BodyFrameSource.OpenReader();

            this.bones = new List<Tuple<JointType, JointType>>();
            // Torso
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));

            this.bodyColors = new List<Pen>();
            this.bodyColors.Add(new Pen(Brushes.Red, 6));
            this.bodyColors.Add(new Pen(Brushes.Orange, 6));
            this.bodyColors.Add(new Pen(Brushes.Green, 6));
            this.bodyColors.Add(new Pen(Brushes.Blue, 6));
            this.bodyColors.Add(new Pen(Brushes.Indigo, 6));
            this.bodyColors.Add(new Pen(Brushes.Violet, 6));

            


        }

        private void InitializeDrawingObjects()
        {
            this.drawingGroup = new DrawingGroup();
            this.bodies = new Body[this.Sensor.BodyFrameSource.BodyCount];

            drawingImage = new DrawingImage(this.drawingGroup);
        }

        public void StartProcessing()
        {
            if (this.bodyFrameReader != null)
            {
                // est déclenché chaque fois qu'une nouvelle frame relatif au suivi du corps est disponible pour la lecture
                this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
            }
        }

        public void StopProcessing()
        {
            this.drawingImage.Drawing = null;
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived -= this.Reader_FrameArrived;
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }
        }

        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            // using pour garantir la libération des ressources associées au bodyframe
            // on récupère la frame du corps à partir de la référence fournie par l'événement
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    // si il y pas de body on set les bodies sinon on fait rien et on rafraichies juste la frame avec les nouveaux bodies
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                // ouvre un groupe de dessins
                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    // permet de dessiner un rectangle noir en fond, transparent pour qu'on puisse écrire dessus
                    // les 0 sont les valeurs de X et Y qu'on a pas besoin de définir
                    
                    //dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));

                    int penIndex = 0;
                    foreach (Body body in this.bodies)
                    {
                        // choisi un pen d'une couleur en fonction de l'index
                        Pen drawPen = this.bodyColors[penIndex++];

                        // vérifie sur le corps est actuellement suivi ou non
                        if (body.IsTracked)
                        {
                            //this.DrawClippedEdges(body, dc); //Bordures pour le carré

                            // pour avoir tout les joints du body
                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                            // dictionnaire pour stocker les joints mais en coordonnées 2D avec des points
                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                            // parcours type de joint
                            foreach (JointType jointType in joints.Keys)
                            {
                                // obtient position du joint en espace caméra
                                CameraSpacePoint position = joints[jointType].Position;
                                // vérifie si la profondeur du joint est inférieur à 0
                                if (position.Z < 0)
                                {
                                    //  ajuste la coordonnée Z (profondeur) d'un joint à la valeur claquée (InferredZPositionClamp) pour éviter les problèmes associés à la détection incorrecte ou indéterminée
                                    // peut etre enlevable ??
                                    position.Z = InferredZPositionClamp;
                                }
                                // convertis espace de caméra en coordonnées 2D
                                // detphspace contient coordonnées 2D avec 2 attributs, X et Y, ces 2 attributs vont permettre de construire le joint
                                DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                                jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                            }

                            this.DrawBody(joints, jointPoints, dc, drawPen);

                            //this.DrawHand(body.HandLeftState, jointPoints[JointType.HandLeft], dc);
                            //this.DrawHand(body.HandRightState, jointPoints[JointType.HandRight], dc);
                        }
                    }

                    this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
                }
            }

        }

        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {
            // Draw the bones
            foreach (var bone in this.bones)
            {
                //pour chaque bone, le dessiner
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
            }

            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                Brush drawBrush = null;
                TrackingState trackingState = joints[jointType].TrackingState;

                if (trackingState == TrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (trackingState == TrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    // on dessine une ellipse en fonction de si le joint est inferré ou non
                    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                }
            }
        }


        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext drawingContext, Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // Si nous ne pouvons pas trouver l'un des deux joints, on quitte
            if (joint0.TrackingState == TrackingState.NotTracked || joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // On suppose que tous les os dessinés sont inférés sauf si LES DEUX joints sont suivis
            //pour avoir un pen différent si l'os est inféré

            drawingContext.DrawLine(drawingPen, jointPoints[jointType0], jointPoints[jointType1]);
        }






    }
}

