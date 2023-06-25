using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Global;
namespace Catacomb.Vectors
{
    public class CatLine : Vector
    {
        private Point start;
        private Point end;
        private double slope;
        public CatLine(Point start, Point end)
        {
            if(start.GetX() > end.GetX())
            {
                this.end = start;
                this.start = end;
            }
            else
            {
                this.start = start;
                this.end = end;
            }


            this.slope = (end.GetY() - start.GetY()) / (end.GetX() - start.GetX());
        }

        public CatLine(double x1, double y1, double x2, double y2)
        {
            if(x1 > x2)
            {
                this.start = new Point(x2, y2);
                this.end = new Point(x1, y1);
            }
            else
            {
                this.start = new Point(x1, y1);
                this.end = new Point(x2, y2);
            }

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
            
            return (DoesLineContainXPoint(intersection) && other.DoesLineContainXPoint(intersection));
        }

        private bool DoesLineContainXPoint(double xIn)
        {
            return (xIn + Globals.TOLERANCE >= start.GetMinX(end) && xIn- Globals.TOLERANCE <= start.GetMaxX(end));
        }
        public double GetIntersectPointX(CatLine other)
        {
            try
            {
                double slopeX = GetSlope() - other.GetSlope();
                double intercept = other.GetIntercept() - GetIntercept();
                double result = (double)intercept / (double)slopeX;
                return result;
            }
            catch(DivideByZeroException)
            {
                return -1;
            }
            
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
