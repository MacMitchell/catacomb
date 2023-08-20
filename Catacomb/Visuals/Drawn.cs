﻿using Catacomb.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catacomb.Entities;
using System.Collections;

namespace Catacomb.Visuals
{
    public abstract class Drawn : Vector
    {
        protected ArrayList components;
        protected Canvas canvas;
        protected Vector representive;
        protected double scalar = 1;
        private Point position;
        private bool trespassable;
        public bool Trespassable
        {
            get { return trespassable; }
        }
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Drawn(Vector rep,bool trespass = false)
        {
            canvas = new Canvas();
            representive = rep;
            components = new ArrayList();
            Position = rep.GetStartPoint();
            trespassable = false;
                 
        }

        public double GetScalar()
        {
            return scalar;
        }

        public void SetScalar(double valIn)
        {
            scalar = valIn;
        }
        public virtual bool DoesIntersect(Vector other) 
        {
            
            if (!Trespassable && representive.DoesIntersect(other))
            {
                Console.WriteLine(GetVectorType());
                return true;
            }
            return false;
        }
          

        public virtual bool CheckComponentIntersect(Vector other)
        {
            for (int i = 0; i < components.Count; i++)
            {
                Drawn current = (Drawn)components[i];
                if (current.DoesIntersect(other))
                {

                    return true;
                }
            }
            return false;
        }
        public void AddChild(Drawn other)
        {
            canvas.Children.Add(other.GetCanvas());
            components.Add(other);
        }
        public Canvas GetCanvas()
        {
            return canvas;
        }

        public virtual void EntityEnter(Entity entIn)
        {
            return;
        }

        public virtual void EntityLeave(Entity entIn)
        {
            return;
        }

        public virtual Point GetEndPoint()
        {
            return representive.GetEndPoint();
        }

        public virtual Point GetStartPoint()
        {
            return representive.GetStartPoint();
        }

        public virtual string GetVectorType()
        {
            return "Drawn with Rep: "  + representive.GetVectorType();
        }

        public virtual bool IsWithin(Vector other)
        {
            return representive.IsWithin(other);
        }
        public virtual void Draw()
        {
            Console.WriteLine("BACKGROUND COLOR: " + canvas.Background);
            Canvas.SetTop(canvas, position.GetY());
            Canvas.SetLeft(canvas, position.GetX());
        }

        public virtual bool EntityMove(Entity entityIn,double distance)
        {

            CatThickLine movementVector = entityIn.GetMovementVector(distance, -Position.GetX(), -Position.GetY());
            return !DoesIntersect(movementVector);
            
        }
    }
}