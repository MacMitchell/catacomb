using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{

    /**
     * when creating attacks, please put the base values at the start of the attack. Easier to change in the future 
     */
    class AttackFactory
    {
        public static double offset(double stat)
        {
            return stat - Global.Globals.BASE_ATTACK_STAT;
        }

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
        public static Attack Tackle(CombatEntity castor,Command parent)
        {
            
            double baseDamage = 10;
            //every 10 attack over base makes this do 1 more
            double offset = AttackFactory.offset(castor.AttackStat);
            double damage = baseDamage + offset / 10.0;
             
            

            Attack currentAttack = new Attack(parent);
            
            currentAttack.Damage = damage;
            currentAttack.Name = "Tackle";
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " tackled " + currentAttack.Target.Name +"!";
            };

            return currentAttack;
        }

        public static Attack FireBall(CombatEntity castor, Command parent)
        {
            double baseDamage = 10;
            double offset = AttackFactory.offset(castor.MagicStat);
            baseDamage += offset/10.0;

            Attack currentAttack = new Attack(parent);
            currentAttack.Damage = baseDamage;
            currentAttack.Name = "Fireball";
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " throw a fireball at " + currentAttack.Target.Name + "!";
            };
            return currentAttack;
        }

        /**
         * NOTE: This attack will not heal you if it kills the monster 
         */
        public static Attack Leech(CombatEntity castor, Command parent)
        {
            double baseDamage = 10;
            double healToDamgeRatio = 1.0;

            double offset = AttackFactory.offset(castor.MagicStat);
            baseDamage += offset / 10.0;

            Attack currentAttack = new Attack(parent);
            currentAttack.Damage = baseDamage;
            currentAttack.Name = "Leech";

            currentAttack.ExecuteAttack = (CombatEntity c, CombatEntity t) =>
            {
                double targetHealthBefore = currentAttack.Target.Health;
                currentAttack.InflectDamage(currentAttack.Target, currentAttack.Damage);

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

        public static Attack Bulster(CombatEntity castor, Command parent)
        {
            double baseIncrease = 10;
            string name = "Bulster";

            Attack delcareAttack = CreateDeclareCommand(castor, name, parent);

            Attack currentAttack = new Attack(delcareAttack);
            currentAttack.SelfDefenseStatChange = baseIncrease;
            currentAttack.ExecuteAttack += (CombatEntity c, CombatEntity t) =>
            {
                currentAttack.Description = currentAttack.Castor.Name + " raised their defense by " + baseIncrease + "!";
            };
            return delcareAttack;
        }

        
        
    }
}
