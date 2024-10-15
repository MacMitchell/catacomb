using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff.AttackFactories
{
    public class AttackDecFactory
    {
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

    }
}
