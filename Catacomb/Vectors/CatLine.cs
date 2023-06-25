using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Global;
namespace Catacomb.Vectors
{
    class CatLine : Vector
    {
        private Point start;
        private Point end;
        private double slope;
        public CatLine(Point start, Point end)
        {
            this.start = start;
            this.end = end;

            this.slope = (end.GetY() - start.GetY()) / (end.GetX() - start.GetX());
        }

        public CatLine(double x1, double y1, double x2, double y2)
        {
            this.start = new Point(x1, y1);
            this.end = new Point(x2,y2);

            this.slope = (end.GetY() - start.GetY()) / (end.GetX() - start.GetX());
        }
        public bool DoesIntersect(Vector other)
        {
            if(other.GetVectorType() == "Line")
            {
                return DoesLineIntersect((CatLine)other);
            }

            return false;
        }


        public bool DoesLineIntersect(CatLine other)
        {
            if(Math.Abs(other.GetSlope() - GetSlope()) < Globals.TOLERANCE)
            {
                return false;
            }
            double intersection = GetIntersectPointX(other);
            Console.WriteLine("\nThis Intersection: " + GetIntercept() + ", OTHER: " + other.GetIntercept());
            Console.WriteLine("\n====================\nInterSect:  " + intersection);
            return (DoesLineContainXPoint(intersection) && other.DoesLineContainXPoint(intersection));
        }

        private bool DoesLineContainXPoint(double xIn)
        {
            return (xIn > start.GetMinX(end) && xIn < start.GetMaxX(end));
        }
        public double GetIntersectPointX(CatLine other)
        {
            double slopeX = GetSlope() - other.GetSlope();
            double intercept = other.GetIntercept() - GetIntercept();
            double result = (double)intercept / (double)slopeX;
            return result;
        }
        public double GetSlope()
        {
            return slope;
        }
        public Point GetEndPoint()
        {
            return end;
        }

        public Point GetStartPoint()
        {
            return start;
        }

        public string GetVectorType()
        {
            return "Line";
        }
        public bool IsWithin(Vector other)
        {
            return DoesIntersect(other);
        }

        public double GetIntercept()
        {
            return start.GetY() - ((double)start.GetX() * (double)GetSlope()); 
        }

        
    }
}
