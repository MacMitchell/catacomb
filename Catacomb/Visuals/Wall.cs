using Catacomb.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Global;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Catacomb.Visuals
{
    class Wall : Drawn
    {
        Line l;

        public Wall(Point start, Point end,double width = Globals.LINE_THICKNESS):base(new CatThickLine(start,end,width),false)
        {
            l = new Line();
            l.X1 = start.GetX();
            l.Y1 = start.GetY();
            l.X2 = end.GetX();
            l.Y2 = end.GetY();
            l.Stroke = Globals.LINE_COLOR;
            l.StrokeThickness =  Globals.LINE_THICKNESS;
            canvas.Children.Add(l);
            canvas.Width = Math.Abs(end.GetX() - start.GetX());
            canvas.Height = Math.Abs(start.GetMaxY(end) - start.GetMinY(end));


            
            Draw();
        }
        public override void Draw()
        {
            return;
        }

        public override string GetVectorType()
        {
            return "Wall with Rep: " + representive.GetVectorType();
        }
    }
}
