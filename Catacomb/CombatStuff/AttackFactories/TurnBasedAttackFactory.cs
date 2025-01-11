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


        public static Attack BlizzardPassive(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 5;
            baseDamage += castor.MagicStat / 10;

            double speedReduce = 5;
            speedReduce += castor.MagicStat / 10;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Blizzard Passive";
            currentAttack.Type = AttackType.EOT;
            currentAttack.DamageType = DType.magic;

            currentAttack.Damage = baseDamage;
            currentAttack.SpeedStatChange = -speedReduce;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"A blizzard rages, slowing and dmaaging {currentAttack.Target.Name}!";
            };
            return currentAttack;
        }

        public static Attack BurnLover(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double increase = castor.Burn / 10;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.SOT;
            currentAttack.Name = "Burn Lover";
            currentAttack.SelfMagicAttackStatChange = increase;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} loves the burn!";
            };

            return currentAttack;
        }

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

        public static Attack GoblinBackupArmy(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Damage = baseDamage;
            currentAttack.DamageType = DType.physical;


            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "Goblin Backup Army";
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} army attacks {currentAttack.Target.Name}!";
                if (!currentAttack.Castor.metadata.ContainsKey("goblin_army_attack_count"))
                {
                    currentAttack.Castor.metadata.Add("goblin_army_attack_count", 0);
                }
                currentAttack.Castor.metadata["goblin_army_attack_count"] = ( (int) currentAttack.Castor.metadata["goblin_army_attack_count"]) + 1;
            };
            
            return currentAttack;

        }

        public static Attack GoblinFriend(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;
            baseDamage += castor.AttackStat / 2;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Damage = baseDamage;
            currentAttack.DamageType = DType.physical;


            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "Goblin Friend";
            currentAttack.Description = "A friendly goblin that gets stronger with you!";
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name}'s goblin friend attacks!";
            };

            return currentAttack;
        }

        public static Attack HotBod(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseBurn = 5;
            baseBurn += castor.MagicStat / 5;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Burn = baseBurn;
            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "HotBod";
            currentAttack.Description = "Burns everyone at the end of the turn";
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " is burning hot. It causes " + currentAttack.Target.Name +" to burn!";
                Attack followup = Attack.CreateAttack(castor, currentAttack, it, castor, castor.CreateEntityDecorator(null, true));
                followup.Target = castor;
                followup.Burn = baseBurn;
                followup.ExecuteAttack += (CombatEntity co, CombatEntity oo) => {
                    followup.Description = currentAttack.Castor.Name + " gets even hotter";

                };
            };

            return currentAttack;
        }



        public static Attack LesserManaRegen(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double regenAmount = 5;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Lesser Mana Regen";
            currentAttack.Type = AttackType.EOT;

            currentAttack.SelfManaDrain = Math.Min(regenAmount,castor.MaxMana - castor.Mana);
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} regenerates some mana";
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


        /**
         * When the target health falls below half health, it greatly increaes your spped and slightly attack UNTESTED 
         */
        public static Attack SenseBlood(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double attackIncreasse = 10;
            double speedIncrease = 100;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "Sense Blood";
            currentAttack.ExecuteAttack = (CombatEntity c, CombatEntity t) =>
            {
                if (other.Health * 2 <= other.MaxHealth)
                {
                    Attack followUp = Attack.CreateAttack(castor, currentAttack, it, other, dec?.Clone());
                    followUp.SelfAttackStatChange = attackIncreasse;
                    followUp.SelfSpeedStatChange = speedIncrease;
                    followUp.Target = currentAttack.Target;
                    followUp.Castor = currentAttack.Castor;
                    currentAttack.Description = currentAttack.Castor.Name + " senses some blood in the air";
                    followUp.ExecuteAttack += (CombatEntity c2, CombatEntity t2) =>
                    {
                        followUp.Description = followUp.Castor.Name + "'s speed greatly increased";
                        UtilAttackFactory.GenerateFollowUpTextAttack(followUp, followUp.Castor.Name + "'s attack increased!");
                        castor.RemoveTempAttack(SenseBlood);
                    };
                }
                else
                {
                    currentAttack.CommandReturnResult = it.ExecuteNext(c, t);
                }
            };
            return currentAttack;
        }

        public static Attack SenseFire(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec= null)
        {
            
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.EOT;
            currentAttack.Name = "Sense Fire";
            currentAttack.ExecuteAttack = (CombatEntity c, CombatEntity t) =>
            {
                if (other.Burn > 0)
                {
                    Attack followUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                    followUp.SelfSpeedStatChange = followUp.Target.Burn;

                    currentAttack.Description = currentAttack.Castor.Name + " senses some fire in the air";
                    followUp.ExecuteAttack += (CombatEntity c2, CombatEntity t2) =>
                    {
                        followUp.Description = followUp.Castor.Name + "'s speed increased";

                        Attack followup2 = Attack.CreateFollowupAttack(followUp, it, dec);
                        followup2.SelfAttackStatChange = followup2.Target.Burn / 10;
                        followup2.ExecuteAttack += (CombatEntity c3, CombatEntity t3) =>
                         {
                             followup2.Description = $"{followup2.Castor.Name} attack increased!";
                         };
                    };
                }
                else
                {
                    currentAttack.CommandReturnResult = it.ExecuteNext(c, t);
                }
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
