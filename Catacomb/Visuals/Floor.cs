﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using Catacomb.Vectors;
namespace Catacomb.Visuals
{
    class Floor :Drawn
    {
        Rectangle floor;
        public SolidColorBrush color;


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
            floor = new Rectangle();
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
