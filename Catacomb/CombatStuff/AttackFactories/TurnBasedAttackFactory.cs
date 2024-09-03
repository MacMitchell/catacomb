using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff.AttackFactories
{

    /**
     *  This class is for creating start/end of turn attacks 
     *  Try to keep flat values used in the attack defined at the top
     * Current paremeters: (CombatEntity castor, Command parent, CommandIterator it,CombatEntity other, AttackDecorator dec =null)
     * Actually create the attack with  Attack.CreateAttack(castor, parent, it, other, dec);
     */
    public static class TurnBasedAttackFactory
    {

        public static Attack Regeneration(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double healPercent = 5.0;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.SOT;
            currentAttack.Name = "Regeneration";

            currentAttack.ExecuteAttack = (CombatEntity c, CombatEntity t) =>
            {
                double healAmount = currentAttack.Castor.MaxHealth * (healPercent / 100.0);
                double healthBefore = currentAttack.Castor.Health;
                currentAttack.Heal(currentAttack.Castor, healAmount);
                double healthAfter = currentAttack.Castor.Health;

                currentAttack.Description = "Regeneration! "  + currentAttack.Castor.Name + " healed for " + (healthAfter - healthBefore) + "!";

            };
            return currentAttack;
        }

        public static Attack SharpStick(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double damage = 5.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "Sharp Stick";

            currentAttack.Damage = damage;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = "A sharp stick pokes " + currentAttack.Target.Name + "!";
            };
            return currentAttack;
        }

        public static Attack ToxicAura(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null) 
        {
            double baseToxicAmount = 10;


            
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.SOT;
            currentAttack.Name = "Toxic Aura";
            currentAttack.ExecuteAttack = (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Target.Poison += baseToxicAmount;

                currentAttack.Description = currentAttack.Castor.Name + " gives off a toxic aura";

                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, (currentAttack.Target.Name + " poison increased by 10"));
            };
            return currentAttack;
        } 

    }
}
