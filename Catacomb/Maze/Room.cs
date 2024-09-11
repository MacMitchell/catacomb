using Catacomb.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Catacomb.Vectors;
using Catacomb.Entities;
using Catacomb.CombatStuff;
namespace Catacomb.Maze
{
    public class Room 
    {
       
        private Connection top;
        private Connection right;
        private Connection bottom;
        private Connection left;
        private static int counter = 0;
        protected DrawnRoom roomDrawn;
        private bool isDrawn = false;

        public bool IsDrawn
        {
            get { return isDrawn; }
            set { isDrawn = false; }
        }
        public virtual double MaxWidth
        {
            get {return 1600; }
        }

        public virtual double MaxHeight
        {
            get { return 1600; }
        }

        public virtual double MinWidth
        {
            get { return 800; }
        }
        public virtual double MinHeight
        {
            get { return 800; }
        }

        public DrawnRoom RoomDrawn
        {
            get { return roomDrawn; }
        }
        public CatRectangle Representive
        {
            get { if(roomDrawn == null)
                {
                    return null;
                }
                return (CatRectangle)roomDrawn.representive;
            }
        }

        public List<MonsterType> PossibleMonsters { get => possibleMonsters; set => possibleMonsters = value; }

        public int id;

        private List<MonsterType> possibleMonsters;
        public Room()
        {
            
            id = counter++;
            top = null;
            right = null;
            bottom = null;
            left = null;
            roomDrawn = null;
            PossibleMonsters = new List<MonsterType>();
            PossibleMonsters.Add(MonsterType.All);
        }

      
        public int getId()
        {
            return id;
        }

        public void connect(Room other, int direction)
        {
            Connection connect = new Connection(this, other);
            switch (direction)
            {
                case 0: top = connect; other.bottom = connect; break;
                case 1: right = connect; other.left = connect; break;
                case 2: bottom = connect; other.top = connect; break;
                case 3: left = connect; other.right = connect; break;

            }
        }

        public Connection GetConnection(int direction)
        {
            //return connections[direction];
            switch(direction)
            {
                case 0: return top;
                case 1: return right;
                case 2: return bottom;
                default : return left;
            }
        }

        public bool RemoveConnection(int direction)
        {
            Room other = GetConnectedRoom(direction);
            switch(direction)
            {
                case 0: top = null; other.bottom = null;  break;
                case 1: right = null; other.left = null; break;
                case 2: bottom = null; other.top = null; break;
                case 3: left = null; other.right = null; break;

            }
            return true;
        }
        public Room GetConnectedRoom(int direction)
        {
            return GetConnection(direction).GetOther(this);  
        }

        public bool HasConnection(int direction)
        {
            return GetConnection(direction) != null;
        }
        public virtual string GetRoomType()
        {
            return "Room";
        }

        public Canvas GetCanvas()
        {
            return roomDrawn.GetCanvas();
        }
        

        public virtual void Create(Point p1, Point p2)
        {
            roomDrawn = new DrawnRoom(this,p1, p2);
        }

        public virtual void Create(Point p1)
        {
            Point p2 = new Point(250, 250);
            p2 = p1.AddPoint(p2);
            Create(p1, p2);
        }

        public virtual void Draw()
        {
            if(!isDrawn)
            {
                isDrawn = true;
                roomDrawn.Draw();
            }
            
        }

        public virtual void CloseConnection(int direction)
        {

            Room otherRoom = GetConnectedRoom(direction);
            if (otherRoom.isDrawn)
            {
                otherRoom.RoomDrawn.CloseConnectionPoints(GetOppositeDirection(direction));
            }
            RemoveConnection(direction);
            

            RoomDrawn.CloseConnectionPoints(direction);    
        }

        public static int GetOppositeDirection(int direction)
        {
            int newDir = direction >= 2 ? direction - 2 : direction + 2;
            return newDir;
        }

        public virtual Room Clone()
        {
            return new Room();
        }
        public List<Room> GetAllConnectedRooms(bool inclusive = false)
        {
            List<Room> allRooms = new List<Room>();
            if (inclusive)
            {
                allRooms.Add(this);
            }
            for(int i =0; i < Global.Globals.CONNECTION_LIMIT; i++)
            {
                if (HasConnection(i))
                {
                    allRooms.Add(GetConnectedRoom(i));
                }
            }
            return allRooms;
        }
        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            Room otherRoom = (Room)obj;
            return id == otherRoom.id;
        }

        public virtual List<Monster> AcceptableMonsters(List<Monster> allMonsters)
        {
            if (possibleMonsters.Contains(MonsterType.All)){
                return allMonsters;
            }
            var filteredResults = allMonsters.Where((monster) => possibleMonsters.Contains(monster.Type));
            return (List<Monster>)filteredResults;
        }
    }
}
