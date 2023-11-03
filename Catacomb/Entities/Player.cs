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
        private static double width = 25;
        private static double height = 25;
        
        public Player(Point positionIn) : base(positionIn, new CatRectangle(0, 0, width,height)) {
            canvas.Width = width;
            canvas.Height = height;
            canvas.Background = Brushes.Orange;
            Velocity = 0;
            MaxVelocity = 250;
            Draw();
        }


        public override void Draw()
        {
            //position is in the middle of the player
            Update();
            base.Draw();
        }

        public override void Update()
        {
            double distanceToX = width / 2;
            double distanceToY = height / 2;
            representive = new CatRectangle(Position.GetX() - distanceToX, Position.GetY() - distanceToY, Position.GetX() + distanceToX, Position.GetY() + distanceToY);

        }




        public void KeyPress(Key e)
        {
            switch (e)
            {
                case Key.A:  Angle = Math.PI;  break;
                case Key.S:  Angle = Math.PI/2; break;
                case Key.D:  Angle = 0;  break;
                case Key.W:  Angle = 3*Math.PI/2; break;
            }
        }
    }
}
