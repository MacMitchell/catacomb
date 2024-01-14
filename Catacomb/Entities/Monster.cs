using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Vectors;
using Catacomb.Global;
using System.Windows.Media;

namespace Catacomb.Entities
{
    public class Monster : Entity
    {
        public Monster(double width, double height,double maxVelocity) : base(new Point(0,0), new CatRectangle(0, 0, width, height))
        {
            Width = width;
            Height = height;
            
            canvas.Width = Width;
            canvas.Height = Height;
            SetColor(Brushes.Red);
            Velocity = 0;
            MaxVelocity = maxVelocity;
        }
        
        public void PlaceMonster(Point spawnPoint)
        {
            Position = spawnPoint;
            Draw();
        }
    }
}
