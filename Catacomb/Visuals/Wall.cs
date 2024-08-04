using Catacomb.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Global;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace Catacomb.Visuals
{
    class Wall : Drawn
    {
        Line l;
        public static int drawnId = 1;
        
        public override int DrawnId
        {
            get { return drawnId; }
        }
        public Wall(Point start, Point end,double width = Globals.LINE_THICKNESS):base(new CatThickLine(start,end,width),false)
        {
            l = new Line();
            l.StartPoint = new Avalonia.Point(start.GetX(), start.GetY());
            l.EndPoint = new Avalonia.Point(end.GetX(), end.GetY());
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
