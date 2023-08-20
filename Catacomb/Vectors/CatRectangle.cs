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


        public CatRectangle(Point start, Point end)
        {
            double minX = start.GetMinX(end);
            double minY = start.GetMinY(end);
            double maxX = start.GetMaxX(end);
            double maxY = start.GetMaxY(end);

            this.start = new Point(minX, minY);
            this.end = new Point(maxX, maxY);
        }

        public Point GetTopLeft()
        {
            return start;
        }

        public Point GetTopRight()
        {
            return new Point(end.GetX(), start.GetY());
        }

        public Point GetBottomLeft()
        {
            return new Point(start.GetX(), end.GetY());
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
    }
}
