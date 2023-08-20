using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Vectors
{
    public class CatThickLine : Vector
    {

        //for the names, imagine the line goes from left to right. 
        CatLine upperLine;
        CatLine lowerLine;
        Point start;
        
        Point end;

        public CatThickLine(Point p1, Point p2, double width)
        {
            width = width/ 2;
            start = p1.GetSmallerPoint(p2);
            end = p1.GetBiggerPoint(p2);

            //probably dont need to make a catline here, could just use the two points to get the angle. (shoulder shrug)
            CatLine center = new CatLine(start, end);
            double angle = center.GetAngle();


            upperLine = GetCatLineFromCenter(angle+Math.PI/2, width, center);
            lowerLine = GetCatLineFromCenter(angle - Math.PI / 2, width, center);
        }

        public bool DoesIntersect(Vector other)
        {
            Console.WriteLine("Upper: " + upperLine.ToString() + ", Lower: " + lowerLine.ToString());
            if (other.DoesIntersect(upperLine) || other.DoesIntersect(lowerLine)){
                Console.WriteLine("HERERERERERERERERERER");
                return true;
            }

            //creates lines from start and end points
            CatLine startLine = new CatLine(upperLine.GetStartPoint(), lowerLine.GetStartPoint());
            CatLine endLine = new CatLine(upperLine.GetEndPoint(), lowerLine.GetEndPoint());
            return (other.DoesIntersect(startLine)|| other.DoesIntersect(endLine));
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
            return "Thick_Line";
        }

        public bool IsWithin(Vector other)
        {
            return DoesIntersect(other);
        }

        //this name....
        private CatLine GetCatLineFromCenter(double angle, double distance,CatLine line)
        {
            Point start = line.GetStartPoint().GetRotateCopy(angle, distance);
            Point end = line.GetEndPoint().GetRotateCopy(angle, distance);
            return new CatLine(start, end);
        }
    }
}
