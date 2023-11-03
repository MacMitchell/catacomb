using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Vectors
{
    public class CatRectangle : Vector
    {
        //reference the top left corner of the rectangle;
        private Point start;
        //references the bottom right corner of the rectangle
        private Point end;

        public Point Center
        {
            get
            {
                double x = (start.X + end.X) / 2;
                double y = (start.Y + end.Y) / 2;
                return new Point(x, y);
            }
        }


        public CatRectangle(Point start, Point end)
        {
            double minX = start.GetMinX(end);
            double minY = start.GetMinY(end);
            double maxX = start.GetMaxX(end);
            double maxY = start.GetMaxY(end);

            this.start = new Point(minX, minY);
            this.end = new Point(maxX, maxY);
        }

        public Point TopLeft
        {
            get { return start; }
        }
        public Point GetTopLeft()
        {
            return start;
        }


        public Point TopRight
        {
            get { return new Point(end.GetX(), start.GetY()); }
        }
        public Point GetTopRight()
        {
            return new Point(end.GetX(), start.GetY());
        }

        public Point BottomLeft
        {
            get { return new Point(start.GetX(), end.GetY()); }
        }

        public Point GetBottomLeft()
        {
            return new Point(start.GetX(), end.GetY());
        }

        public Point BottomRight
        {
            get { return end; }
        }
        public Point GetBottomRight()
        {
            return end;
        }

        public CatRectangle(double x1, double y1, double x2, double y2)
        {
            double minX = x1 > x2 ? x2 : x1;
            double minY = y1 > y2 ? y2 : y1;
            double maxX = x1 > x2 ? x1 : x2;
            double maxY = y1 > y2 ? y1 : y2;

            this.start = new Point(minX, minY);
            this.end = new Point(maxX, maxY);
        }

        public bool DoesIntersect(Vector other)
        {
            if (other.GetVectorType() == "Line")
            {
                return DoesIntersect((CatLine)other);
            }
            if(other.GetVectorType() == GetVectorType())
            {
                return DoesIntersect((CatRectangle)other);
            }
            return other.DoesIntersect(this);
        }


        public bool IsPointInRectangle(Point p)
        {

            return p.GetX() >= start.GetX() && p.GetX() <= end.GetX() &&
                p.GetY() >= start.GetY() && p.GetY() <= end.GetY();
        }

        public bool DoesIntersect(CatLine other)
        {
            //quick check
            Point otherStart = other.GetStartPoint();
            Point otherEnd = other.GetEndPoint();

            //quick check
            if (otherStart.GetMaxX(otherEnd) < start.GetX() || otherStart.GetMaxY(otherEnd) < start.GetY() ||
                otherStart.GetMinX(otherEnd) > end.GetX() || otherStart.GetMinY(otherEnd) > end.GetY())
            {
                return false;
            }
            if (IsPointInRectangle(otherStart) || IsPointInRectangle(otherEnd))
            {
                return true;
            }

            CatLine[] lines = { new CatLine(TopLeft, TopRight),new CatLine(TopLeft, BottomLeft), 
                                new CatLine(BottomLeft,BottomRight), new CatLine(TopRight,BottomRight)};
            foreach ( CatLine line in lines)
            {
                if (other.DoesIntersect(line))
                {
                    return true;
                }

            }
            return false;

            /*
            double[] yIntercepts = { start.GetY(), end.GetY() };
            double[] xIntercepts = { start.GetX(), end.GetX() };

            foreach (double db in yIntercepts)
            {
                Point interscept = other.GetPointWithYVal(db);
                if (interscept != null && interscept.GetX() >= start.GetX() && interscept.GetX() <= end.GetX())
                {
                    return true;
                }
            }

            foreach (double db in xIntercepts)
            {
                Point interscept = other.GetPointWithXVal(db);
                if (interscept != null && interscept.GetY() >= start.GetY() && interscept.GetY() <= end.GetY())
                {
                    return true;
                }
            }
             return false;
            */

        }



        public bool DoesIntersect(CatRectangle other)
        {
            Point otherStart = other.GetStartPoint();
            Point otherEnd = other.GetEndPoint();
            Point bottomLeft = new Point(otherStart.GetX(), otherEnd.GetY());
            Point topRight = new Point(otherEnd.GetX(), otherStart.GetY());

            Point[] points = { otherStart, otherEnd, bottomLeft, topRight };
            foreach (Point p in points)
            {
                if (IsPointInRectangle(p))
                {
                    return true;
                }
            }

            CatLine[] lines = { new CatLine(otherStart, topRight), new CatLine(topRight, otherEnd), new CatLine(otherEnd, bottomLeft), new CatLine(bottomLeft, otherStart) };
            foreach (CatLine l in lines)
            {
                if (DoesIntersect(l))
                {
                    return true;
                }
            }
            return false;
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
            return "Rectangle";
        }

        public bool IsWithin(Vector other)
        {
            if(other.GetVectorType() == "Line")
            {
                return IsWithin((CatLine)other);
            }
            return DoesIntersect(other);
        }

        public bool IsWithin(CatLine other)
        {
            if (IsPointInRectangle(other.Start)){
                return true;
            }
            if (IsPointInRectangle(other.End))
            {
                return true;
            }
            return DoesIntersect(other);
        }

        public override string ToString()
        {
            return "TopLeft: " + GetTopLeft().ToString() + ", TopRight: " + GetTopRight() +
                   "\nBottomLeft: " + GetBottomLeft() + ", BottomRight: " + GetBottomRight();
        }


        public double GetHeight()
        {
            return end.GetY() - start.GetY();
        }
        public double GetWidth()
        {
            return end.GetX() - start.GetX();
        }


        /**
         * 0: expand/shrink the top  
         * 1: expand/shrink the rightSide
         * 2: expand/shrink the bottom
         * 3: expand/shrink the leftside
         * 
         * 
         * Negative numbers will shrink the rectangle
         */
        public void expand(int direction, double distance)
        {
            
            switch (direction)
            {
                case 0: start = start.AddPoint(new Point(0, -distance)); break;
                case 1: end = end.AddPoint(new Point(distance, 0)); break;
                case 2: end = end.AddPoint(new Point(0, distance)); break;
                case 3: start = start.AddPoint(new Point(-distance, 0)); break;
            }

        }

        public void expandInAllDirection(double distance)
        {
            for(int i =0;  i < 4; i++)
            {
                expand(i, distance / 2);
            }
        }

        public void ShrinkInvasiveCatRectangle(CatRectangle other, Tuple<Point,Point> connectionPoints, double margin = 0) 
        {
            //First get direction this rectangle is to other
            double deltaX = Math.Abs(this.Center.X - other.Center.X);
            double deltaY = Math.Abs(this.Center.Y - other.Center.Y);
            //if(deltaY > deltaX || PassThroughVert(other))
            //TODO ASAP: Update this intersect so it will actual see if they intersect, not see if they are within
            if(new CatLine(other.GetTopLeft(),other.GetBottomLeft()).DoesIntersect(this) || new CatLine(other.GetTopRight(), other.GetBottomRight()).DoesIntersect(this))
            {
                if(connectionPoints.Item1.Y > Center.Y)
                {
                    double newY = Math.Abs(other.start.Y - (end.Y + margin));
                    other.expand(0, -newY);
                }
                else
                {
                    double newY =  other.end.Y - (start.Y + margin);
                    other.expand(2, -newY);
                }
            }
            else
            {
                if(connectionPoints.Item1.X < Center.X)
                {
                    double newX = (other.end.X + margin) - start.X;
                    other.expand(1, -newX);
                }
                else
                {
                    double newX = (end.X + margin) - other.start.X;
                    other.expand(3, -newX);
                }
            }
        }

        private bool PassThroughVert(CatRectangle other)
        {
            return other.start.Y < start.Y && end.Y < other.start.Y && other.end.Y > start.Y && other.end.Y > end.Y;
        }
        
    }
}
