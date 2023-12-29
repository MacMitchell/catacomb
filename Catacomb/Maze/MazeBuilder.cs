using Catacomb.Visuals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;
using System.Windows.Controls;

namespace Catacomb.Maze
{
    
    public class MazeBuilder
    {
        private static Random rand;
        private int limit = Global.Globals.CONNECTION_LIMIT;
        public double margin = 100;
        public double gap = 150;
        private static int connectionLimit = Global.Globals.CONNECTION_LIMIT;

        
        //list of rooms that could not be built. The int value for the direction the PARENT needs to be. i.e. grab from availableParent[int]
        private List<Tuple<Room, int>> freeRooms; 
        
        private List<List<Room>> availableParents;
        public MazeBuilder()
        {
            rand = new Random();
            availableParents = new List<List<Room>>();
            for (int i = 0; i < limit; i++)
            {
                availableParents.Add(new List<Room>());
            }
            freeRooms = new List<Tuple<Room, int>>();
        }

        /**
         *  Places down the rooms next to one another, it does not draw the maze
         */ 
        public Room BuildMaze(int size, int step)
        {
            ArrayList found = new ArrayList();
            found.Add(new Room());

        
            while (found.Count < size)
            {

                int index = rand.Next(found.Count);
                Room current = (Room) found[index];
                //get two random numbers with the max size of the step. One number for the horizontal direction, the other for vertical direction
                int[] placement = GetVertAndHoriDistance(1, step);
                //decide which direction to go first
                int direction = rand.Next(4);
                for(int i = 0; i < placement[0]; i++)
                {
                    if (!current.HasConnection(direction))
                    {
                        //TODO: make another function to make the rooms

                        //Room newRoom = i == placement[0]-1?  new Room() : new Hallway();
                        Room newRoom = GetFillerRoom();
                        found.Add(newRoom);
                        current.connect(newRoom,direction);
                    }
                    current = current.GetConnectedRoom(direction);
                }

                direction = direction % 2 == 0 ? 2 * rand.Next(2) + 1 : 2 * rand.Next(2);
                for (int i = 0; i < placement[1]; i++)
                {
                    if (!current.HasConnection(direction))
                    {
                        //TODO: make another function to make the rooms
                        Room newRoom = i == placement[1] - 1 ? GetKeyRoom() : GetFillerRoom();
                        found.Add(newRoom);
                        current.connect(newRoom, direction);
                    }
                    current = current.GetConnectedRoom(direction);
                }


            }
            return ((Room)found[0]);
        }



        public Room GetFillerRoom()
        {
            return new Hallway();
        }

        public Room GetKeyRoom()
        {
            return new Room();
        }
        public void BuildRoom(List<Room> createdRooms, Room current,Canvas mazeCanvas)
        {
            createdRooms.Add(current);
            current.Draw();

            mazeCanvas.Children.Add(current.GetCanvas());
            for (int i =0;i < Global.Globals.CONNECTION_LIMIT; i++)
            {
                if (!current.HasConnection(i) || current.GetConnectedRoom(i).IsDrawn)
                {
                    continue;
                }
                //SOMETIMES THIS COLLIDES WITH THE STARTER ROOM, FIX PLEASE 
                bool result = CreateRoomNeighbors(createdRooms, current, i);
                if (result)
                {
                    BuildRoom(createdRooms, current.GetConnectedRoom(i), mazeCanvas);
                }
                else
                {
                    freeRooms.Add(new Tuple<Room, int>(current.GetConnectedRoom(i), i));

                    //sometimes it is not called when it fails to build
                    current.CloseConnection(i);
                    //current.RoomDrawn.CloseConnectionPoints(i);
                }
             
            }
        }

        /**
         * Time complexity is n^2. Things improve to make it faster
         *      1. Dont erase the whole room when it doesnt fit
         *      2. improve the check with collisions, dont need to check all rooms. Probably the biggest time save
         */
        public void BuildRooms(Room start, Canvas parentCanvas, Point origin = null)
        {
            List<Room> createdRooms = new List<Room>();
            if (origin == null) { 
                origin = new Point(0, 0);
            }
            start.Create(origin);
            BuildRoom(createdRooms, start,parentCanvas);

            BuildMissingRooms(createdRooms, parentCanvas);
        }


