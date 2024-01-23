using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    class AttackFactory
    {

        
        public static Attack Tackle(CombatEntity castor,Command parent)
        {
            double baseDamage = 10;
            //every 10 attack over base makes this do 1 more
            double offset = castor.AttackStat - Global.Globals.BASE_ATTACK_STAT;
            double damage = baseDamage + offset / 10.0;
            Attack currentAttack = new Attack(parent);
            currentAttack.Damage = damage;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " tackled " + currentAttack.Target.Name;
            };

            return currentAttack;
        }
        
    }
}
