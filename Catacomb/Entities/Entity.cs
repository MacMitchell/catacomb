﻿using System;
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
        private double maxVelocity;
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

        private Drawn container;
        public Drawn Container
        {
            get { return container; }
            set { container = value; }
        }
        public Entity(Point positionIn, Vector rep): base(rep)
        {
            canvas = new Canvas();
            Velocity = 1;
            maxVelocity = 1;
            Angle = 0;
            Position = positionIn;
        }

        /**
         * THIS FUNCTION ASSUMES THAT THE ENTITY CAN MOVE
         */
        public virtual void MoveMe(double time)
        {
            double distance = velocity * time;
            double newX = (distance ) * Math.Cos(angle) + Position.GetX();
            double newY = (distance ) * Math.Sin(angle) + Position.GetY();

            Point temp = Position;
            Position = new Point(newX, newY);

            // Draw();
            Update();
        }

        public virtual void Move(double time)
        {
            double distance = time * velocity;
            if(container.EntityMove(this, distance))
            {
                MoveMe(time);
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
            Point center = new Point(Position.GetX() + width / 2, Position.GetY() + width / 2);
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
            base.Draw();
        }

        public virtual void Update() { }
    }

   


}
