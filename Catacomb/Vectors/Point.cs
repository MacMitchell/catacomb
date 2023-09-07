using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Global;

namespace Catacomb.Vectors
{
    public class Point
    {

        private double x;
        private double y;

        public double X
        {
            get { return X; }
        }

        public double Y
        {
            get { return Y; }
        }
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double GetX()
        {
            return x;
        }
        public double GetY()
        {
            return y;
        }
        public void UpdateX(double  x)
        {
            this.x = x;
        }
        public void UpdateY(double y)
        {
            this.y = y;
        }
        public double GetDistance(Point other)
        {
            double dX = Math.Pow(this.x - other.x, 2);
            double dY = Math.Pow(this.y - other.y, 2);
            double distance = Math.Sqrt(dX + dY);
            return distance;
        }

        public double GetMinX(Point other)
        {
            double result = GetX() < other.GetX() ? GetX() : other.GetX();
            return result;
        }
        public double GetMaxX(Point other)
        {
            double result = GetX() > other.GetX() ? GetX() : other.GetX();
            return result;
        }
        public double GetMinY(Point other)
        {
            double result = GetY() < other.GetY() ? GetY() : other.GetY();
            return result;
        }
        public double GetMaxY(Point other)
        {
            double result = GetY() > other.GetY() ? GetY() : other.GetY();
            return result;
        }

        public bool AreEqual(Point other)
        {
            return (GetX() - other.GetX()) < Globals.TOLERANCE && (GetY() - other.GetY()) < Globals.TOLERANCE;
        }

        override public String ToString()
        {
            return "(" + GetX() + ", " + GetY() + ")";
        }


        public int compare(Point other)
        {
            double compareThis = GetX();
            double compareOther = other.GetX();
            if (Math.Abs(this.GetX() - other.GetX()) < Globals.TOLERANCE)
            {
                compareThis = this.GetY();
                compareOther = other.GetY();
            }
            if(Math.Abs(compareThis - compareOther) < Globals.TOLERANCE)
            {
                return 0;
            }
            else if(compareThis > compareOther)
            {
                return 1;
            }
            else
            {
                return -1;
            }
            
        }
        public Point GetSmallerPoint(Point other)
        {
            int compare = this.compare(other);
            if(compare > 0)
            {
                return other;
            }

            return this;
        }

        public Point GetBiggerPoint(Point other)
        {
            int compare = this.compare(other);
            if(compare < 0)
            {
                return other;
            }
            return this;
        }

        public Point GetRotateCopy(double angle, double distance)
        {
            double newX = GetX() + (distance * Math.Cos(angle ));
            double newY = GetY() + (distance * Math.Sin(angle ));

            return new Point(newX, newY);
        }

        public Point MinusPoint(Point other)
        {
            double newX = GetX() - other.GetX();
            double newY = GetY() - other.GetY();
            return new Point(newX, newY);

        }

        public Point AddPoint(Point other)
        {
            double newX = GetX() + other.GetX();
            double newY = GetY() + other.GetY();
            return new Point(newX, newY);
        }
    }
}
