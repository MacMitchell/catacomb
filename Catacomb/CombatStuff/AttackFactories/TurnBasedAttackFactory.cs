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

        /**
         * First time the castor reaches below half health, it burn the target for a good amount
         */
        public static Attack FlamingOutrage(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double burnAmount = 50;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "Flaming Outrage";
            currentAttack.ExecuteAttack = (CombatEntity c, CombatEntity t) =>
            {
                if(castor.Health *2 <= castor.MaxHealth)
                {
                    Attack followUp = Attack.CreateAttack(castor, currentAttack, it, other, dec?.Clone());
                    followUp.Burn = burnAmount;
                    followUp.Target = currentAttack.Target;
                    followUp.Castor = currentAttack.Castor;
                    currentAttack.Description = currentAttack.Castor.Name + "'s anger causes fire to fly everywhere";
                    followUp.ExecuteAttack += (CombatEntity c2, CombatEntity t2) =>
                    {
                        followUp.Description =  followUp.Target.Name + "'s burn greatly increased!";
                        castor.RemoveTempAttack(FlamingOutrage);
                    };
                }
                else
                {
                    currentAttack.CommandReturnResult = it.ExecuteNext(c, t);
                }
            };
            return currentAttack;
        }

        /**
         * After a set number of turns, it will deal a lot of damage to castor
         */
        public static Attack Meteor(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 80;
            double baseTimer = 4;
            
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Failling Meteor";
            currentAttack.Type = AttackType.EOT;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                if (castor.metadata.ContainsKey("meteor"))
                {
                    if (Convert.ToInt32(castor.metadata["meteor"]) == 0)
                    {
                        currentAttack.Description = "The Meteor Fell!";
                        Attack metoerAttack = Attack.CreateAttack(castor, currentAttack, it, other, dec?.Clone());

                       metoerAttack.Damage = baseDamage;
                        metoerAttack.DamageType = DType.magic;
                        metoerAttack.ExecuteAttack += (CombatEntity ct, CombatEntity tt) =>
                        {
                            metoerAttack.Description = "The meteor deals devastating damage to " + currentAttack.Castor.Name + "!";
                            //remove attack
                            castor.metadata.Remove("meteor");
                            castor.RemoveTempAttack(Meteor);
                        };

                    }
                    else
                    {
                        currentAttack.Description = "The meteor is getting bigger...";    
                        castor.metadata["meteor"] = Convert.ToInt32(castor.metadata["meteor"]) - 1;
                    }
                }
                else
                {
                    castor.metadata["meteor"] = baseTimer;
                    currentAttack.Description = "A meteor appears in the sky";
                }

            };

            
            return currentAttack;
        }
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
