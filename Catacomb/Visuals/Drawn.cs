using Catacomb.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Catacomb.Entities;
using System.Collections;



namespace Catacomb.Visuals
{
    public abstract class Drawn : Vector
    {
       
        protected List<Drawn> components;
        protected List<Interactable> interactables;
        protected Canvas canvas;
        public Vector representive;
        protected double scalar = 1;
        //The position is the top left of the entity
        private Point position;
        protected bool trespassable;

        protected Drawn parent = null;
        public virtual int DrawnId
        {
            get { return 0; }
        }
        public Vector Representive
        {
            get { return representive; }
        }
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
            components = new List<Drawn>();
            interactables = new List<Interactable>();
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
            other.parent = this;
            components.Add(other);
        }

        public void RemoveChild(Drawn other)
        {
            canvas.Children.Remove(other.GetCanvas());
            other.parent = null;
            components.Remove(other);
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
            return representive.DoesIntersect(other);
        }
        public virtual bool IsPointInVector(Point p)
        {
            return representive.IsPointInVector(p);
        }
        public virtual bool IsWithin(Drawn other)
        {
            
            return representive.DoesIntersect(other.representive);
        }
        public virtual void Draw()
        {
            Canvas.SetTop(canvas, position.GetY());
            Canvas.SetLeft(canvas, position.GetX());
        }

        public virtual bool EntityMove(Entity entityIn,double distance)
        {
            CatThickLine movementVector = entityIn.GetMovementVector(distance, -Position.GetX(), -Position.GetY());

            bool localIntersect = DoesEntityMoveIntersect(entityIn, distance);
            return !localIntersect;
        }

        /**
         * override this function if you want to change how a room intersects with an entity moving
         */
        public virtual bool DoesEntityMoveIntersect(Entity entityIn, double distance)
        {
            CatThickLine movementVector = entityIn.GetMovementVector(distance, -Position.GetX(), -Position.GetY());

            bool localIntersect = DoesIntersect(movementVector);
            return localIntersect;
        }

        public virtual bool Erase()
        {
            canvas.Children.Clear();
            components.Clear();
            interactables.Clear();
            return true;
        }

        public Drawn GetChildDrawnWithDrawnId(int drawnId)
        {
            for(int i =0; i < components.Count; i++)
            {
                Drawn child = (Drawn)components[i];
                if(child.DrawnId == drawnId)
                {
                    return child;
                }
            }
            return null;
        }
        public void AddInteractable(Interactable i)
        {
            interactables.Add(i);
            AddChild(i);
        }

        public void RemoveInteractable(Interactable i)
        {
            interactables.Remove(i);
            RemoveChild(i);
        }

        public void RemoveSelf()
        {
            parent.RemoveChild(this);
        }
        public void RemoveSelfInteractable()
        {
            parent.RemoveInteractable((Interactable)this);
        }


        public void Interact(Point p)
        {
            p = p.MinusPoint(Position);
            foreach (Interactable i in interactables)
            {
                Console.WriteLine(i.ToString());
                Console.WriteLine(p.ToString());
                if (i.CanInteract(p))
                {
                    i.Execute();
                    break;
                }
            }
        }
    }
}
