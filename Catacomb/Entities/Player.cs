using Catacomb.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Catacomb.Visuals;

namespace Catacomb.Entities
{
    public class Player :Entity
    {
        double width = 10;
        double height = 10;
        
        public Player(Point positionIn) : base(positionIn, new CatRectangle(0, 0, 10, 10)) {
            canvas.Width = width;
            canvas.Height = height;
            canvas.Background = Brushes.Orange;
            Draw();
        }


        public override void Draw()
        {
            //position is in the middle of the player
            double distanceToX = width / 2;
            double distanceToY = height / 2;
            representive = new CatRectangle(Position.GetX() - distanceToX, Position.GetY() - distanceToY, Position.GetX() + distanceToX, Position.GetY() + distanceToY);
            base.Draw();
        }

   

       
        public void Move(Key e)
        {
            switch (e)
            {
                case Key.A:  Angle = Math.PI; base.Move(1); break;
                case Key.S:  Angle = Math.PI/2; base.Move(1); break;
                case Key.D:  Angle = 0;  base.Move(1); break;
                case Key.W:  Angle = 3*Math.PI/2; base.Move(1); break;
            }
        }
    }
}
