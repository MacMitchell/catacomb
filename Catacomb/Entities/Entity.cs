using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Catacomb.Vectors;
using Catacomb.Visuals;
using Catacomb.CombatStuff;
namespace Catacomb.Entities
{
    public abstract class Entity :Drawn
    {
        private double  velocity;
        private double maxVelocity;
        protected CombatEntity fighter;
        private double accerlation;

        public int id;
        public static int idCounter = 0;
        public virtual CombatEntity Fighter
        {
            get { return fighter; }
            set { fighter = value; }
        }
        public double MaxVelocity
        {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }
        public double Velocity {
            get { return velocity; }
            set { velocity = value; }
        }
        private double angle;
        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        private Double? tempAngle;




        private double width = 25;
        private double height = 25;
        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        private DrawnRoom container;
        public DrawnRoom Container
        {
            get { return container; }
            set { container = value; }
        }

        public Point Center
        {
            get { return new Point(Position.X, Position.Y); }
            //get { return new Point(Position.GetX() + width / 2, Position.GetY() + width / 2); } 
        }

        public Point TopLeft
        {
            get { return Center.AddPoint( new Point(-Width / 2.0, -Height / 2.0)); }
        }
        public double Accerlation { get => accerlation; set => accerlation = value; }
        public Double? TempAngle { get => tempAngle; set => tempAngle = value; }

        public Entity(Point positionIn, Vector rep): base(rep)
        {
            canvas = new Canvas();
            Velocity = 1;
            maxVelocity = 1;
            Angle = 0;
            Position = positionIn;
            id = idCounter++;
            tempAngle = null;
        }


        /**
         * THIS FUNCTION ASSUMES THAT THE ENTITY CAN MOVE
         */
        public virtual void MoveMe(double time)
        {
            double distance = velocity * time;
            double newX;
            double newY;
            if (TempAngle == null)
            {
                newX = (distance) * Math.Cos(angle) + Position.GetX();
                newY = (distance) * Math.Sin(angle) + Position.GetY();
            }
            else
            {
                newX = (distance) * Math.Cos(TempAngle.Value) + Position.GetX();
                newY = (distance) * Math.Sin(TempAngle.Value) + Position.GetY();
            }
            Point temp = Position;
            Position = new Point(newX, newY);

            // Draw();
            Update();
        }
        /**
         * TODO: make it so the player moves in x and y direction seperate. 
         * EX: if there is a way to player right and they hold right and down, they should still move down
         * HOW TO IMPLEMENT (MAYBE): make two movement vectors, one for x movment, one for y movement
         */
        public virtual void Move(double time)
        {
            double distance = time * velocity;
            if(container.EntityMove(this, distance))
            {
                MoveMe(time);
                return;
            }
            
            double a = 0;
            double b = distance;
            double minTime = 0;
            double maxTime = time;
            for(int i =1; i <= 5; i++)
            {
                double c  = (a + b) / 2;
                double tempTime = (minTime + maxTime) / 2;
                if (container.EntityMove(this, c))
                {
                    a = c;
                    minTime = tempTime;
                }
                else
                {
                    b = c;
                    maxTime = tempTime;
                }
            }
            if (container.EntityMove(this, a))
            {
                MoveMe(minTime);
            }

            
            return;



            double correctTime = 0;


            //if they cant move, this shortens the movement and tries again.
            //TODO: make to programatically get the correct distance to move to instead of this
            for (int i=0; i < 10; i++)
            {
                double currentTime = time / 2;

                double currentDistance = distance / 2;
                if (container.EntityMove(this, currentDistance))
                {
                    correctTime = currentTime;
                    currentDistance *= 1.5;
                    currentTime *= 1.5;
                   
                }
                else
                {
                    currentDistance *= 0.5;
                    currentTime *= 0.5;
                }
            }
            correctTime -= 0.1;
            if(correctTime > 0)
            {
                MoveMe(correctTime);
            }

        }

        public virtual CatThickLine GetMovementVector(double distance, double offsetX, double offsetY)
        {
            CatRectangle rep = (CatRectangle)representive;
            //assuming it is a square
            double width = rep.GetWidth();
            Point center = Center;
            //Point center = new Point(Position.GetX() + width / 2, Position.GetY() + width / 2);
            center = center.GetRotateCopy(angle, width/2);
            double newX =   center.GetX() + distance * Math.Cos(Angle);
            double newY =   center.GetY() + distance * Math.Sin(Angle);

            

            newX += offsetX;
            newY += offsetY;

            Point offsetPosition = center.AddPoint(new Point(offsetX, offsetY));
            CatThickLine moveVector = new CatThickLine(offsetPosition, new Point(newX, newY), width);
            return moveVector;

        }

        public override void Draw()
        {
            Update();

            Canvas.SetTop(canvas, Position.GetY() - Height/2.0);
            Canvas.SetLeft(canvas, Position.GetX() - Width/2.0);
        }

        public void SetColor( IImmutableBrush color)
        {
            canvas.Background = color;
        }
        public virtual void Update()
        {
            double distanceToX = Width / 2;
            double distanceToY = Height / 2;
            representive = new CatRectangle(Position.GetX() - distanceToX, Position.GetY() - distanceToY, Position.GetX() + distanceToX, Position.GetY() + distanceToY);

            if(container == null)
            {
                return;
            }
            for(int i =0; i < Global.Globals.CONNECTION_LIMIT; i++)
            {
                if (!container.parent.HasConnection(i))
                {
                    continue;
                }
                CatRectangle rep = (CatRectangle)container.parent.GetConnectedRoom(i).RoomDrawn.representive;
                if (rep.IsPointInVector(Position))
                {
                    container = container.parent.GetConnectedRoom(i).RoomDrawn;
                }
            }
        }

        public virtual void Interact()
        {
            container.Interact(Center);
        }

        public virtual DrawnRoom GetCurrentRoom()
        {
            return container.GetEntityCurrentRoom(this); 
        }

        public bool equals(Entity other)
        {
            return id == other.id;
        }
    }
}
