using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Catacomb.Vectors;
using Catacomb.Entities;
namespace Catacomb.Visuals
{
    public class Interactable : Drawn
    {
        Vector interactableRange;
        bool automatic = false;

        public delegate void InteractableExecute();
        protected InteractableExecute execute;


        public Interactable(Vector drawnVector, Vector interactableRange) : base(drawnVector)
        {
            this.interactableRange = interactableRange;

        }
     
        public bool CanInteract(Point p)
        {
            return interactableRange.IsPointInVector(p);
        }

        public void Execute()
        {
            execute();
        }

        
    }
}
