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

        public Point Start
        {
            get { return start; }
        }
        public Point End
        {
            get { return end; }
        }
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


        }

        public double GetSlope()
        {
            return  (end.GetY() - start.GetY()) / (end.GetX() - start.GetX());
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
        }
        public bool DoesIntersect(Vector other)
        {
            if(other.GetVectorType() == "Line")
            {
                return DoesIntersect((CatLine)other);
            }

            return other.DoesIntersect(this);
        }


        public bool DoesIntersect(CatLine other)
        {
            if(Math.Abs(other.GetSlope() - GetSlope()) < Globals.TOLERANCE)
            {
                return false;
            }
            Point intersection;
            if (IsSlopeZero())
            {
                intersection = other.GetPointWithYVal(start.GetY());
            }
            else if (other.IsSlopeZero())
            {
                intersection = GetPointWithYVal(other.GetStartPoint().GetY());
            }
            else if (IsSlopeInifite())
            {
                intersection = other.GetPointWithXVal(start.GetX());
            }
            else if (IsSlopeInifite())
            {
                intersection = GetPointWithXVal(other.GetStartPoint().GetX());
            }
            else
            {
                intersection = GetIntersectPoint(other);
            }
            if(intersection == null)
            {
                return false;
            }
            return IsPointWithinRange(intersection) && other.IsPointWithinRange(intersection);
        }

       
        public bool IsPointInVector(Point p)
        {
            Point r1 = GetPointWithXVal(p.X);
            if(r1 == null)
            {
                return false;
            }
            Point r2 = GetPointWithYVal(p.Y);
            if(r2 == null)
            {
                return false;
            }
            return r2.AreEqual(r1);
        }

        public Point GetIntersectPoint(CatLine other)
        {
            try
            {
                double slopeX = GetSlope() - other.GetSlope();
                double intercept = other.GetIntercept() - GetIntercept();
                double result = (double)intercept / (double)slopeX;

                double resultY = result * GetSlope() + GetIntercept();
                return new Point(result, resultY); 
            }
            catch(DivideByZeroException)
            {
                return null;
            }
            
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

        public Point GetPointWithXVal(double xVal)
        {
            double slope;
            //checking for infinite slope
            if (IsSlopeInifite())
            {
                if (Globals.AreDoublesEqual(xVal, start.GetX()))
                {
                    return start;
                }
                return null;
            }

            double yVal;
            if (IsSlopeZero())
            {
                yVal = start.GetY();
            }
            else
            {
                slope = GetSlope();
                yVal = xVal * slope + GetIntercept();
            }
                if (!IsPointWithinRange(xVal,yVal))
            {
                return null;
            }
            return new Point(xVal,yVal);
        }

        public Point GetPointWithYVal(double yVal)
        {
            //checking for slope of 0
            if (IsSlopeZero())
            {
                if (Globals.AreDoublesEqual(yVal, start.GetY()))
                {
                    return start;
                }
                return null;
            }

            double xVal;
            if (IsSlopeInifite())
            {
                xVal = start.GetX();
            }
            else
            {
                xVal = ((double)yVal - GetIntercept()) / (double)GetSlope();
            }
            if (!IsPointWithinRange(xVal,yVal))
            {                
                return null;
            }
            return new Point(xVal, yVal);
        }

        override public string ToString()
        {
            return start.ToString() + " TO " + end.ToString();
        }
        public bool IsPointWithinRange(Point other)
        {
            return other.GetX() >= start.GetMinX(end) && other.GetX() <= start.GetMaxX(end) &&
                other.GetY() >= start.GetMinY(end) && other.GetY() <= start.GetMaxY(end);
        }
        public bool IsPointWithinRange(double x, double y)
        {
            return IsPointWithinRange(new Point(x, y));
        }

        public bool IsSlopeZero()
        {
            return Math.Abs(start.GetY() - end.GetY()) < Globals.TOLERANCE;
        }
        public bool IsSlopeInifite()
        {
            return Math.Abs(start.GetX() - end.GetX()) < Globals.TOLERANCE;
        }

        public double GetAngle()
        {
            //lines will always go left to right, so i dont have to worry about half of the trigometric circle

            double deltaX = end.GetX() - start.GetX();
            double deltaY = end.GetY() - start.GetY();

            double angle = Math.Atan(deltaY / deltaX);
            return angle;
        }
    }
}
