using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Vectors
{
    public interface Vector
    {
        Point GetStartPoint();
        Point GetEndPoint();

        double OffsetX { get; set; }
        double OffsetY { get; set; }
        bool IsWithin(Vector other);
        bool DoesIntersect(Vector other);

        bool IsPointInVector(Point p);
        string GetVectorType();

        Vector Clone();

    }
}