        private void BuildMissingRooms(List<Room> createdRooms, Canvas parentCanvas)
        {
            while(freeRooms.Count != 0)
            {
                int direction = freeRooms[0].Item2;
                Room currentRoom = freeRooms[0].Item1;
                bool successful = false;
                while (!successful)
                {
                    int sizeOfAvailableParents = availableParents[direction].Count;
                    int currentParent = unsignedRandom(0, sizeOfAvailableParents);

                    Room newParent = availableParents[direction][currentParent];
                    availableParents[direction].RemoveAt(currentParent);
                    newParent.connect(currentRoom, direction);

                    newParent.RoomDrawn.Erase();
                    newParent.Draw();

                    successful = CreateRoomNeighbors(createdRooms, newParent, direction);
                    if (!successful)
                    {
                        newParent.CloseConnection(direction);
                    }
                }
                freeRooms.RemoveAt(0);
                BuildRoom(createdRooms, currentRoom, parentCanvas);
            }
        }
        /**
         * @param i: i is the direction that the room is being created from the perspective of the parent 
         */
        public bool CreateRoomNeighbors(List<Room> createdRooms, Room parent, int i)
        { 
            Point origin = parent.RoomDrawn.GetNeighborsOrigin(i, gap);
            CatRectangle newRoom = CreateMaxSizeRoom(parent, origin, i);
            Point start = newRoom.TopLeft;
            Point end = newRoom.BottomRight;
            for(int k =0; k < createdRooms.Count; k++)
            {
                CatRectangle otherRoom = (CatRectangle)createdRooms[k].RoomDrawn.Representive;
                if (otherRoom.IsWithin(newRoom))
                {
                    otherRoom.ShrinkInvasiveCatRectangle(newRoom,parent.RoomDrawn.GetConnectionPoints(i));
                }
            }
            if(!isRoomGood(newRoom,parent,i))
            {
                return false;
            }
            Room createRoom = GetChildFromParent(parent, i);
            //parent.GetConnectedRoom(i).Create(newRoom.GetStartPoint(), newRoom.GetEndPoint());
            createRoom.Create(newRoom.GetStartPoint(), newRoom.GetEndPoint());
            setRoomUpForFosterParent(createRoom, start, end);
            
            return true;
        }


        private CatRectangle CreateMaxSizeRoom(Room parent, Point origin, int direction)
        {

            int startPos = Math.Abs(random(0, 3));
            Room roomIn = GetChildFromParent(parent, direction);
            double[] expandDirection = new double[limit];
            double[] dimension = { roomIn.MaxHeight,  roomIn.MaxWidth };

            double minSizeToFitConnection = CalculateMinSize(parent, direction);//50.0 + Global.Globals.LINE_THICKNESS;

            for(int i =0; i < connectionLimit; i++)
            {
                int currentIndex = (startPos + i) % 4;
                if(currentIndex == (direction+2)%4)
                {
                    currentIndex = (currentIndex + 2) % 4;
                }
                int dimensionIndex = (startPos + i) % 2;
                expandDirection[currentIndex] = random(Math.Max(minSizeToFitConnection, roomIn.MinWidth), dimension[dimensionIndex]);
                dimension[dimensionIndex] -= expandDirection[currentIndex];
            }

            Point start = new Point(-expandDirection[3], -expandDirection[0]);
            Point end = new Point(expandDirection[1], expandDirection[2]);

            //setting points referenced to the origin
            start = start.AddPoint(origin);
            end = end.AddPoint(origin);
            
            return new CatRectangle(start,end);
        }

        
        private int[] GetVertAndHoriDistance(int min, int max)
        {
            int[] direction = new int[2];
            int index = rand.Next(2);

            int move = random(min, max);
            direction[index] = move;
            
            index = index == 0 ? index = 1 : index = 0;
            int move2 = random(min, max - Math.Abs(move));

            direction[index] = move2;
            return direction;
        }


