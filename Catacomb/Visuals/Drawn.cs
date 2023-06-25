using Catacomb.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catacomb.Entities;
namespace Catacomb.Visuals
{
    abstract class Drawn 
    {
        public abstract bool doesIntersect(Vector other);
        public abstract Canvas GetDrawn();
        public abstract Point getEndPoint();
        public abstract Point getStartPoint();
        public abstract bool isWithin(Vector other);

        public  void EntityEnter(Entity entIn)
        {
            return;
        }

        public void EntityLeave(Entity entIn)
        {
            return;
        }
    }
}
