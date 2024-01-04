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
using Catacomb.Entities;

namespace Catacomb.Visuals
{
    public class DrawnRoom : Drawn
    {
        private static Random rand;
        protected Room parent;
        public Tuple<Point, Point>[] connectionPoints;
        private Tuple<Point,Point> originalRep;

        public String testString = "";
        public double Width
        {
            get { return ((CatRectangle)base.representive).GetWidth(); }
        }

        public double Height
        {
            get { return ((CatRectangle)base.representive).GetHeight(); }
        }
        public DrawnRoom(Room parentIn, Point start, Point end) : base(new CatRectangle(start, end), true)
        {
            originalRep = new Tuple<Point, Point>(start, end);
            parent = parentIn;
            canvas = new Canvas();
            canvas.Width = start.GetMaxX(end) - start.GetMinX(end); //+ 5* Globals.LINE_THICKNESS;
            canvas.Height = start.GetMaxY(end) - start.GetMinY(end);// + 5* Globals.LINE_THICKNESS;
            canvas.Background = Globals.BACKGROUND_COLOR;
            rand = new Random();


            connectionPoints = new Tuple<Point, Point>[Globals.CONNECTION_LIMIT];
            for(int i =0; i < Globals.CONNECTION_LIMIT; i++)
            {
                connectionPoints[i] = null;
            }
            testString  = parent.getId().ToString()+": " + parent.ToString() + "\n" + representive.ToString();

        }


        protected void DrawConnection(Point start, Point end, int direction)
        {
            if (!parent.HasConnection(direction))
            {
                base.AddChild(new Wall(start, end));
                return;
            }

            DrawnRoom otherRoom = parent.GetConnectedRoom(direction).RoomDrawn;
            int oppositeDirection = Room.GetOppositeDirection(direction);
            
            Tuple<Point, Point> localConnectionPoints = GetLocalConnectionPoints(direction);

            //if(otherRoom == null || !otherRoom.HasConnectionPoints(oppositeDirection))
            if(localConnectionPoints == null)
            {
                //Tuple<Point, Point> connectionPoints = GetConnectionPoints(direction, start, end);
                CreateConnectionPoints(direction, start, end);
                localConnectionPoints = connectionPoints[direction]; 
                base.AddChild(new Wall(start, localConnectionPoints.Item1));
                base.AddChild(new Wall(localConnectionPoints.Item2, end));
            }
            else
            {
                //Tuple<Point, Point> connectionPoints = GetNeighborsConnectionPoints(direction);

                Point p1;
                Point p4;
                double distance = 0;

                if(direction %2 == 0)
                {
                    p1 = new Point(localConnectionPoints.Item1.X, start.Y);
                    p4 = new Point(localConnectionPoints.Item2.X, end.Y);
                    distance = Math.Abs(start.Y - localConnectionPoints.Item1.Y);
                }
                else
                {
                    p1 = new Point(start.X, localConnectionPoints.Item1.Y);
                    p4 = new Point(end.X, localConnectionPoints.Item2.Y);
                    distance = Math.Abs(start.X - localConnectionPoints.Item1.X);

                }
                Point p2 = (localConnectionPoints.Item1);

                Point p3 = localConnectionPoints.Item2;
                

                ((CatRectangle) representive).expand(direction, distance);


                base.AddChild(new Wall(start, p1));
                base.AddChild(new Wall(p1, p2));


                base.AddChild(new Wall(p3, p4));
                base.AddChild(new Wall(p4, end));

            }
        }

        /**
         * This function make a new wall connecting the connection points in the given direction
         * This is used when you dont want the player to go the room in the given direction
         * @param direction it is the direction relative to this room
         */
        public void CloseConnectionPoints(int direction)
        {
            Tuple<Point, Point> connectionPoint = connectionPoints[direction];
            base.AddChild(new Wall(connectionPoint.Item1, connectionPoint.Item2));
        }
        public override void Draw()
        {
            DrawRoom();

            if (Globals.DEBUG)
            {
                TextBox testBox = new TextBox();
                testBox.Text = testString;//parent.getId().ToString() + "\n" + representive.ToString();
                canvas.Children.Add(testBox);
            }
            
            base.Draw();

        }

        protected virtual void DrawRoom()
        {
            DrawRep();
        }

        protected virtual void DrawRep()
        {
            CatRectangle rep = (CatRectangle)representive;
            
            CatRectangle horiWallRect = new CatRectangle(0 - Globals.LINE_THICKNESS / 2, 0, rep.GetWidth() + Globals.LINE_THICKNESS / 2, rep.GetHeight());
            CatRectangle vertWallRect = new CatRectangle(0, 0 - Globals.LINE_THICKNESS / 2, rep.GetWidth(), rep.GetHeight() + Globals.LINE_THICKNESS / 2);


            DrawConnection(horiWallRect.TopLeft, horiWallRect.TopRight, 0);
            DrawConnection(horiWallRect.BottomLeft, horiWallRect.BottomRight, 2);

            DrawConnection(vertWallRect.TopLeft, vertWallRect.BottomLeft, 3);
            DrawConnection(vertWallRect.TopRight, vertWallRect.BottomRight, 1);
        }
        public override bool DoesIntersect(Vector other)
        {
            return CheckComponentIntersect(other);
        }
        public override string GetVectorType()
        {
            return "DrawnRoom with Rep: " + representive.GetVectorType();
        }

        
        
