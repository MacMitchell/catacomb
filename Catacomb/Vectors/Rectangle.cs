using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Vectors
{
    class Rectangle : Vector
    {
        private Point start;
        private Point end;

        public Rectangle(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

        public Rectangle(double x1, double y1, double x2, double y2)
        {
            this.start = new Point(x1, y1);
            this.end = new Point(x2, y2);
        }

        public bool DoesIntersect(Vector other)
        {
            return false;
        }

        public Point GetEndPoint()
        {
            throw new NotImplementedException();
        }

        public Point GetStartPoint()
        {
            throw new NotImplementedException();
        }

        public string GetVectorType()
        {
            return "Rectangle";
        }

        public bool IsWithin(Vector other)
        {
            throw new NotImplementedException();
        }
    }
}
