using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    public class Attack : Command
    {
		private double damage; //the damage the attack does (+:reduces health)
		private double selfHeal; //how much the one is attacking will heal (negative value will cause self damage)
		private double poison; //how much the attack will poison its target (+: increases poison)
		private double mentalBreak; //how much mentalbreak the attack will apply to the target (+: increases mental break)
		private double armorChange; //how much will the attack change the target armor after the damage is done (+: reduces armor)
		private double selfArmorChange; //how much the attack will change the armor of the user (+: reduces armor)
		private double defenseStatChange; //how much the attack will change the defense value of its target (+:reduces)
		private double magicResistStatChange;//how much the attack will change the magic resist of its target(+: reduces)
		private double selfMagicResistStatChange; //(+: reduces)
		private double selfDefenseStatChange; //how much the attack will change the defense of the user (+: reduces)
		private double attackStatChange; //how much the attack will change the attack value of its target (+: reduces)
		private double selfAttackStatChange; //how much the attack will change the attack value of the user (+: reduces)
		private double magicAttackStatChange; // (+: reduces )
		private double selfMagicAttackStatChange; //(+: reduces)
		private double speedStatChange;//changes the speed stat of the target (+: reduces)
		private double selfSpeedStatChange; //(+: reduces)
        private List<Attack> children;
        private string name;
        private string description;
        public Attack(Command inputCommand = null):base(inputCommand)
        {
            this.damage = 0;
            this.selfHeal = 0;
            this.poison = 0;
            this.mentalBreak = 0;
            this.armorChange = 0;
            this.selfArmorChange = 0;
            this.defenseStatChange = 0;
            this.magicResistStatChange = 0;
            this.selfMagicResistStatChange = 0;
            this.selfDefenseStatChange = 0;
            this.attackStatChange = 0;
            this.selfAttackStatChange = 0;
            this.magicAttackStatChange = 0;
            this.selfMagicAttackStatChange = 0;
            this.speedStatChange = 0;
            this.selfSpeedStatChange = 0;
        }

        public double Damage { get => damage; set => damage = value; }
        public double SelfHeal { get => selfHeal; set => selfHeal = value; }
        public double Poison { get => poison; set => poison = value; }
        public double MentalBreak { get => mentalBreak; set => mentalBreak = value; }
        public double ArmorChange { get => armorChange; set => armorChange = value; }
        public double SelfArmorChange { get => selfArmorChange; set => selfArmorChange = value; }
        public double DefenseStatChange { get => defenseStatChange; set => defenseStatChange = value; }
        public double MagicResistStatChange { get => magicResistStatChange; set => magicResistStatChange = value; }
        public double SelfMagicResistStatChange { get => selfMagicResistStatChange; set => selfMagicResistStatChange = value; }
        public double SelfDefenseStatChange { get => selfDefenseStatChange; set => selfDefenseStatChange = value; }
        public double AttackStatChange { get => attackStatChange; set => attackStatChange = value; }
        public double SelfAttackStatChange { get => selfAttackStatChange; set => selfAttackStatChange = value; }
        public double MagicAttackStatChange { get => magicAttackStatChange; set => magicAttackStatChange = value; }
        public double SelfMagicAttackStatChange { get => selfMagicAttackStatChange; set => selfMagicAttackStatChange = value; }
        public double SpeedStatChange { get => speedStatChange; set => speedStatChange = value; }
        public double SelfSpeedStatChange { get => selfSpeedStatChange; set => selfSpeedStatChange = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public override void Execute(CombatEntity castor, CombatEntity target)
        {

            target.Armor += armorChange;
            castor.Armor += selfArmorChange;
            
            target.Defense += defenseStatChange;
            castor.Defense += selfDefenseStatChange;

            target.MagicResist += magicAttackStatChange;
            castor.MagicResist += selfMagicAttackStatChange;

            target.AttackStat += attackStatChange;
            castor.AttackStat += selfAttackStatChange;

            target.MagicStat += magicAttackStatChange;
            castor.MagicStat += selfMagicAttackStatChange;

            target.Speed += speedStatChange;
            castor.Speed += selfSpeedStatChange;

            if (damage > 0) {
                InflectDamage(target, damage);
            }
            else
            {
                Heal(target, damage * -1);
            }

            //if damages itself
            if (selfHeal < 0)
            {
                InflectDamage(castor, selfHeal*-1);
            }
            else
            {
                Heal(castor, selfHeal);
            }
        }

        /**
         * Heals the entity passed. If it is a negative value it will damage the player 
         */
        protected virtual void Heal(CombatEntity target, double healAmount)
        {
            target.Health = target.Health + healAmount;
            if(target.Health > target.MaxHealth)
            {
                target.Health = target.MaxHealth;
            }
        }
        protected virtual void InflectDamage( CombatEntity target,double damage)
        {
            if(damage == 0)
            {
                return;
            }

            //attack the armor first
            if(target.Armor > 0)
            {
                double tempDamage = damage;
                damage -= target.Armor;
                target.Armor -= tempDamage;
            }
            //no damage gets through the armor
            if(damage <= 0)
            {
                return;
            }

            target.Health -= damage;
        }
    }
}