        /**
         * This just checks to see if this room has connection points
         * It is mostly used to check if a neightbor has connection points before grabbing them
         */
        private bool HasConnectionPoints(int dir)
        {
            bool result = connectionPoints[dir] != null;
            return result;
        }

        /**
         * Returns the neighbors connection points in refernce the current room
         * The direction is in reference to THIS room
        * Assume the neighbor has connection points that are NOT null
        */
        private Tuple<Point,Point> GetNeighborsConnectionPoints(int dir)
        {
            DrawnRoom otherRoom = parent.GetConnectedRoom(dir).RoomDrawn;

            Tuple<Point,Point> neightborConnection = otherRoom.GetConnectionPoints(Room.GetOppositeDirection(dir));
           
            //convert to room cordinates
            Point start = neightborConnection.Item1.MinusPoint(Position);
            Point end = neightborConnection.Item2.MinusPoint(Position);

            connectionPoints[dir] = new Tuple<Point, Point>(start, end);
            return new Tuple<Point, Point>(start, end);
        }


        /**
         * Creates connection points in the given range 
         */
        protected virtual void CreateConnectionPoints(int index, Point start, Point end)
        {
            double doorLength = 100;
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
        public Tuple<Point,Point> GetLocalConnectionPoints(int direction)
        {
            //if no connection return null
            if (!parent.HasConnection(direction))
            {
                return null;
            }
            if(connectionPoints[direction] != null)
            {
                return connectionPoints[direction];
            }

            int oppositeDirection = Room.GetOppositeDirection(direction);
            DrawnRoom neighbor = parent.GetConnectedRoom(direction).RoomDrawn;
            if (neighbor != null && neighbor.HasConnectionPoints(oppositeDirection))
            {
                return GetNeighborsConnectionPoints(direction);
            }
            return null;

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

        
        //assum
        public Tuple<Point,Point> PlaceNeighbor(int direction, double gap, Point p1, Point p2 )
        {

            CatRectangle tempRep = new CatRectangle(p1, p2);
            Point start = tempRep.GetTopLeft();
            Point end = tempRep.GetBottomRight();
            double otherWidth = Math.Abs(start.GetX() - end.GetX());
            double otherHeight = Math.Abs(start.GetY() - end.GetY());

            

            double newX = start.GetX();
            double newY = start.GetY();
            switch (direction)
            {
                case 0: newY = base.Position.GetY() - otherHeight - gap; break;
                case 1: newX = base.Position.GetX() + Width + gap; break;
                case 2: newY = base.Position.GetY() + Height + gap; break;
                case 3: newX = base.Position.GetX() - otherWidth - gap; break;
            }

            Point newStart = new Point(newX, newY);
            Point newEnd = new Point(newStart.GetX()+ otherWidth, newStart.GetY()+otherHeight);

            return new Tuple<Point, Point>(newStart, newEnd);
        }


        /**
         * Returns a point that is in the middle of the given connection point plus the gap 
         */
        public Point GetNeighborsOrigin(int direction, double gap)
        { 
            Tuple<Point,Point> connection = GetConnectionPoints(direction);
            double newX = (connection.Item1.X + connection.Item2.X)/2;
            double newY = (connection.Item1.Y + connection.Item2.Y)/2;
            switch (direction)
            {
                case 0: newY = newY  - gap; break;
                case 1: newX = newX + gap; break;
                case 2: newY = newY  + gap; break;
                case 3: newX = newX - gap; break;
            }

            Point newStart = new Point(newX, newY);

            return newStart;
        }

        public override bool EntityMove(Entity entityIn, double distance)
        {
            Vector globalMovement = entityIn.GetMovementVector(distance, 0, 0);
            for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
            {
                if (!parent.HasConnection(i) )
                {
                    continue;
                }
                Room connectedRoom = parent.GetConnectedRoom(i);
                //checks if the movement vector is in the other room
                if (connectedRoom.RoomDrawn != null && connectedRoom.RoomDrawn.IsWithin(globalMovement))
                {
                    //does the movement vector intersect a door?
                    entityIn.Container = parent.GetConnectedRoom(i).RoomDrawn;
                    if (parent.GetConnectedRoom(i).RoomDrawn.DoesEntityMoveIntersect(entityIn, distance))
                    {
                        return false;
                        
                    }
                }
            }
            
            return base.EntityMove(entityIn,distance);
        }

        /**
         * Converts the given points (GLOBAL CORDINATE) into a point in the local cordinates
         * Does NOT modify the original point
         */
        public Point convertPointToLocal(Point pointIn)
        {
            return pointIn.MinusPoint(Position);
        }

        public override bool  Erase()
        {
            base.representive = new CatRectangle(originalRep.Item1, originalRep.Item2);
            for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
            {
                connectionPoints[i] = null;
            }

            parent.IsDrawn = false;
            return base.Erase();
        }
    }
}
