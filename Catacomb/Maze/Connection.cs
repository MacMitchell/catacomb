using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Maze
{
    class Connection
    {
        private Room r1;
        private Room r2;
    
        public Connection(Room r1, Room r2)
        {
            this.r1 = r1;
            this.r2 = r2;
        }
        public void removeOther(Room roomIn)
        {
            if (r1 == roomIn)
            {
                r1 = null;
            }
            else if (r2 == roomIn)
            {
                r2 = null;
            }
            else
            {
                throw new RoomExecption();
            }
        }
        public Room GetOther(Room roomIn)
        {
            if (r1 == roomIn)
            {
                return r2;
            }
            else if (r2 == roomIn)
            {
                return r1;
            }
            else
            {
                throw new RoomExecption();
            }

        }
    }
}
class RoomExecption : Exception
{
    public RoomExecption() : base("Could not Find the given room") { }
}

