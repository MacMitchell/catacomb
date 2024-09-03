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

        private CatMaze destination;

        
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


        public StairRoom(CatMaze destination) : base(){
            this.destination = destination;
        }
        public override Room Clone()
        {
            return new StairRoom(destination);
        }
        public override void Create(Point p1, Point p2)
        {
            roomDrawn = new DrawnStairRoom(this, p1, p2, destination);
        }

    }

    public class DrawnStairRoom : DrawnRoom
    {
        private CatMaze destination;
        public DrawnStairRoom(Room parentIn, Point start, Point end, CatMaze destination) : base(parentIn, start, end)
        {
            this.destination = destination;
        }
        protected override void DrawRoom()
        {
            base.DrawRoom();
            base.AddInteractable(new Stair(this.convertPointToLocal(base.Center),destination));
            
            return;
        }
    }
}
