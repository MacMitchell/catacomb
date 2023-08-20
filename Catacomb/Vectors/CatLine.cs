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
                Console.WriteLine("1");
            }
            else if (other.IsSlopeZero())
            {
                intersection = GetPointWithYVal(other.GetStartPoint().GetY());
                Console.WriteLine("2");
            }
            else if (IsSlopeInifite())
            {
                intersection = other.GetPointWithXVal(start.GetX());
                Console.WriteLine("3");
            }
            else if (IsSlopeInifite())
            {
                intersection = GetPointWithXVal(other.GetStartPoint().GetX());
                Console.WriteLine("4");
            }
            else
            {
                intersection = GetIntersectPoint(other);
                Console.WriteLine("5");
            }
            if(intersection == null)
            {
                return false;
            }
            Console.WriteLine(intersection.ToString());
            if(IsPointWithinRange(intersection) && other.IsPointWithinRange(intersection))
            {
                Console.WriteLine("HERERERER");
            }
            return IsPointWithinRange(intersection) && other.IsPointWithinRange(intersection);
        }

        private bool DoesLineContainXPoint(double xIn)
        {
            return (xIn + Globals.TOLERANCE >= start.GetMinX(end) && xIn- Globals.TOLERANCE <= start.GetMaxX(end));
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
