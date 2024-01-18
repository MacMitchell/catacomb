using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Vectors;
using Catacomb.Global;
using System.Windows.Media;
using Catacomb.Maze;

namespace Catacomb.Entities
{
    public class Monster : Entity
    {
        private Movement movementAI;
        public Movement MovementAI
        {
            get { return movementAI; }
            set { movementAI = value; }
            
        }
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

        public override void Move(double time)
        {
            if(MovementAI != null)
            {
                if (movementAI.SetUpMove(time))
                {
                    Velocity = MaxVelocity;
                    base.Move(time);
                    Draw();
                }
            }   
        }
        public Monster DoesCollideWithPlayer(List<Room> roomsToCheck, Player playIn)
        {
            if (!roomsToCheck.Contains(this.Container.parent))
            {
                return null;
            }
            if (this.IsWithin(playIn))
            {
                return this;
            }
            return null;
        }
    }
}
