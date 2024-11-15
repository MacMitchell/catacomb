using Catacomb.CombatStuff.AttackFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Catacomb.CombatStuff
{

    /**
     * when creating attacks, please put the base values at the start of the attack. Easier to change in the future 
     * the function returned here should be able to be called with all null values. If you need to use a entity name, put in the execute
     */
    class AttackFactory
    {


        /**
         * Creates a attack that just prints the name 
         */
        private static Attack CreateDeclareCommand(CombatEntity castor, string name, Command parent,string description = "Hello World")
        {
            Attack blank = new Attack(parent);
            blank.Name = name;
            blank.Description = description;

            blank.ExecuteAttack = (CombatEntity UNUSED, CombatEntity NOPE) => 
            { 
                blank.Description = blank.Castor.Name + " used " + name + "!";
            };
            return blank;
            
        }

        
        public static Attack Tackle(CombatEntity castor,Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec= null)
        {
            
            double baseDamage = 10;
            //every 10 attack over base makes this do 1 more
            double damage = baseDamage +  castor.AttackStat/ 10.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);//new Attack(parent);

            currentAttack.Damage = damage;
            currentAttack.Name = "Tackle";
            currentAttack.DamageType = DType.physical;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " tackled " + currentAttack.Target.Name +"!";
            };

            return currentAttack;
        }

        public static Attack FireBall(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;
       
            baseDamage += castor.MagicStat/10.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);//new Attack(parent);
            currentAttack.Damage = baseDamage;
            currentAttack.Name = "Fireball";
            currentAttack.DamageType = DType.physical;
            currentAttack.SelfManaDrain = -5;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " throw a fireball at " + currentAttack.Target.Name + "!";
            };
            return currentAttack;
        }

        /**
         * @EXAMPLE: healing and displaying the healing amount
         * NOTE: This attack will not heal you if it kills the monster 
         */
        public static Attack Leech(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;
            double healToDamgeRatio = 1.0;

            baseDamage += castor.MagicStat / 5.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Damage = baseDamage;
            currentAttack.Name = "Leech";
            currentAttack.DamageType = DType.magic;

            double targetHealthBefore = other != null ? other.Health: 0;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {

                double healthAfter = currentAttack.Target.Health;
                double healAmount = targetHealthBefore - healthAfter;
                healthAfter = Math.Max(0, healAmount);


                Attack followUp = new Attack(currentAttack);
                followUp.Castor = currentAttack.Castor;
                followUp.Target = currentAttack.Target;

                followUp.Damage = 0;
                followUp.SelfHeal = healthAfter * healToDamgeRatio;
                followUp.ExecuteAttack += (CombatEntity UNUSED, CombatEntity UNSUED2) =>
                {
                    followUp.Description = currentAttack.Castor.Name + " Healed " + healthAfter;
                };

            };
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " stole some life from " + currentAttack.Target.Name + "!";
            };
            return currentAttack;
        }

        public static Attack Bulster(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseIncrease = 10;
            string name = "Bulster";

            Attack delcareAttack = CreateDeclareCommand(castor, name, parent);

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);//new Attack(delcareAttack);
            currentAttack.SelfDefenseStatChange = baseIncrease;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " raised their defense by " + baseIncrease + "!";
            };
            return delcareAttack;
        }

        /**=============================================================
         *EXAMPLES END ALPHABETICAL AFTER
         *==============================================================*/


        public static Attack Combustion(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {

            double baseDamage = 20;
            //every 10 attack over base makes this do 1 more
            double damage = baseDamage + castor.MagicStat;


            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);//new Attack(parent);

            currentAttack.Damage = damage;
            currentAttack.Name = "Combustion";
            currentAttack.DamageType = DType.magic;
            currentAttack.SelfHeal = castor.MaxHealth * -2;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " explodes!!!";
            };

            return currentAttack;
        }
        /*
         * Deals good damage, has a chance to burn the target 
         */
        public static Attack FireBlast(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double percent = 1.0;
            double burnAmount = 10;


            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "FireBlast";
            currentAttack.Damage = castor.MagicStat * percent;
            currentAttack.DamageType = DType.magic;
            currentAttack.SelfManaDrain = -45;
            if (Global.Globals.Rand.Next(1, 3) == 2)
            {
                currentAttack.Burn = burnAmount;
            }
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " engulfed " + currentAttack.Target.Name +" in flames!";

                if(currentAttack.Burn > 0)
                {
                    UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, currentAttack.Target.Name + " was burned!");
                }
            };

            return currentAttack;
        }

       
        /**
         * Deals decent damage, slows target for % 
         */
        public static Attack FrostLance(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 25;
            baseDamage += castor.MagicStat / 10.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);//new Attack(parent);
            currentAttack.Damage = baseDamage;
            currentAttack.Name = "FrostLance";
            currentAttack.DamageType = DType.magic;

            currentAttack.ExecuteAttack = (CombatEntity c, CombatEntity t) =>
            {
                double targetHealthBefore = currentAttack.Target.Health;
                double armorBefore = currentAttack.Target.Armor;
                double speedBefore = currentAttack.Target.Speed;
                currentAttack.SpeedStatChange = currentAttack.Target.Speed * -0.05;

                currentAttack.DefaultExecuteAttack(currentAttack.Castor, currentAttack.Target);

                double speedAfter = currentAttack.Target.Speed;
                double healthAfter = currentAttack.Target.Health;
                double armorAfter = currentAttack.Target.Armor;

                double damageDifference = targetHealthBefore - healthAfter;
                double speedDifference = Math.Abs(speedBefore - speedAfter);
                double armorDifference = Math.Abs(armorBefore - armorAfter);
                //dialog for getting the damage done
                Attack followUp = new Attack(currentAttack);
                followUp.Castor = currentAttack.Castor;
                followUp.Target = currentAttack.Target;

                followUp.Damage = 0;
                
                followUp.ExecuteAttack += (CombatEntity UNUSED, CombatEntity UNSUED2) =>
                {
                    followUp.Description = currentAttack.Castor.Name + " dealt " + (damageDifference+armorDifference )+ " to " + currentAttack.Target.Name + "!";
                };

                //dialog for getting how much the target was slowed by
                Attack speedFollowUp = new Attack(currentAttack);
                speedFollowUp.Castor = currentAttack.Castor;
                speedFollowUp.Target = currentAttack.Target;

                speedFollowUp.Damage = 0;

                speedFollowUp.ExecuteAttack += (CombatEntity UNUSED, CombatEntity UNSUED2) =>
                {
                    speedFollowUp.Description = currentAttack.Target.Name + " speed fell by " + speedDifference  + "!";
                };

            };
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " used Fronst Lance!";
            };
            return currentAttack;
        }


        /**
        * Deals some base damage. Also deals the damge the burn should.
        */
        public static Attack Heatwave(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 15;
            baseDamage += castor.MagicStat / 3.0;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Heatwave";
            currentAttack.Damage = baseDamage;
            currentAttack.Type = AttackType.A;
            currentAttack.DamageType = DType.magic;
            currentAttack.SelfManaDrain = -20;
            currentAttack.ExecuteAttack += (CombatEntity no, CombatEntity o) =>
            {
                currentAttack.Description = castor.Name + " directs a painful heatwave at " + currentAttack.Target.Name;
                if (currentAttack.Target.Burn > 0) {
                    Attack followUp = Attack.CreateAttack(castor, currentAttack, it, other, dec?.Clone());
                    followUp.Target = currentAttack.Target;
                    followUp.Damage = followUp.CalculateBurnDamage( currentAttack.Target.Burn, currentAttack.Target);
                    followUp.DamageType = DType.pierce;
                    followUp.ExecuteAttack += (CombatEntity n2, CombatEntity n3) =>
                    {
                        followUp.Description = "The heatwave causes " + currentAttack.Target.Name + "'s burns to burn!";
                    };
                }

            };
            return currentAttack;
        }
        /**
         * increases target's burn by 10 
         */
        public static Attack Ignite(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseBurn = 10;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Ignite";
            currentAttack.Burn = baseBurn;
            currentAttack.Type = AttackType.A;
            currentAttack.SelfManaDrain = -20;
            currentAttack.ExecuteAttack += (CombatEntity no, CombatEntity o) =>
            {
                currentAttack.Description = castor.Name + " set " + currentAttack.Target.Name + " on fire!";
                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, currentAttack.Target.Name + "'s burn increased.");
            };
            return currentAttack;
        }
        public static Attack HeatingClaws(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 8;

            baseDamage += castor.AttackStat / 3.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Heating Claws";
            currentAttack.Damage = baseDamage;
            currentAttack.Type = AttackType.A;
            currentAttack.DamageType = DType.physical;

            currentAttack.ExecuteAttack += (CombatEntity no, CombatEntity o) =>
            {
                currentAttack.Description = castor.Name + " slashed their claws at " + currentAttack.Target.Name + "!";
                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, castor.Name + "'s claws are heating up");
                castor.AddTempAttackDecorator(AttackDecFactory.HeatedClaws);
            };
            return currentAttack;
        }

        public static Attack PoisonSlash(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;
            double poisonDamage = 2;

            baseDamage += castor.AttackStat / 2.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Poison Slash";
            currentAttack.Damage = baseDamage;
            currentAttack.Type = AttackType.A;
            currentAttack.DamageType = DType.physical;
            currentAttack.Poison = poisonDamage;

            currentAttack.ExecuteAttack += (CombatEntity no, CombatEntity o) =>
            {
                currentAttack.Description = castor.Name + " slashed at " + currentAttack.Target.Name + "!";
                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, currentAttack.Target.Name +" was poisoned by " + currentAttack.Poison + "!");

            };
            return currentAttack;
        }

        public static Attack SelfIgnition(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseBurn = 50;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Self Ignition";
            currentAttack.Type = AttackType.A;
            currentAttack.Burn = baseBurn;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " sets EVERYONE on fire!";
                Attack followup = Attack.CreateAttack(castor, currentAttack, it, castor, castor.CreateEntityDecorator(null, true));
                followup.Target = castor;
                followup.Burn = baseBurn;
                followup.ExecuteAttack += (CombatEntity co, CombatEntity oo) =>{
                    followup.Description = currentAttack.Castor.Name + " sets themself on fire";

                };
            };

            return currentAttack;
        }



        public static Attack Slash(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;
            bool crit = Global.Globals.Rand.Next(0, 4) == 3;
            baseDamage += castor.AttackStat / 2.0;
            if (crit)
            {
                baseDamage *= 2;
            }

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Slash";
            currentAttack.Damage = baseDamage;
            currentAttack.DamageType = DType.physical;
            currentAttack.Type = AttackType.A;


            currentAttack.ExecuteAttack += (CombatEntity no, CombatEntity o) =>
            {
                currentAttack.Description = castor.Name + " slashed at " + currentAttack.Target.Name + "!";
                if (crit)
                {
                    UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack,"Critical hit!");
                }
            };
            return currentAttack;
        }
        

        public static Attack SummonMeteor(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Summon Meteor";
            currentAttack.DamageType = DType.none;
            currentAttack.Type = AttackType.A;


            currentAttack.ExecuteAttack += (CombatEntity no, CombatEntity o) =>
            {
                if (currentAttack.Target.metadata.ContainsKey("meteor"))
                {
                    currentAttack.Description = "There already is a meteor in the sky. There is room for only one (per person).";
                }
                else
                {
                    currentAttack.Description = castor.Name + " summoned a meteor!!!";
                    currentAttack.Target.AttackTempAttack(TurnBasedAttackFactory.Meteor);
                }
            };
            return currentAttack;
        }
    }
}
