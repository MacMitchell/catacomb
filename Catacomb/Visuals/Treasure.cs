using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

using Catacomb.Vectors;
using Catacomb.Entities;
using Catacomb.CombatStuff;
namespace Catacomb.Visuals
{
    public class Treasure:Interactable
    {
        public delegate InteractableExecute CreateTreasureExecute(Treasure treasure);

        public static int drawnId = 3;
        private static double size = 40;

        public override int DrawnId
        {
            get { return drawnId; }
        }
        public Treasure(Point middle, CreateTreasureExecute getExecute):base(new CatRectangle(middle.X - size / 2, middle.Y - size / 2, middle.X + size / 2, middle.Y + size / 2),
                                                    new CatRectangle(middle.X - size / 1.1, middle.Y - size / 1.1, middle.X + size / 1.1, middle.Y + size / 1.1)){

            canvas.Width = size;
            canvas.Height = size;

            canvas.Background = Brushes.Orange;
            trespassable = true;
            execute = getExecute(this);
            Draw();
        }

        public void CreateExecute()
        {
            execute =() => {
                //RemoveSelfInteractable(); 
                CatPopUp treasurePopUp = new CatPopUp();
                treasurePopUp.Message = "TEST message";
                treasurePopUp.Title = "TREASURE";

                Player.Instance.Fighter.AddAttack(AttackFactory.FrostLance);
                treasurePopUp.onFinish = () => { RemoveSelfInteractable(); };
                CatacombManager.Instance.DisplayPopUp(treasurePopUp);
                
            };
        }

        public static CreateTreasureExecute CreateBasicAttackTreasure(CombatEntity.AttackGenerator newAttack)
        {
            //returns a function
            return (Treasure treasureIn) =>
                () => {
                {
                    CatPopUp treasurePopUp = new CatPopUp();
                    treasurePopUp.Message = " You learned the skill " + newAttack(Player.Instance.Fighter, null, null, null, null).Name + "!";
                    treasurePopUp.Title = "Found SKill!";

                    Player.Instance.Fighter.AddAttack(newAttack);
                    treasurePopUp.onFinish = () => { treasureIn.RemoveSelfInteractable(); };
                    CatacombManager.Instance.DisplayPopUp(treasurePopUp);
                };
            };
        }
        public static CreateTreasureExecute CreateAttackDecTreasure(CombatEntity.DecoratorGenerator newAttack)
        {
            //returns a function
            return (Treasure treasureIn) =>
                () => {
                    {
                        CatPopUp treasurePopUp = new CatPopUp();
                        treasurePopUp.Message = " You found the artifact: " + newAttack(null,null).AttackDecorateName + "!" +"\n"+
                                                    newAttack(null,null).AttackDecorateDescription;
                        treasurePopUp.Title = "Found ARTIFACT!";

                        Player.Instance.Fighter.AddAttackDecorator(newAttack);
                        treasurePopUp.onFinish = () => { treasureIn.RemoveSelfInteractable(); };
                        CatacombManager.Instance.DisplayPopUp(treasurePopUp);
                    };
                };
        }
    }
}
