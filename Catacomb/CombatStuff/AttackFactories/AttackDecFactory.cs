using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff.AttackFactories
{
    public class AttackDecFactory
    {


        /**
         * Infliects a small amount of burn when dealing physical damage
         * UNTESTED 
         */
        public static AttackDecorator BurningTouch(Attack attackParent, Command parent = null)
        {
            AttackDecorator burningTouch = new AttackDecorator(attackParent, parent);
            burningTouch.SetBurn = (double value, Attack mainAttack) => mainAttack.Damage > 0 && mainAttack.DamageType == DType.physical ? value + 10 : value;
            burningTouch.Type = AttackType.DA;
            burningTouch.AttackDecorateName = "Burning Touch";
            return burningTouch;
        }
        public static AttackDecorator CoolFlames(Attack attackParent, Command parent = null)
        {
            AttackDecorator coolFlames = new AttackDecorator(attackParent, parent);

            coolFlames.SetDamage = (double value, Attack mainAttack) => mainAttack.Burn > 0 ? value-20 : value;
            coolFlames.AttackDecorateDescription = "Heal slightly when an attack burns you";
            coolFlames.Type = AttackType.DD;
            coolFlames.AttackDecorateName = "Cool Flames";
            return coolFlames;
        }

        /**
         * Slightly lowers the targets defense when dealing physical damage 
         */
        public static AttackDecorator CorrosiveTouch(Attack attackParent, Command parent= null)
        {
            AttackDecorator corrsiveTouch = new AttackDecorator(attackParent, parent);
            corrsiveTouch.SetDefenseStatChange = (double value, Attack mainAttack) => mainAttack.Damage > 0 && mainAttack.DamageType == DType.physical ? value - 3 : value;

            corrsiveTouch.Type = AttackType.DA;
            corrsiveTouch.AttackDecorateName = "Corrosive Touch";
            return corrsiveTouch;
        }
        /**
         * Goes with the end of turn attack Goblin Backup army 
         */
        public static AttackDecorator GoblinArmyRage(Attack attackParent, Command parent = null)
        {
            AttackDecorator armyRage = new AttackDecorator(attackParent, parent);
            armyRage.SetDamage = (double value, Attack mainAttack) => mainAttack.Damage > 0 ? value + 10 : value;
            armyRage.Type = AttackType.DA;
            armyRage.AttackDecorateName = "Golbin Army Rage";
            return armyRage;
        }

        public static AttackDecorator HeatedClaws(Attack attackParent, Command parent = null)
        {
            AttackDecorator heatedClaws = new AttackDecorator(attackParent, parent);
            
            
            heatedClaws.SetBurn = (double value, Attack mainAttack) => mainAttack.Damage > 0 && heatedClaws.DamageType == DType.physical ? value + 5 : value;
            
            heatedClaws.Type = AttackType.DA;
            return heatedClaws;
        }

        public static AttackDecorator PoisonTouch(Attack attackParent, Command parent = null)
        {
            AttackDecorator poisonTouch = new AttackDecorator(attackParent, parent);
            poisonTouch.SetPoison = (double value, Attack mainAttack) => mainAttack.Damage > 0 ? value + 5 : value;
            poisonTouch.Type = AttackType.DA;
            return poisonTouch;
        }


        public static AttackDecorator ShoulderGaurd(Attack attackParent, Command parent = null)
        {
            AttackDecorator poisonTouch = new AttackDecorator(attackParent, parent);
            poisonTouch.SetDamage = (double value, Attack mainAttack) => Math.Max(value - 5, 0);
            poisonTouch.Type = AttackType.DD;
            return poisonTouch;
        }

        public static AttackDecorator Sleepy(Attack attackParent, Command parent = null)
        {
            AttackDecorator Sleepy = new AttackDecorator(attackParent, parent);
            Sleepy.AttackDecorateName = "Sleepy";
            Sleepy.AttackDecorateDescription = "After big attacks, you get tired";
            Sleepy.Type = AttackType.DA;
            Sleepy.SetSelfTired = (double value, Attack mainAttack) => mainAttack.Damage >= 40 ? value + 1 : value;

            return Sleepy;
        }

    }
}
