using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catacomb.Vectors;
using Catacomb.Visuals;

namespace Catacomb.Entities
{
    public abstract class Entity :Drawn
    {
        private double  velocity;
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

        private Drawn container;
        public Drawn Container
        {
            get { return container; }
            set { container = value; }
        }
        public Entity(Point positionIn, Vector rep): base(rep)
        {
            canvas = new Canvas();
            Velocity = 30;
            Angle = 0;
            Position = positionIn;
        }

        /**
         * THIS FUNCTION ASSUMES THAT THE ENTITY CAN MOVE
         */
        public virtual void move(double time)
        {
            double distance = velocity * time;
            double newX = (distance - representive.GetEndPoint().GetX() + representive.GetStartPoint().GetX()) * Math.Cos(angle) + Position.GetX();
            double newY = (distance - representive.GetEndPoint().GetY() + representive.GetStartPoint().GetY()) * Math.Sin(angle) + Position.GetY();

            
            Position = new Point(newX, newY);
            Console.WriteLine("New Position: " + Position.ToString());
            Draw();
        }

        public virtual void Move(double time)
        {
            double distance = time * velocity;
            if (container.EntityMove(this, distance))
            {
                move(time);
            
            }

        }

        public virtual CatThickLine GetMovementVector(double distance, double offsetX, double offsetY)
        {
            CatRectangle rep = (CatRectangle)representive;
            //assuming it is a square
            double width = rep.GetWidth();
            double newX = Position.GetX() + distance * Math.Cos(Angle);
            double newY = Position.GetY() + distance * Math.Sin(Angle);

            newX += offsetX;
            newY += offsetY;

            Point offsetPosition = Position.AddPoint(new Point(offsetX, offsetY));
            CatThickLine moveVector = new CatThickLine(offsetPosition, new Point(newX, newY), width);
            return moveVector;

        }

    }

    


}
