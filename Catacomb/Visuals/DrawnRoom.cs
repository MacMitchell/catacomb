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
        private Tuple<Point, Point>[] connectionPoints;
        public DrawnRoom(Room parentIn, Point start, Point end) : base(new CatRectangle(start, end), true)
        {

            parent = parentIn;
            canvas = new Canvas();
            canvas.Width = start.GetMaxX(end) - start.GetMinX(end); //+ 5* Globals.LINE_THICKNESS;
            canvas.Height = start.GetMaxY(end) - start.GetMinY(end);// + 5* Globals.LINE_THICKNESS;
            canvas.Background = Globals.BACKGROUND_COLOR;
            rand = new Random();


            connectionPoints = new Tuple<Point, Point>[4];
            connectionPoints[0] = null;
            connectionPoints[1] = null;
            connectionPoints[2] = null;
            connectionPoints[3] = null;
            //TEMP
            CreateHorizontalWalls();
            CreateVerticalWalls();
            Draw();
        }

        private void CreateHorizontalWalls()
        {
            CatRectangle rep = (CatRectangle)representive;
            double doorLength = 50;

            CatRectangle wallRect = new CatRectangle(0 - Globals.LINE_THICKNESS / 2, 0, rep.GetWidth() + Globals.LINE_THICKNESS / 2, rep.GetHeight());
            //CatRectangle wallRect = new CatRectangle(0,0 , rep.GetWidth(), rep.GetHeight());


            Point[] startHoriPoints = { wallRect.GetTopLeft(), wallRect.GetBottomLeft() };
            Point[] endHoriPoints = { wallRect.GetTopRight(), wallRect.GetBottomRight() };
            for (int i = 0; i < startHoriPoints.Length; i++)
            {
                if (!parent.HasConnection(i * 2))
                {
                    base.AddChild(new Wall(startHoriPoints[i], endHoriPoints[i]));
                    continue;
                }

                Tuple<Point, Point> connectionPoints = GetConnectionPoints(i * 2, startHoriPoints[i], endHoriPoints[i]);

                base.AddChild(new Wall(startHoriPoints[i], connectionPoints.Item1));
                base.AddChild(new Wall(connectionPoints.Item2, endHoriPoints[i]));
            }
        }

        private void CreateVerticalWalls()
        {

            CatRectangle rep = (CatRectangle)representive;
            CatRectangle wallRect = new CatRectangle(0, 0 - Globals.LINE_THICKNESS / 2, rep.GetWidth(), rep.GetHeight() + Globals.LINE_THICKNESS / 2);
            //CatRectangle wallRect = new CatRectangle(0, 0,  rep.GetWidth(), rep.GetHeight());


            //TEMP
            double doorLength = 50;


            Point[] startVertPoints = { wallRect.GetTopRight(), wallRect.GetTopLeft() };
            Point[] endVertPoints = { wallRect.GetBottomRight(), wallRect.GetBottomLeft() };
            for (int i = 0; i < startVertPoints.Length; i++)
            {
                if (!parent.HasConnection((i * 2) + 1))
                {
                    base.AddChild(new Wall(startVertPoints[i], endVertPoints[i]));
                    continue;
                }
                Tuple<Point, Point> points = GetConnectionPoints(i * 2 + 1, startVertPoints[i], endVertPoints[i]);
                

                base.AddChild(new Wall(startVertPoints[i], points.Item1));
                base.AddChild(new Wall(points.Item2, endVertPoints[i]));
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



        //Assume the neighbor has connection points that are NOT null
        private Tuple<Point,Point> GetNeighborsConnectionPoints(int dir)
        {
            DrawnRoom neighbor = parent.GetConnectedRoom(dir).RoomDrawn;

            Tuple<Point,Point> neightborConnection = neighbor.GetConnectionPoints(Room.GetOppositeDirection(dir));
           
            //convert to room cordinates
            Point start = neightborConnection.Item1.MinusPoint(Position);
            Point end = neightborConnection.Item2.MinusPoint(Position);
            return new Tuple<Point, Point>(start, end);
        }

        private void CreateConnectionPoints(int index, Point start, Point end)
        {
            double doorLength = 50;
            if (index % 2 != 0)
            {
                double x = start.GetX();
                double distance = start.GetMaxY(end) - (start.GetMinY(end) + doorLength);

                double point1 = rand.NextDouble() * distance + start.GetMinY(end);
                double point2 = point1 + doorLength;
                connectionPoints[index] = new Tuple<Point, Point>(new Point(x, point1), new Point(x, point2));
            }
            else
            {
                double y = start.GetY();
                double distance = start.GetMaxX(end) - (start.GetMinX(end) + doorLength);

                double point1 = rand.NextDouble() * distance + start.GetMinX(end);
                double point2 = point1 + doorLength;
                connectionPoints[index] =  new Tuple<Point, Point>(new Point(point1, y), new Point(point2, y));
            }
        }

        private Tuple<Point, Point> GetConnectionPoints(int index, Point start, Point end)
        {
            CreateConnectionPoints(index, start, end);
            return connectionPoints[index];
        }

        /**
         * This returns the connection point in GLOBAL cordinates
         * */
        public Tuple<Point,Point> GetConnectionPoints(int index)
        {
            Tuple<Point,Point> localCordinates =  connectionPoints[index];
            Point current = base.Position;
            Point start = current.AddPoint(localCordinates.Item1);
            Point end = current.AddPoint(localCordinates.Item2);
            return new Tuple<Point, Point>(start, end);
        }
    }
}
