using Catacomb.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catacomb.Vectors;

namespace Catacomb.Maze
{
    public class Room 
    {

        private Connection top;
        private Connection right;
        private Connection bottom;
        private Connection left;
        private static int counter = 0;
        private Drawn roomDrawn;

        public Drawn RoomDrawn
        {
            get { return roomDrawn; }
        }
        private int id;
        public Room()
        {
            id = counter++;
            top = null;
            right = null;
            bottom = null;
            left = null;
            roomDrawn = null;
        }
        public int getId()
        {
            return id;
        }

        public void connect(Room other, int direction)
        {
            Connection connect = new Connection(this, other);
            switch(direction)
            {
                case 0: top = connect; other.bottom = connect;break;
                case 1: right = connect; other.left = connect; break;
                case 2: bottom = connect; other.top = connect; break;
                case 3: left = connect; other.right = connect; break;

            }
        }

        public Connection GetConnection(int direction)
        {
            switch(direction)
            {
                case 0: return top;
                case 1: return right;
                case 2: return bottom;
                default : return left;
            }
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
        

        public virtual void Draw(Point p1, Point p2)
        {
            if(roomDrawn == null)
            {
                roomDrawn = new DrawnRoom(this, p1, p2);
            }
            
        }

        public virtual void Draw() {  }
    }
}
