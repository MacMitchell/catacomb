using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;


using Catacomb.Vectors;


namespace Catacomb.Visuals
{
    class Stair : Interactable
    {
        public static int drawnId = 3;
        public const double size = 40;
        public static SolidColorBrush BACKGROUND_STAIR_COLOR = System.Windows.Media.Brushes.Black;
        public override int DrawnId
        {
            get { return drawnId; }
        }

        public Stair(Vectors.Point middle) : base(  new CatRectangle(middle.X - size / 2, middle.Y - size / 2, middle.X + size / 2, middle.Y + size / 2),
                                                    new CatRectangle(middle.X-size/1.1, middle.Y - size/1.1, middle.X + size/1.1, middle.Y+size/1.1)) {
            canvas.Width = size;
            canvas.Height = size;
            
            
            Line outline1 = new Line();
            outline1.X1 = 0;
            outline1.Y1 = 0;
            outline1.X2 = 0;
            outline1.Y2 = size;
            outline1.Stroke = Brushes.White;
            outline1.StrokeThickness = 3;
            canvas.Children.Add(outline1);

            Line outline2 = new Line();
            outline2.X1 = 0;
            outline2.Y1 = 0;
            outline2.X2 = size;
            outline2.Y2 = 0;
            outline2.Stroke = Brushes.White;
            outline2.StrokeThickness = 3;
            canvas.Children.Add(outline2);

            Line outline3 = new Line();
            outline3.X1 = 0;
            outline3.Y1 = size;
            outline3.X2 = size;
            outline3.Y2 = size;
            outline3.Stroke = Brushes.White;
            outline3.StrokeThickness = 3;
            canvas.Children.Add(outline3);

            Line outline4 = new Line();
            outline4.X1 = size;
            outline4.Y1 = 0;
            outline4.X2 = size;
            outline4.Y2 = size;
            outline4.Stroke = Brushes.White;
            outline4.StrokeThickness = 3;
            canvas.Children.Add(outline4);

            int lineCount = 4;
            for (int i = 1; i  <= lineCount; i++)
            {
                Line l = new Line();
                l.X1 = 0;
                l.X2 = size;
                l.Y1 = size / lineCount * i;
                l.Y2 = size / lineCount * i;
                l.Stroke = Brushes.White;
                l.StrokeThickness = 2;
                canvas.Children.Add(l);

                if (i < lineCount)
                {
                    Line l2 = new Line();
                    l2.X1 = 0;
                    l2.X2 = size;
                    l2.Y1 = size / lineCount * i + 2;
                    l2.Y2 = size / lineCount * i + 2;
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
