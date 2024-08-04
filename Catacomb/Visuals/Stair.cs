using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia;
using Catacomb.Vectors;


namespace Catacomb.Visuals
{
    class Stair : Interactable
    {
        public static int drawnId = 3;
        public const double size = 40;
        public static IImmutableBrush BACKGROUND_STAIR_COLOR = Avalonia.Media.Brushes.Black;
        public override int DrawnId
        {
            get { return drawnId; }
        }

        public Stair(Vectors.Point middle) : base(  new CatRectangle(middle.X - size / 2, middle.Y - size / 2, middle.X + size / 2, middle.Y + size / 2),
                                                    new CatRectangle(middle.X-size/1.1, middle.Y - size/1.1, middle.X + size/1.1, middle.Y+size/1.1)) {
            canvas.Width = size;
            canvas.Height = size;
            
            
            Line outline1 = new Line();
            outline1.StartPoint = new Avalonia.Point(0, 0);
            outline1.EndPoint = new Avalonia.Point(0, size);
            outline1.Stroke = Brushes.White;
            outline1.StrokeThickness = 3;
            canvas.Children.Add(outline1);

            Line outline2 = new Line();
            outline2.StartPoint = new Avalonia.Point(0, 0);
            outline2.EndPoint = new Avalonia.Point(size, 0);
            
            outline2.Stroke = Brushes.White;
            outline2.StrokeThickness = 3;
            canvas.Children.Add(outline2);

            Line outline3 = new Line();
            outline3.StartPoint = new Avalonia.Point(0, size);
            outline3.EndPoint = new Avalonia.Point(size, size);

            outline3.Stroke = Brushes.White;
            outline3.StrokeThickness = 3;
            canvas.Children.Add(outline3);

            Line outline4 = new Line();
            outline4.StartPoint = new Avalonia.Point(size, 0);
            outline4.EndPoint = new Avalonia.Point(size, size);
            outline4.Stroke = Brushes.White;
            outline4.StrokeThickness = 3;
            canvas.Children.Add(outline4);

            int lineCount = 4;
            for (int i = 1; i  <= lineCount; i++)
            {
                Line l = new Line();
                l.StartPoint = new Avalonia.Point(0, size/lineCount * i);
                l.EndPoint = new Avalonia.Point(size, size / lineCount * i);

                l.Stroke = Brushes.White;
                l.StrokeThickness = 2;
                canvas.Children.Add(l);

                if (i < lineCount)
                {
                    Line l2 = new Line();
                    l2.StartPoint = new Avalonia.Point(0, size / lineCount * i+2);
                    l2.EndPoint = new Avalonia.Point(size, size / lineCount * i + 2);
                    l2.Stroke = Brushes.LightGray;
                    l2.StrokeThickness = 2;
                    canvas.Children.Add(l2);
                }
            }
            
            canvas.Background = BACKGROUND_STAIR_COLOR;
            trespassable = true;

            base.execute = ()=>CatacombManager.Instance.NextFloor();
            Draw();
        }


        public override string GetVectorType()
        {
            return "Stair with Rep: " + representive.GetVectorType();
        }
    }
}