        /**
         *  The direction is relative to parent
         * 
         */
        private bool isRoomGood(CatRectangle roomBounds, Room parent, int direction)
        {
            Room newRoom = GetChildFromParent(parent, direction);
            //check if it is big enough
            if(roomBounds.GetWidth() < newRoom.MinWidth || roomBounds.GetHeight() < newRoom.MinHeight)
            {
                return false;
            }


            //check to see that it didnt shrink so much that the connection to room is no longer good

            Tuple<Point, Point> connectionPoints = parent.RoomDrawn.GetConnectionPoints(direction);
            
            //the connection is vertical
            if(direction % 2 == 0)
            {
                double connectionX1 = connectionPoints.Item1.X;
                double connectionX2 = connectionPoints.Item2.X;
                if(connectionX1 < roomBounds.TopLeft.X || connectionX2 < roomBounds.TopLeft.X ||
                    connectionX1 > roomBounds.TopRight.X || connectionX2 > roomBounds.TopRight.X)
                {
                    return false;
                }
            }
            else
            {
                double connectionY1 = connectionPoints.Item1.Y;
                double connectionY2 = connectionPoints.Item2.Y;
                if(connectionY1 < roomBounds.TopLeft.Y || connectionY2 < roomBounds.TopLeft.Y ||
                    connectionY1 > roomBounds.BottomRight.Y || connectionY2 > roomBounds.BottomRight.Y)
                {
                    return false;
                }
                
            }
            return true;

        }



        /**
         * direction is the direction of the child relative to the child
         */
        private double CalculateMinSize(Room parent, int direction)
        {
            double connectionDistance = 0.0;
           Tuple<Point,Point> connectionPoints =  parent.RoomDrawn.GetConnectionPoints(direction);
            if (direction % 2 == 0)
            {
                connectionDistance =  Math.Abs(connectionPoints.Item1.X - connectionPoints.Item2.X);
            }
            else
            {
                connectionDistance =  Math.Abs(connectionPoints.Item1.Y - connectionPoints.Item2.Y);
            }
            connectionDistance += Global.Globals.LINE_THICKNESS;
            return connectionDistance;
        }

        private Room GetChildFromParent(Room parent, int direction)
        {
            return parent.GetConnectedRoom(direction);
        }

        /**
         * This method takes a nearly created room and will determine if it can be a new parent for rooms that were 'destoryed
         * the two points are the points that were first attempted to be for the room.
         */
        private void setRoomUpForFosterParent(Room potentialParent, Point originalStart, Point originalEnd)
        {
            if(!potentialParent.HasConnection(0) && Global.Globals.AreDoublesEqual(originalStart.Y, potentialParent.RoomDrawn.GetStartPoint().Y)){
                availableParents[0].Add(potentialParent);
            }
            if(!potentialParent.HasConnection(1) && Global.Globals.AreDoublesEqual(originalEnd.X, potentialParent.RoomDrawn.GetEndPoint().X)){
                availableParents[1].Add(potentialParent);
            }
            if(!potentialParent.HasConnection(2) && Global.Globals.AreDoublesEqual(originalEnd.Y, potentialParent.RoomDrawn.GetEndPoint().Y))
            {
                availableParents[2].Add(potentialParent);
            }
            if(!potentialParent.HasConnection(3) && Global.Globals.AreDoublesEqual(originalStart.X, potentialParent.RoomDrawn.GetStartPoint().X))
            {
                availableParents[3].Add(potentialParent);
            }
        }
 /** Create a random number that will be equal or less than max. It will be equal or greater than the min
  *     The number has an equal chance to be negative or positive*/
        private int random(int min, int max)
        {
            int sign = (2 * (rand.Next(2) + 1)) - 3;
            int number = rand.Next(max) + min;
            int result = max >= min ? number * sign : 0;
            return result;
        }

        private int unsignedRandom(int min, int max)
        {
            int number = rand.Next(max) + min;
            return number;
        }
        private double random(double min, double max)
        {
            if(max < min)
            {
                return min;
            }
            double difference = max - min;
            double number = (rand.NextDouble()* difference) + min;
            //double result = max >= min ? number : 0;
            return number;
        }
    }
}
