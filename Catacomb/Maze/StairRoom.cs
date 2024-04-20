using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Visuals;
using Catacomb.Vectors;
namespace Catacomb.Maze
{
    public class StairRoom : Room
    {
        public override double MaxWidth
        {
            get { return 250; }
        }

        public override double MaxHeight
        {
            get { return 250; }
        }

        public override double MinWidth
        {
            get { return 250; }
        }
        public override double MinHeight
        {
            get { return 250; }
        }
        public StairRoom() : base(){}
        public override void Create(Point p1, Point p2)
        {
            roomDrawn = new DrawnStairRoom(this, p1, p2);
        }

    }

    public class DrawnStairRoom : DrawnRoom
    {
        public DrawnStairRoom(Room parentIn, Point start, Point end) : base(parentIn, start, end)
        {
        }
        protected override void DrawRoom()
        {
            base.DrawRoom();
            base.AddInteractable(new Stair(this.convertPointToLocal(base.Center)));
            
            return;
        }
    }
}
