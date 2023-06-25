using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Vectors
{
    interface Vector
    {
        Point GetStartPoint();
        Point GetEndPoint();

        bool IsWithin(Vector other);
        bool DoesIntersect(Vector other);

        string GetVectorType();

    }
}
