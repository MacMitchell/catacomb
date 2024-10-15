using Catacomb.Vectors;
using Catacomb.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Maze.Rooms
{
   public class RandomTreasureRoom : BaseTreasureRoom
    {
        public RandomTreasureRoom(Treasure.CreateTreasureExecute executeIn) : base(executeIn)
        {
        }

        public override Room Clone()
        {
            RandomTreasureRoom toReturn = new RandomTreasureRoom(GetExecute);
            return toReturn;
        }
        public override void Create(Point p1, Point p2)
        {
            if (roomDrawn == null)
            {
                roomDrawn = new DrawnRandomTreasureRoom(this, p1, p2, GetExecute);
            }
        }
        internal class DrawnRandomTreasureRoom: BaseDrawnTreasureRoom
        {
            public DrawnRandomTreasureRoom(Room parentIn, Point start, Point end, Treasure.CreateTreasureExecute getExecute) : base(parentIn, start, end, getExecute) { }

            public override void Draw()
            {
                base.Draw();

                double offset = 100;
                Point topLeft = base.originalRep.TopLeft;
                Point bottomRight = base.originalRep.BottomRight;

                double xValue = Global.Globals.GetRandomNumber(topLeft.X+offset, bottomRight.X-offset);
                double yValue = Global.Globals.GetRandomNumber(topLeft.Y + offset, bottomRight.Y - offset);

                base.AddTreasure(base.convertPointToLocal(new Point(xValue, yValue)));

            }
        }
    }

}
