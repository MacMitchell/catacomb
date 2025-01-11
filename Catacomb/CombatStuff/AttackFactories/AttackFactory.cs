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
        private static Attack CreateDeclareCommand(CombatEntity castor, string name, Command parent, string description = "Hello World")
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


        public static Attack Tackle(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {

            double baseDamage = 10;
            //every 10 attack over base makes this do 1 more
            double damage = baseDamage + castor.AttackStat / 10.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);//new Attack(parent);

            currentAttack.Damage = damage;
            currentAttack.Name = "Tackle";
            currentAttack.DamageType = DType.physical;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " tackled " + currentAttack.Target.Name + "!";
            };

            return currentAttack;
        }

        public static Attack FireBall(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;

            baseDamage += castor.MagicStat / 10.0;

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

            double targetHealthBefore = other != null ? other.Health : 0;

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
        public static Attack ArmorBreak(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null){
            double baseDamage = 10;
            baseDamage += castor.AttackStat / 4;

            double baseDefenceDecrease = 5;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Damage = baseDamage;
            currentAttack.DefenseStatChange = -1 * baseDefenceDecrease;

            currentAttack.Name = "Armor Break";
            currentAttack.DamageType = DType.physical;
            currentAttack.Type = AttackType.A;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} unleashes a powerful and breaks some of {currentAttack.Target.Name}'s defence!";

            };

            return currentAttack;
        }




        /**
         * Gains low damage but also gains some armor 
         */
        public static Attack ArmoredStrike(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;
            baseDamage += castor.AttackStat / 3;

            double baseArmor = 8;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Damage = baseDamage;
            currentAttack.Name = "Armored Strike";
            currentAttack.DamageType = DType.physical;
            currentAttack.Type = AttackType.A;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} + strikes at {currentAttack.Target.Name}!";
                Attack armorFollowUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                armorFollowUp.SelfArmorChange = baseArmor;
                armorFollowUp.ExecuteAttack += (CombatEntity c2, CombatEntity o2) =>
                 {
                     armorFollowUp.Description = $"{currentAttack.Castor.Name} gained some armor";
 
                 };
            };

            return currentAttack;
        }


        /**
         * Deals poor physical damage, but has a chance to slightly decrease armor  UNTESTED
         */
        public static Attack Bite(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 10;

            baseDamage += castor.AttackStat / 2;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);

            currentAttack.Damage = baseDamage;
            currentAttack.Name = "Bite";
            currentAttack.DamageType = DType.physical;
            currentAttack.Type = AttackType.A;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
             {
                 currentAttack.Description = currentAttack.Castor.Name + " bites " + currentAttack.Target.Name + "!";
                 if (Global.Globals.Rand.Next(0, 4) == 3)
                 {
                     Attack followUp = Attack.CreateAttack(castor, currentAttack, it, other, dec?.Clone());
                     followUp.Target = currentAttack.Target;
                     followUp.DefenseStatChange = -5;
                     followUp.ExecuteAttack += (CombatEntity c2, CombatEntity t2) =>
                     {
                         followUp.Description = "The bite shredded some of " + followUp.Target.Name + " armor!";
                     };
                 }
             };

            return currentAttack;
        }


        public static Attack Blizzard(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Blizzard";
            currentAttack.Type = AttackType.A;
            currentAttack.SelfManaDrain = -20;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} creates a blizzard";
                currentAttack.Castor.AddTempAttack(TurnBasedAttackFactory.BlizzardPassive);
            };
            return currentAttack;
        }

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


        /**
         * UNTESTED
         * */
        public static Attack DefensiveStance(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseArmor = 15;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);//new Attack(parent);

            currentAttack.SelfArmorChange = baseArmor;
            currentAttack.Name = "Defensive Stance";
            currentAttack.Type = AttackType.A;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} assumes a defensive stance!";
                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, $"{currentAttack.Castor.Name} armor increased!");
            };

            return currentAttack;
        }

        /**
        * UNTESTED 
        */
        public static Attack EnchantmentBurn(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Enchantment Burn";
            currentAttack.Type = AttackType.A;
            currentAttack.SelfManaDrain = -5;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) => {
                currentAttack.Description = $"{currentAttack.Castor.Name}'s enchantents their weapon with burning fire!";

                //adds the poison touch decorator for each attackstat /50. At least once
                for (int i = 0; i < Math.Ceiling((currentAttack.Castor.AttackStat + currentAttack.Castor.MagicStat) / 50); i++)
                {
                    currentAttack.Castor.AddTempAttackDecorator(AttackDecFactory.BurningTouch);
                }
            };

            return currentAttack;
        }


        /**
        * UNTESTED 
        */
        public static Attack EnchantmentCorrosive(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Enchantment Corrosive";
            currentAttack.Type = AttackType.A;
            currentAttack.SelfManaDrain = -5;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) => {
                currentAttack.Description = $"{currentAttack.Castor.Name}'s enchantents their weapon with armor corroding acid!";

                //adds the poison touch decorator for each attackstat /50. At least once
                for (int i = 0; i < Math.Ceiling((currentAttack.Castor.AttackStat + currentAttack.Castor.MagicStat) / 50); i++)
                {
                    currentAttack.Castor.AddTempAttackDecorator(AttackDecFactory.CorrosiveTouch);
                }
            };

            return currentAttack;
        }

        /**
        * UNTESTED 
        */
        public static Attack EnchantmentPoison(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Enchantment Poison";
            currentAttack.Type = AttackType.A;
            currentAttack.SelfManaDrain = -5;


            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) => {
                currentAttack.Description = $"{currentAttack.Castor.Name}'s enchantents their weapon with poison";

                //adds the poison touch decorator for each attackstat /50. At least once
                for (int i = 0; i < Math.Ceiling((currentAttack.Castor.AttackStat + currentAttack.Castor.MagicStat) / 50); i++)
                {
                    currentAttack.Castor.AddTempAttackDecorator(AttackDecFactory.PoisonTouch);
                }
            };

            return currentAttack;
        }



        public static Attack FieryBite(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 20;
            baseDamage += castor.AttackStat / 2.0;

            double baseBurn = 10;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Fiery Bite";
            currentAttack.DamageType = DType.physical;
            currentAttack.Type = AttackType.A;

            currentAttack.Damage = baseDamage;
            currentAttack.Burn = baseBurn;
            currentAttack.SelfManaDrain = -5;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o2) =>{
                currentAttack.Description = $"{currentAttack.Castor.Name} bites {currentAttack.Target.Name} with their fangs of fire";
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

        public static Attack FireBreath(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double burnAmount = 20;
            double baseDamage = 15;
            baseDamage += castor.MagicStat;


            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Fire Breath";
            currentAttack.DamageType = DType.magic;
            currentAttack.SelfManaDrain = -25;
            currentAttack.Damage = baseDamage;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " breaths fire at " + currentAttack.Target.Name + "!";

                if (Global.Globals.Rand.NextDouble() > 0.75)
                {
                    Attack followup = Attack.CreateFollowupAttack(currentAttack, it, dec);
                    followup.Burn = burnAmount;
                    followup.ExecuteAttack += (CombatEntity c2, CombatEntity o) =>
                     {
                         followup.Description = $"{currentAttack.Target.Name} got burned!";
                     };
                }
            };

            return currentAttack;
        }

        public static Attack FireLance(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 15;
            baseDamage += castor.MagicStat / 3;

            double burnAmount = 10;
            double burnChance = 0.5;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Fire Lance";
            currentAttack.Type = AttackType.A;
            currentAttack.DamageType = DType.magic;
            currentAttack.Damage = baseDamage;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} tossed a lance of fire at {currentAttack.Target.Name}!";

                if(Global.Globals.Rand.NextDouble() < burnChance)
                {
                    Attack followUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                    followUp.Burn = burnAmount;
                    followUp.ExecuteAttack += (CombatEntity c2, CombatEntity o2) =>
                    {
                        followUp.Description = $"{currentAttack.Target.Name} got burned!";
                    };
                }
            };

            return currentAttack;
        }

        public static Attack FlamingCharge(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 20;
            baseDamage += castor.AttackStat / 2.0;

            double baseSpeedIncrease = 25;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Flaming Charge";
            currentAttack.Type = AttackType.A;
            currentAttack.DamageType = DType.physical;

            currentAttack.Damage = baseDamage;
            currentAttack.SelfManaDrain = -5;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
             {
                 currentAttack.Description = $"{currentAttack.Castor.Name} wraps themself in fire and charges at {currentAttack.Target.Name}";

                 Attack followUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                 followUp.SelfSpeedStatChange = baseSpeedIncrease;
                 followUp.ExecuteAttack += (CombatEntity c2, CombatEntity o2) =>
                 {
                     followUp.Description = $"{followUp.Castor.Name} increased their speed";
                 };

             };

            return currentAttack;
        }


        public static Attack FrostBlast(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 15;
            baseDamage += castor.MagicStat / 3;

            double baseSlow = castor.MagicStat / 10;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Frost Blast";
            currentAttack.Type = AttackType.A;
            currentAttack.DamageType = DType.magic;

            currentAttack.Damage = baseDamage;
            currentAttack.SpeedStatChange -= baseSlow;
            currentAttack.SelfManaDrain = -15;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} unleashes a massive blast of frost at {currentAttack.Target.Name}!";
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


        /*
         * Slightly lowers the targets attack UNTESTED
         */
        public static Attack Growl(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseAttackDecrease = 5;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);

            currentAttack.Name = "Growl";
            currentAttack.AttackStatChange = -baseAttackDecrease;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " growls!";
                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, currentAttack.Target.Name +"'s attack fell!");
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
         * Deals bonus damage if the target is slower than castor 
         */
        public static Attack IceBeam(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 15;
            baseDamage += castor.MagicStat / 3.0;
            double damageIncrease = 20;
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Ice Beam";
            currentAttack.Type = AttackType.A;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} starts firing a beam of ice at {currentAttack.Target.Name}!";
                Attack damageFollowUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                damageFollowUp.DamageType = DType.magic;
                damageFollowUp.Damage = currentAttack.Castor.Speed > currentAttack.Target.Speed ? baseDamage + damageIncrease : baseDamage;
                damageFollowUp.ExecuteAttack += (CombatEntity c2, CombatEntity o2) =>
                {
                    damageFollowUp.Description = currentAttack.Castor.Speed > currentAttack.Target.Speed ? $"direct hit!" : $"it grazes {currentAttack.Target.Name}, but still hurts!";
                };
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

        public static Attack LavaBomb(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double damage = 10;
            double burn = 10;
            damage += castor.MagicStat / 2;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Type = AttackType.A;
            currentAttack.DamageType = DType.magic;
            currentAttack.Name = "Lava Bomb";

            currentAttack.Damage = damage;
            currentAttack.Burn = burn;
            currentAttack.SelfManaDrain = -20;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
            {
                currentAttack.Description = $"{currentAttack.Castor.Name} jumps into a pool of lava, flinging lava at {currentAttack.Target.Name}!";

                Attack selfDamage = Attack.CreateSelfTargetAttack(currentAttack, it, currentAttack.Castor);
                selfDamage.Burn = burn;
                selfDamage.ExecuteAttack += (CombatEntity c2, CombatEntity o2) =>
                {
                    selfDamage.Description = $"{currentAttack.Castor.Name} burns itself in the lava";
                };
            };

            return currentAttack;
        }

        public static Attack MakeshiftMagic(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseMagicStatRaise = 6;
            double baseMagicResistStatRaise = 6;
            double baseMidDamage = 15;
            double baseHighDamage = 30;


            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            const string metaKey = "makeshift_magic";
            currentAttack.Name = "Makeshift Magic";
            currentAttack.Type = AttackType.A;

            if (!castor.metadata.ContainsKey(metaKey))
            {
                //buff castor
                currentAttack.SelfMagicResistStatChange = baseMagicResistStatRaise;
                currentAttack.SelfMagicAttackStatChange = baseMagicStatRaise;
                currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity c2) =>
                {
                    currentAttack.Description = currentAttack.Castor.Name + " casts some homemade magic!";
                    UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, currentAttack.Castor.Name + " magic increased!");
                    UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, currentAttack.Castor.Name + " magic resist increased!");
                    castor.metadata[metaKey] = 1;

                };


            }
            else if (Convert.ToInt32(castor.metadata[metaKey]) == 1)
            {
                //deal decent damage
                currentAttack.DamageType = DType.magic;
                currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity c2) =>
                {
                    currentAttack.Description = currentAttack.Castor.Name + " casts some homemade magic!";
                    Attack followUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                    followUp.Damage = baseMidDamage + currentAttack.Castor.MaxMagicStat / 3;
                    followUp.DamageType = DType.magic;
                    followUp.ExecuteAttack += (CombatEntity c3, CombatEntity c4) =>{
                        followUp.Description = followUp.Castor.Name + "'s homemade magic actuals hurts " + followUp.Target.Name + "!";
                    };
                    castor.metadata[metaKey] = 2;

                };


            }
            else
            {
                //big damage
                currentAttack.DamageType = DType.magic;
                currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity c2) =>
                {
                    currentAttack.Description = currentAttack.Castor.Name + " casts some homemade magic!";
                    Attack followUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                    followUp.Damage = baseHighDamage + currentAttack.Castor.MaxMagicStat;
                    followUp.DamageType = DType.magic;
                    followUp.ExecuteAttack += (CombatEntity c3, CombatEntity c4) => {
                        followUp.Description = followUp.Castor.Name + " homemade magic unleashes a huge wave of arcane!!!!";
                    };
                    castor.metadata.Remove(metaKey);
                };

            }

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

        /**
         * UNTESTED
         * */
        public static Attack RaiseGoblinArmylDamage(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Raise Goblin Army Damage";
            currentAttack.Type = AttackType.A;
            
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o)=>{
                currentAttack.Description = $"{currentAttack.Castor.Name} shouts with rage!";
                currentAttack.Castor.AddTempAttackDecorator(AttackDecFactory.GoblinArmyRage);
                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, $"{currentAttack.Castor.Name}'s army might grows stronger");
            };

            return currentAttack;
        }

        /**
         * UNTESTED
         * */
        public static Attack RaiseGoblinArmylPoison(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Raise Goblin Army Poison";
            currentAttack.Type = AttackType.A;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) => {
                currentAttack.Description = $"{currentAttack.Castor.Name}'s casts a spell buffing their army!";
                currentAttack.Castor.AddTempAttackDecorator(AttackDecFactory.PoisonTouch);
                UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, $"{currentAttack.Castor.Name}'s army weapons grow with a poisonous light");
            };

            return currentAttack;
        }

        public static Attack SalamderReset(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double heal = castor.Burn/4;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Salamder Reset";
            currentAttack.Type = AttackType.A;
            currentAttack.SelfManaDrain = -100;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity o) =>
             {
                 currentAttack.Castor.AttackStat = currentAttack.Castor.MaxAttackStat;
                 currentAttack.Castor.MagicStat = currentAttack.Castor.MaxMagicStat;
                 currentAttack.Castor.Defense = currentAttack.Castor.MaxDefense;
                 currentAttack.Castor.MagicResist = currentAttack.Castor.MaxMagicResist;

                 currentAttack.Description = $"{currentAttack.Castor.Name} takes a breath to reset itself";
                 Attack followUp = Attack.CreateFollowupAttack(currentAttack, it, dec);
                 followUp.SelfHeal = heal;
                 followUp.ExecuteAttack += (CombatEntity c2, CombatEntity o2) =>
                 {
                     followUp.Castor.Burn = 0;
                     followUp.Description = $"{currentAttack.Castor.Name} cleans off its burns, healing itself";
                 };
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


        //UNTESTED
        public static Attack Slam(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 15;
            baseDamage += castor.AttackStat / 3.0;
            double baseSlow = 0;
            
            bool slow = Global.Globals.Rand.Next(0, 4) == 3;
            if (slow)
            {
                baseSlow = 20;
            }

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Slam";
            currentAttack.Damage = baseDamage;
            currentAttack.SpeedStatChange = -baseSlow;
            currentAttack.DamageType = DType.physical;
            currentAttack.Type = AttackType.A;

            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = castor.Name + " slams " + currentAttack.Target.Name + "!";
                if (currentAttack.SpeedStatChange != 0)
                {
                    UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, currentAttack.Target.Name + " speed fell!");
                }
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



        /**
         * UNTESTED
         * */
        public static Attack Smash(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            double baseDamage = 20;
           
            baseDamage += castor.AttackStat / 2.0;

            Attack currentAttack = Attack.CreateAttack(castor, parent, it, other, dec);
            currentAttack.Name = "Smash";
            currentAttack.DamageType = DType.physical;
            currentAttack.Type = AttackType.A;


            currentAttack.ExecuteAttack += (CombatEntity no, CombatEntity o) =>
            {
                currentAttack.Description = castor.Name + " slowly raises their weapon high and smashes it down...";
                int hitChance = Global.Globals.Rand.Next(0, 101);
                double speedDifference = (currentAttack.Target.Speed - currentAttack.Castor.Speed)/10;
                //if castor is faster, hit
                //else it is 60% chance -  (target.speed-castor.speed/10)
                if (currentAttack.Castor.Speed > currentAttack.Target.Speed || hitChance < 60 - speedDifference)
                {
                    Attack followUp = Attack.CreateAttack(castor, currentAttack, it, other, dec?.Clone());
                    followUp.Target = currentAttack.Target;
                    followUp.Damage = baseDamage;
                    followUp.DamageType = DType.physical;
                    followUp.ExecuteAttack += (CombatEntity n2, CombatEntity n3) =>
                    {
                        followUp.Description = "... and it smashes into " + followUp.Target.Name + "!";
                    };
                }
                else
                {
                    UtilAttackFactory.GenerateFollowUpTextAttack(currentAttack, "... and "+ currentAttack.Castor.Name + " missed!");

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
