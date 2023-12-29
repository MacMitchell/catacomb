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

        public override double MaxWidth
        {
            get {return 400; }
        }

        public override double MaxHeight
        {
            get { return 400; }
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

        public override Room Clone()
        {
            return new Hallway();
        }

    }
}
