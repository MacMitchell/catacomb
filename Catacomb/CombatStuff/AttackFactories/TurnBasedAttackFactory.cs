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
        public static Attack Rage(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double statChange = 6;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "Regeneration";
            currentAttack.SelfDefenseStatChange = -statChange;
            currentAttack.SelfAttackStatChange = statChange;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                
                currentAttack.Description = "Rage! " + currentAttack.Castor.Name + " is getting enraged! Attack rose but defense fell!" ;

            };
            return currentAttack;
        }

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
            currentAttack.DamageType = DType.physical;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = "A sharp stick pokes " + currentAttack.Target.Name + "!";
            };
            return currentAttack;
        }

        /**
         * This is a debuff that slowly starts to do more damage over time.
         */
        public static Attack StackingCurse(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double defaultDamage = 5;
            double stackingAmount = 2;

            double currentDamage = defaultDamage;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.EOT;
            currentAttack.DamageType = DType.pierce;
            currentAttack.Name = "Stacking Curse";

            if (castor.metadata.ContainsKey("stacking_curse"))
            {
                //currentDamage = ( (int) castor.metadata["stacking_curse"])  + stackingAmount;
                currentDamage = Convert.ToDouble(castor.metadata["stacking_curse"]) + stackingAmount;
            }

            currentAttack.SelfHeal = currentDamage * -1;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                castor.metadata["stacking_curse"] = currentDamage;

                currentAttack.Description = currentAttack.Castor.Name + " takes damage from the curse";


                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, ("The curse worsens"));
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

                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, (currentAttack.Target.Name + " poison increased by " + baseToxicAmount));
            };
            return currentAttack;
        } 

    }
}
