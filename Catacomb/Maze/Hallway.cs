using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Maze
{
    public class Hallway : Room
    {
        public override string GetRoomType()
        {
            return "Hallway";
        }
        
    }
}
