﻿using Catacomb.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Controls.Shapes;

using Catacomb.Global;
using System.Collections;
using Catacomb.Entities;

namespace Catacomb.Visuals
{
    public class DrawnRoom : Drawn
    {
        private static Random rand;
        public  Room parent;
        public Tuple<Point, Point>[] connectionPoints;
        public CatRectangle originalRep;
        protected bool[] createdConnectionLocal;
        protected List<CatRectangle> potentialSpawnAreas;
        protected Ellipse[] debugConnections;


        public String testString = "";
        public double Width
        {
            get { return ((CatRectangle)base.representive).GetWidth(); }
        }

        public double Height
        {
            get { return ((CatRectangle)base.representive).GetHeight(); }
        }

        protected Point Center
        {
            get { return new Point(originalRep.Center.X, originalRep.Center.Y); }
        }
        public DrawnRoom(Room parentIn, Point start, Point end) : base(new CatRectangle(start, end), true)
        {
            originalRep = new CatRectangle(start, end);
            parent = parentIn;
            canvas = new Canvas();
            canvas.Width = start.GetMaxX(end) - start.GetMinX(end); //+ 5* Globals.LINE_THICKNESS;
            canvas.Height = start.GetMaxY(end) - start.GetMinY(end);// + 5* Globals.LINE_THICKNESS;
            canvas.Background = Globals.MAZE_BACKGROUND_COLOR;
            rand = new Random();
            debugConnections = new Ellipse[Globals.CONNECTION_LIMIT];
            createdConnectionLocal = new bool[Globals.CONNECTION_LIMIT];

            
            connectionPoints = new Tuple<Point, Point>[Globals.CONNECTION_LIMIT];
            for(int i =0; i < Globals.CONNECTION_LIMIT; i++)
            {
                connectionPoints[i] = null;
                createdConnectionLocal[i] = false;
            }

            potentialSpawnAreas = new List<CatRectangle>();

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

            if(localConnectionPoints == null)
            {
                CreateConnectionPoints(direction, start, end);
                createdConnectionLocal[direction] = true;
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
                DrawFloor(new CatRectangle(p2, p4));
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
            connectionPoints[direction] = null;
            createdConnectionLocal[direction] = false;
            canvas.Children.Remove(debugConnections[direction]);
            base.AddChild(new Wall(connectionPoint.Item1, connectionPoint.Item2));
        }
        public override void Draw()
        {
            DrawRoom();

            if (Globals.DEBUG)
            {                
                if (Globals.SHOW_CONNECTION_POINTS)
                {
                    for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
                    {

                        Point connection = GetLocalConnectionPointCenter(i);
                        if(connection == null)
                        {
                            continue;
                        }
                        double diameter = 16;
                        debugConnections[i] = new Ellipse();
                        debugConnections[i].Height = diameter;
                        debugConnections[i].Width = diameter;
                        debugConnections[i].Fill = Brushes.Purple;
                        canvas.Children.Add(debugConnections[i]);

                        Line test = new Line();
                        test.StartPoint = new Avalonia.Point(convertPointToLocal(Center).X, convertPointToLocal(Center).Y);
                        test.EndPoint = new Avalonia.Point(connection.X, connection.Y);
                        test.StrokeThickness = 8;
                        test.Stroke = Brushes.Orange;
                        canvas.Children.Add(test);
                        Canvas.SetLeft(debugConnections[i], connection.X - diameter / 2.0);
                        Canvas.SetTop(debugConnections[i], connection.Y - diameter /2.0);
                    }
                }
                if (Globals.SHOW_ROOM_META_TEXT)
                {
                    TextBlock testBox = new TextBlock();
                    testBox.Background = Brushes.White;
                    testBox.Text = testString;
                    canvas.Children.Add(testBox);
                    Canvas.SetLeft(testBox, Globals.LINE_THICKNESS * 2);
                    Canvas.SetTop(testBox, Globals.LINE_THICKNESS * 2);
                }
            }
            
            base.Draw();

        }

        protected virtual void DrawRoom()
        {
            DrawFloor();
            AddFloorToSpawnArea();
            DrawRep();
        }


        protected virtual void AddFloorToSpawnArea()
        {
            //THis grabs the same area as the floor. It MIGHT need to be shunk a little bit, we will see
            CatRectangle rep = (CatRectangle)representive;
            CatRectangle localRep = new CatRectangle(convertPointToLocal(rep.TopLeft), convertPointToLocal(rep.BottomRight));
            potentialSpawnAreas.Add(localRep);
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

        /**
         * This returns the center poit in LOCAL cordinates 
         */
        public Point GetLocalConnectionPointCenter(int direction, bool global = false)
        {
            Tuple<Point,Point> average;
            double averageX = 0.0 ;
            double averageY = 0.0;
            if(!parent.HasConnection(direction))
            {
                return null;
            }
            DrawnRoom neighbor = parent.GetConnectedRoom(direction).RoomDrawn;
            if (createdConnectionLocal[direction])
            {
                average = connectionPoints[direction];
            }
            else if (neighbor != null && neighbor.HasConnectionPoints(Room.GetOppositeDirection(direction)))
            {
                average = GetNeighborsConnectionPoints(direction);
                averageX = direction == Globals.LEFT ? Globals.CONNCETION_LENGTH : direction == Globals.RIGHT ? -Globals.CONNCETION_LENGTH : 0.0;
                averageY = direction == Globals.TOP ? Globals.CONNCETION_LENGTH : direction == Globals.BOTTOM ? -Globals.CONNCETION_LENGTH : 0.0;
            }
            else
            {
                return null;
            }
            averageX += (average.Item1.X + average.Item2.X) /2.0;
            averageY += (average.Item1.Y + average.Item2.Y) / 2.0;
            return global ? convertPointToGlobal(new Point(averageX,averageY)) :new Point(averageX, averageY);
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

        public CatRectangle CreateConnectionRep(int direction, double gap)
        {

            //The GOAL is to create a square that goes from the parent room to the child room that encompasses the connection
            Tuple<Point, Point> connectionPoints = GetConnectionPoints(direction);
            double distance = connectionPoints.Item1.GetDistance(connectionPoints.Item2)/2.0;
            
            //These points reference the center of the connection points, will need to offset each point
            Point otherRoomPoint = GetNeighborsOrigin(direction, gap);
            Point thisRoomPoint = GetNeighborsOrigin(direction,0);

            switch (direction)
            {
                //top and bottom
                case 0:
                case 2:
                    thisRoomPoint = thisRoomPoint.AddPoint(new Point(distance, 0));
                    otherRoomPoint = otherRoomPoint.AddPoint(new Point(-distance, 0));
                    break;


                //left and right
                case 1:
                case 3:
                    thisRoomPoint = thisRoomPoint.AddPoint(new Point(0, distance));
                    otherRoomPoint = otherRoomPoint.AddPoint(new Point(0, -distance));
                    break;
            }
            return new CatRectangle(thisRoomPoint, otherRoomPoint);
        }

        public virtual bool DoesVectorIntersectConnectingRooms(Vector vectorIn)
        {
            //assumes vector is in global space
            for(int i =0; i < Globals.CONNECTION_LIMIT; i++)
            {
                if (!parent.HasConnection(i))
                {
                    continue;
                }
                Room connectedRoom = parent.GetConnectedRoom(i);

                if(connectedRoom.RoomDrawn != null && connectedRoom.RoomDrawn.IsWithin(vectorIn))
                {
                    if (connectedRoom.RoomDrawn.DoesGlobalIntersect(vectorIn)) {
                        return true;
                    };
                }
            }
            return DoesGlobalIntersect(vectorIn);
        }

        public bool DoesGlobalIntersect(Vector vectorIn)
        {
            double oldOffsetX = vectorIn.OffsetX;
            double oldOffsetY = vectorIn.OffsetY;

            vectorIn.OffsetX -= Position.X;
            vectorIn.OffsetY -= Position.Y;
            bool result = DoesIntersect(vectorIn);
            vectorIn.OffsetX = oldOffsetX;
            vectorIn.OffsetY = oldOffsetY;
            return result;
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
                    if (parent.GetConnectedRoom(i).RoomDrawn.DoesEntityMoveIntersect(entityIn, distance))
                    {
                        return false;
                        
                    }                    

                }
            }
            
            return base.EntityMove(entityIn,distance);
        }

        public DrawnRoom GetConnection(int direction)
        {
            return parent.GetConnectedRoom(direction).RoomDrawn;
        }

        /**
         * Converts the given points (GLOBAL CORDINATE) into a point in the local cordinates
         * Does NOT modify the original point
         */
        public Point convertPointToLocal(Point pointIn)
        {
            return pointIn.MinusPoint(Position);
        }

        public Point convertPointToGlobal(Point pointIn)
        {
            return pointIn.AddPoint(Position);
        }
        public override bool  Erase()
        {
            base.representive = originalRep.Clone();
            for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
            {
                connectionPoints[i] = null;
            }

            parent.IsDrawn = false;
            return base.Erase();
        }

        protected virtual void DrawFloor()
        {
            CatRectangle rep = (CatRectangle)representive;
            CatRectangle localRep = new CatRectangle(convertPointToLocal(rep.TopLeft), convertPointToLocal(rep.BottomRight));
            DrawFloor(localRep);
        }
        protected virtual void DrawFloor(CatRectangle floor)
        {
            base.AddChild(new Floor(floor));
            
        }
        public DrawnRoom GetEntityCurrentRoom(Entity entityIn)
        {
            if (originalRep.IsWithin(entityIn))
            {
                return this;
            }
            for(int i =0; i < Globals.CONNECTION_LIMIT; i++)
            {
                if (!parent.HasConnection(i))
                {
                    continue;
                }
                if (GetConnection(i).originalRep.IsWithin(entityIn))
                {
                    return GetConnection(i);
                }
            }
            double minDistance = originalRep.Center.GetDistance(entityIn.Center);
            DrawnRoom min = this;
            for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
            {
                if (!parent.HasConnection(i))
                {
                    continue;
                }
                double distance = GetConnection(i).originalRep.Center.GetDistance(entityIn.Center);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    min = GetConnection(i);
                }
            }
            return min;
        }
        public virtual Point GenerateSpawnPoint(double width, double height, double offset = Globals.LINE_THICKNESS)
        {
            if(potentialSpawnAreas.Count == 0)
            {
                return null;
            }
            CatRectangle potentialArea = potentialSpawnAreas[Globals.Rand.Next(potentialSpawnAreas.Count)];

            double minX = potentialArea.TopLeft.X + offset*2; //+ (width / 2.0);
            double maxX = potentialArea.TopRight.X - offset - (width / 1.0);

            double minY = potentialArea.TopLeft.Y + offset*2;//+ (height / 2.0);
            double maxY = potentialArea.BottomRight.Y - offset - (height / 1.0);
            if(maxX < minX || maxY < minY)
            {
                return null;
            }
            double distanceX = maxX - minX;
            double distanceY = maxY - minY;

            double randomX = rand.NextDouble() * distanceX + minX;
            double randomY = rand.NextDouble() * distanceY + minY;
            Point localSpawnPoint = new Point(randomX, randomY);

            return convertPointToGlobal(localSpawnPoint);
        }

        public virtual void MustExecute(CatMaze mazeIn)
        {
            return;
        }
        
    }
}
