using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Catacomb.Vectors;
namespace Catacomb.Visuals
{
    class Floor :Drawn
    {
        Rectangle floor;
        public IImmutableBrush color;
        public static int drawnId = 2;
        public override int DrawnId
        {
            get { return 2; }
        }

        public Floor(CatRectangle rep): base(rep)
        {
            Create(rep);
        }
        public Floor(Point start, Point end) : base(new CatRectangle(start,end))
        {
            CatRectangle rep = (CatRectangle)representive;
            Create(rep);

        }

        private void Create(CatRectangle rep)
        {
            floor = new Avalonia.Controls.Shapes.Rectangle();
            floor.Width = rep.GetWidth();
            floor.Height = rep.GetHeight();
            floor.Fill = Global.Globals.FLOOR_COLOR;
            canvas.Children.Add(floor);
            canvas.Width = floor.Width;
            canvas.Height = floor.Height;
            trespassable = true;
            Draw();
        }
        public override string GetVectorType()
        {
            return "Floor with Rep: " + representive.GetVectorType();
        }
    }
}
