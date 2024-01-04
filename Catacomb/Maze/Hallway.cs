using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;
using Catacomb.Visuals;
using System.Windows.Media;
using Catacomb.Global;

namespace Catacomb.Maze
{
    public class Hallway : Room
    {

        public override double MaxWidth
        {
            get {return 100; }
        }

        public override double MaxHeight
        {
            get { return 100; }
        }

        public override double MinWidth
        {
            get { return 100; }
        }
        public override double MinHeight
        {
            get { return 100; }
        }
        public Hallway() : base() { }
        public override string GetRoomType()
        {
            return "Hallway";
        }

        public override string ToString()
        {
            return GetRoomType();
        }

        public override void Create(Point p1, Point p2)
        {
            roomDrawn = new DrawnHallway(this, p1, p2);
        }

        public override Room Clone()
        {
            return new Hallway();
        }

    }

    public class DrawnHallway : DrawnRoom
    {
        public DrawnHallway(Room parentIn, Point start, Point end) : base(parentIn,start, end)
        {
        }
        protected override void DrawRoom()
        {
            base.DrawRoom();
            //base.AddChild(new Wall(new Point(50, 50),new Point( 100, 50)));
            //base.AddChild(new Wall(new Point(75, 25),new Point( 75, 75)));
            return;

            CatRectangle rep = (CatRectangle)representive;
            double radiusOfRoom = 150.0/2.0;
            Point repCenter = convertPointToLocal(rep.Center);
            Point start = repCenter.MinusPoint(new Point(radiusOfRoom, radiusOfRoom));
            Point end = repCenter.AddPoint(new Point(radiusOfRoom, radiusOfRoom));


            rep = new CatRectangle(start, end);

            Point repEnd = convertPointToLocal(rep.BottomRight);
            //rep = new CatRectangle(new Point(0, 0), repEnd);
            CatRectangle horiWallRect = new CatRectangle(0 - Globals.LINE_THICKNESS / 2, 0, rep.GetWidth() + Globals.LINE_THICKNESS / 2, rep.GetHeight());
            CatRectangle vertWallRect = new CatRectangle(0, 0 - Globals.LINE_THICKNESS / 2, rep.GetWidth(), rep.GetHeight() + Globals.LINE_THICKNESS / 2);

            DrawConnection(rep.TopLeft, rep.TopRight, 0);
            DrawConnection(rep.BottomLeft, rep.BottomRight, 2);

            DrawConnection(rep.TopLeft, rep.BottomLeft, 3);
            DrawConnection(rep.TopRight, rep.BottomRight, 1);
            canvas.Background = Brushes.Blue;
            

        }

        /**
         *If a connection point exists on the other side, it creates the new one such that they connect 
         */
        protected override void CreateConnectionPoints(int index, Point start, Point end)
        {
            int oppositeDirection = Room.GetOppositeDirection(index);
            Tuple<Point, Point> oppositeConnection = GetLocalConnectionPoints(oppositeDirection);
            if (oppositeConnection == null)
            {
                base.CreateConnectionPoints(index, start, end);
                return;
            }
            //vertical door i.e. direction = 1 or 3 
            if(index % 2 != 0)
            {
                //dont change the y values of the connection points
                double x = start.X;

                double y1 = oppositeConnection.Item1.Y;
                double y2 = oppositeConnection.Item2.Y;

                Point p1 = new Point(x, y1);
                Point p2 = new Point(x, y2);
                connectionPoints[index] = new Tuple<Point, Point>(p1, p2);
            }
            else{
                double y = start.Y;

                double x1 = oppositeConnection.Item1.X;
                double x2 = oppositeConnection.Item2.X;

                Point p1 = new Point(x1, y);
                Point p2 = new Point(x2, y);
                connectionPoints[index] = new Tuple<Point, Point>(p1, p2);

            }
        }
    }
}
