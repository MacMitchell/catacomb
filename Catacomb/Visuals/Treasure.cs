using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

using Catacomb.Vectors;
using Catacomb.Entities;
using Catacomb.CombatStuff;
namespace Catacomb.Visuals
{
    public class Treasure:Interactable
    {
        public static int drawnId = 3;
        private static double size = 40;

        public override int DrawnId
        {
            get { return drawnId; }
        }
        public Treasure(Point middle):base(new CatRectangle(middle.X - size / 2, middle.Y - size / 2, middle.X + size / 2, middle.Y + size / 2),
                                                    new CatRectangle(middle.X - size / 1.1, middle.Y - size / 1.1, middle.X + size / 1.1, middle.Y + size / 1.1)){

            canvas.Width = size;
            canvas.Height = size;

            canvas.Background = Brushes.Orange;
            trespassable = true;
            CreateExecute();
            
            Draw();
        }

        void CreateExecute()
        {
            execute = () => {
                //RemoveSelfInteractable(); 
                CatPopUp treasurePopUp = new CatPopUp();
                treasurePopUp.Message = "TEST message";
                treasurePopUp.Title = "TREASURE";

                Player.Instance.Fighter.AddAttack(AttackFactory.FrostLance);
                treasurePopUp.onFinish = () => { RemoveSelfInteractable(); };
                CatacombManager.Instance.DisplayPopUp(treasurePopUp);
                
            };
        }
    }
}
