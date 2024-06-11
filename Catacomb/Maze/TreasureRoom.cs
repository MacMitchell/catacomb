using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Vectors;
using Catacomb.Visuals;
namespace Catacomb.Maze
{
    public class TreasureRoom : Room
    {
        public override double MaxWidth
        {
            get { return 1500; }
        }
        public override double MaxHeight
        {
            get { return 1500; }
        }
        public override double MinWidth
        {
            get { return 1500; }
        }
        public override double MinHeight
        {
            get { return 1500; }
        }

        public TreasureRoom() : base()
        {

        }
        public override void Create(Point p1, Point p2)
        {
            if (roomDrawn == null)
            {
                roomDrawn = new DrawnTreasureRoom(this, p1, p2);
            }
        }
    }

    public class DrawnTreasureRoom:DrawnRoom{
        public DrawnTreasureRoom(Room parentIn, Point start, Point end) : base(parentIn, start, end)
        {
        }

        protected override void DrawRoom()
        {
            base.DrawFloor();
            base.DrawRep();


            double offset = 200.0;//200.0;
            double width = 150.0; //distance between the two walls at the entrance
            double upperY = convertPointToLocal(Center).Y - width / 2.0;
            double lowerY = convertPointToLocal(Center).Y + width / 2.0;

            double extendUpperY = upperY - 150;
            double extendLowerY = lowerY + 150;

            double firstDistance = 400;
            double secondDistance = 150;
            double thirdDistance = 200;


            double roomWidth = 250;
            double roomHeight = 300;


            Point roomCenter = new Point(offset + firstDistance + secondDistance + thirdDistance + (roomWidth / 2.0), convertPointToLocal(Center).Y);

            base.AddChild(new Wall(new Point(offset, lowerY), new Point(offset + firstDistance, lowerY)));
            base.AddChild(new Wall(new Point(offset + firstDistance, lowerY), new Point(offset + firstDistance, extendLowerY)));
            base.AddChild(new Wall(new Point(offset + firstDistance, extendLowerY), new Point(offset + firstDistance + secondDistance, extendLowerY)));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance, extendLowerY), new Point(offset + firstDistance + secondDistance, lowerY)));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance, lowerY), new Point(offset+firstDistance+secondDistance+thirdDistance,lowerY)));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance + thirdDistance, lowerY), new Point(offset + firstDistance + secondDistance + thirdDistance, lowerY + (roomHeight / 2.0 - width / 2))));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance + thirdDistance, lowerY + (roomHeight / 2.0 - width / 2)), new Point(offset + firstDistance + secondDistance + thirdDistance+roomWidth, lowerY + (roomHeight / 2.0 - width / 2))));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance + thirdDistance + roomWidth, lowerY + (roomHeight / 2.0 - width / 2)), new Point(offset + firstDistance + secondDistance + thirdDistance + roomWidth, lowerY - ( width / 2.0))));


            base.AddChild(new Wall(new Point(offset, upperY), new Point(offset + firstDistance, upperY)));
            base.AddChild(new Wall(new Point(offset + firstDistance, upperY), new Point(offset + firstDistance, extendUpperY)));
            base.AddChild(new Wall(new Point(offset + firstDistance, extendUpperY), new Point(offset + firstDistance + secondDistance, extendUpperY)));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance, extendUpperY), new Point(offset + firstDistance + secondDistance, upperY)));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance, upperY), new Point(offset + firstDistance + secondDistance + thirdDistance, upperY)));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance + thirdDistance, upperY), new Point(offset + firstDistance + secondDistance + thirdDistance, upperY - (roomHeight / 2.0 - width / 2))));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance + thirdDistance, upperY - (roomHeight / 2.0 - width / 2)), new Point(offset + firstDistance + secondDistance + thirdDistance + roomWidth, upperY - (roomHeight / 2.0 - width / 2))));
            base.AddChild(new Wall(new Point(offset + firstDistance + secondDistance + thirdDistance + roomWidth, upperY - (roomHeight / 2.0 - width / 2)), new Point(offset + firstDistance + secondDistance + thirdDistance + roomWidth, upperY + (width / 2.0))));



            base.AddInteractable(new Treasure(roomCenter));


            return;
        }
    }

}
