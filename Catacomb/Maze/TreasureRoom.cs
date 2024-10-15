using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Vectors;
using Catacomb.Visuals;
using Catacomb.CombatStuff;
namespace Catacomb.Maze
{
    public class BaseTreasureRoom : Room
    {
        public BaseTreasureRoom(Treasure.CreateTreasureExecute executeIn)
        {
            GetExecute = executeIn;
        }
        private Treasure.CreateTreasureExecute getExecute;
        public Treasure.CreateTreasureExecute GetExecute {get => getExecute; set  => getExecute = value; }

    };
    public class BaseDrawnTreasureRoom: DrawnRoom
    {
        private Treasure.CreateTreasureExecute getExecute;
        public BaseDrawnTreasureRoom(Room parentIn, Point start, Point end, Treasure.CreateTreasureExecute getExecute) : base(parentIn, start, end)
        {
            this.getExecute = getExecute;
        }
        protected void AddTreasure(Point p)
        {
            base.AddInteractable(new Treasure(p, getExecute));
        }
    }

    public class TreasureRoom : BaseTreasureRoom
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


        public TreasureRoom(Treasure.CreateTreasureExecute executeIn) : base(executeIn)
        {
        }

        public override Room Clone()
        {
            TreasureRoom toReturn = new TreasureRoom(GetExecute);
            return toReturn;
        }
        public override void Create(Point p1, Point p2)
        {
            if (roomDrawn == null)
            {
                roomDrawn = new DrawnTreasureRoom(this, p1, p2,GetExecute);
            }
        }
    }

    public class DrawnTreasureRoom:BaseDrawnTreasureRoom{
        public DrawnTreasureRoom(Room parentIn, Point start, Point end,Treasure.CreateTreasureExecute getExecute) : base(parentIn, start, end,getExecute)
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

            double spawnAreaoffset = 25;

            potentialSpawnAreas.Add(new CatRectangle(offset + firstDistance + spawnAreaoffset, extendLowerY - spawnAreaoffset, offset + firstDistance + secondDistance - spawnAreaoffset, lowerY + spawnAreaoffset));
            potentialSpawnAreas.Add(new CatRectangle(offset +firstDistance + spawnAreaoffset, extendUpperY + spawnAreaoffset, offset + firstDistance + secondDistance - spawnAreaoffset, upperY - spawnAreaoffset));
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


            
            base.AddTreasure(roomCenter);            

            return;
        }
    }

}
