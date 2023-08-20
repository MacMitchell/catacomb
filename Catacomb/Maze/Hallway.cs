using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;

namespace Catacomb.Maze
{
    public class Hallway : Room
    {
        public Hallway() : base() { }
        public override string GetRoomType()
        {
            return "Hallway";
        }
        
    }
}
