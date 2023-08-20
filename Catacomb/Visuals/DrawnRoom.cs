using Catacomb.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;
using System.Windows.Controls;
using Catacomb.Global;
using System.Collections;

namespace Catacomb.Visuals
{
    public class DrawnRoom : Drawn
    {
        private static Random rand;
        private Room parent;
        public DrawnRoom(Room parentIn,Point start, Point end):base(new CatRectangle(start,end),true)
        {
            parent = parentIn;
            canvas = new Canvas();
            canvas.Width = start.GetMaxX(end) - start.GetMinX(end); //+ 5* Globals.LINE_THICKNESS;
            canvas.Height = start.GetMaxY(end) - start.GetMinY(end);// + 5* Globals.LINE_THICKNESS;
            canvas.Background = Globals.BACKGROUND_COLOR;
            rand = new Random();


            
            //TEMP
            CreateHorizontalWalls();
            CreateVerticalWalls();
            Draw();
        }

        private  void CreateHorizontalWalls()
        {
            CatRectangle rep = (CatRectangle)representive;
            double doorLength = 50;

            //CatRectangle wallRect = new CatRectangle(0 - Globals.LINE_THICKNESS / 2,0 , rep.GetWidth() + Globals.LINE_THICKNESS / 2, rep.GetHeight());
            CatRectangle wallRect = new CatRectangle(0,0 , rep.GetWidth(), rep.GetHeight());


            Point[] startHoriPoints = { wallRect.GetTopLeft(), wallRect.GetBottomLeft() };
            Point[] endHoriPoints = { wallRect.GetTopRight(), wallRect.GetBottomRight() };
            for (int i = 0; i < startHoriPoints.Length; i++)
            {
                if (!parent.HasConnection(i * 2))
                {
                    base.AddChild(new Wall(startHoriPoints[i], endHoriPoints[i]));
                    continue;
                }
                double y = startHoriPoints[i].GetY();
                double distance = endHoriPoints[i].GetX() - (startHoriPoints[i].GetX() + doorLength);

                double point1 = rand.NextDouble() * distance + startHoriPoints[i].GetX();
                double point2 = point1 + doorLength;

                Point midPoint1 = new Point(point1, y);
                Point midPoint2 = new Point(point2, y);

                base.AddChild(new Wall(startHoriPoints[i], midPoint1));
                base.AddChild(new Wall(midPoint2, endHoriPoints[i]));
            }
        }

        private void CreateVerticalWalls()
        {

            CatRectangle rep = (CatRectangle)representive;
            //CatRectangle wallRect = new CatRectangle(0, 0 - Globals.LINE_THICKNESS / 2, rep.GetWidth(), rep.GetHeight() + Globals.LINE_THICKNESS / 2);
            CatRectangle wallRect = new CatRectangle(0, 0,  rep.GetWidth(), rep.GetHeight());


            //TEMP
            double doorLength = 50;


            Point[] startVertPoints = { wallRect.GetTopRight(), wallRect.GetTopLeft() };
            Point[] endVertPoints = { wallRect.GetBottomRight(), wallRect.GetBottomLeft() };
            for (int i = 0; i < startVertPoints.Length; i++)
            {
                if (!parent.HasConnection((i * 2)+1))
                {
                    base.AddChild(new Wall(startVertPoints[i], endVertPoints[i]));
                    continue;
                }
                double x = startVertPoints[i].GetX();
                double distance = startVertPoints[i].GetMaxY(endVertPoints[i]) - (startVertPoints[i].GetMinY(endVertPoints[i]) + doorLength);

                double point1 = rand.NextDouble() * distance + startVertPoints[i].GetMinY(endVertPoints[i]);
                double point2 = point1 + doorLength;

                Point midPoint1 = new Point(x, point1);
                Point midPoint2 = new Point(x, point2);

                base.AddChild(new Wall(startVertPoints[i],midPoint1));
                base.AddChild(new Wall(midPoint2, endVertPoints[i]));
            }
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override bool DoesIntersect(Vector other)
        {
            return CheckComponentIntersect(other);
        }
        public override string GetVectorType()
        {
            return "DrawnRoom with Rep: " + representive.GetVectorType();
        }
    }
}
