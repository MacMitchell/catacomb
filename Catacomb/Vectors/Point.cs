using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Vectors
{
    public class Point
    {

        private double x;
        private double y;
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
    }
}
