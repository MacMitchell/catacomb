using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    public enum DType //damage type
    {
        physical,
        magic,
        pierce,
        none,
    }
    public class Attack : Command
    {
        protected double damage; //the damage the attack does (+:reduces health)
        protected double selfHeal; //how much the one is attacking will heal (negative value will cause self damage)
        protected double poison; //how much the attack will poison its target (+: increases poison)
        protected double burn; //how much the attack will burn its target (+: increases poison)
        protected double mentalBreak; //how much mentalbreak the attack will apply to the target (+: increases mental break)
        protected double armorChange; //how much will the attack change the target armor after the damage is done (+: reduces armor)
        protected double selfArmorChange; //how much the attack will change the armor of the user (+: reduces armor)
        protected double defenseStatChange; //how much the attack will change the defense value of its target (+:reduces)
        protected double magicResistStatChange;//how much the attack will change the magic resist of its target(+: reduces)
        protected double selfMagicResistStatChange; //(+: reduces)
        protected double selfDefenseStatChange; //how much the attack will change the defense of the user (+: reduces)
        protected double attackStatChange; //how much the attack will change the attack value of its target (+: reduces)
        protected double selfAttackStatChange; //how much the attack will change the attack value of the user (+: reduces)
        protected double magicAttackStatChange; // (+: reduces )
        protected double selfMagicAttackStatChange; //(+: reduces)
        protected double speedStatChange;//changes the speed stat of the target (+: reduces)
        protected double selfSpeedStatChange; //(+: reduces)
        protected double manaDrain;
        protected double selfManaDrain;
        private int tired;
        private int selfTired;
        protected string name;
        public delegate void ExecuteAttackDelegate(CombatEntity castor, CombatEntity target);
        public ExecuteAttackDelegate ExecuteAttack;
        protected CombatEntity castor;
        protected CombatEntity target;

        private AttackType type = AttackType.A;
        private DType damageType;
        private 
        protected int commandReturnResult ;
        public int CommandReturnResult{
            set { commandReturnResult = value; }
            get { return commandReturnResult; }
        }

        public Attack(Command inputCommand = null) : base(inputCommand)
        {
            this.damage = 0;
            this.selfHeal = 0;
            this.poison = 0;
            this.burn = 0;
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
            this.selfManaDrain = 0;
            this.manaDrain = 0;
            this.tired = 0;
            this.selfTired = 0;

            CommandReturnResult = Command.IGNORE_COMMAND;
            castor = null;
            target = null;
            ExecuteAttack = DefaultExecuteAttack;
            DamageType = DType.pierce;
        }



        public virtual void DefaultExecuteAttack(CombatEntity castorIn, CombatEntity targetIn)
        {
            Target = targetIn;
            Castor = castorIn;

            Target.Armor += ArmorChange;
            Castor.Armor += SelfArmorChange;

            Target.Defense += DefenseStatChange;
            Castor.Defense += SelfDefenseStatChange;

            Target.MagicResist += MagicAttackStatChange;
            Castor.MagicResist += SelfMagicResistStatChange;

            Target.AttackStat += AttackStatChange;
            Castor.AttackStat += SelfAttackStatChange;

            Target.MagicStat += MagicAttackStatChange;
            Castor.MagicStat += SelfMagicAttackStatChange;

            Target.Speed += SpeedStatChange;
            Castor.Speed += SelfSpeedStatChange;

            Target.Mana += ManaDrain;
            Castor.Mana += SelfManaDrain;

            Target.Poison += Poison;
            Target.Burn += Burn;

            Target.Tired += Tired;
            Castor.Tired += SelfTired;
            if (Damage > 0)
            {
                InflectDamage(Target, Damage);
            }
            else
            {
                Heal(Target, Damage * -1);
            }

            //if damages itself
             if (selfHeal < 0)
            {
                castor.Health += selfHeal;
            }
            else
            {
                Heal(Castor, SelfHeal);
            }
        }
        public virtual double Damage { get => damage; set => damage = value; }
        public virtual double SelfHeal { get => selfHeal; set => selfHeal = value; }
        public virtual double Poison { get => poison; set => poison = value; }
        public virtual double MentalBreak { get => mentalBreak; set => mentalBreak = value; }
        public virtual double ArmorChange { get => armorChange; set => armorChange = value; }
        public virtual double SelfArmorChange { get => selfArmorChange; set => selfArmorChange = value; }
        public virtual double DefenseStatChange { get => defenseStatChange; set => defenseStatChange = value; }
        public virtual double MagicResistStatChange { get => magicResistStatChange; set => magicResistStatChange = value; }
        public virtual double SelfMagicResistStatChange { get => selfMagicResistStatChange; set => selfMagicResistStatChange = value; }
        public virtual double SelfDefenseStatChange { get => selfDefenseStatChange; set => selfDefenseStatChange = value; }
        public virtual double AttackStatChange { get => attackStatChange; set => attackStatChange = value; }
        public virtual double SelfAttackStatChange { get => selfAttackStatChange; set => selfAttackStatChange = value; }
        public virtual double MagicAttackStatChange { get => magicAttackStatChange; set => magicAttackStatChange = value; }
        public virtual double SelfMagicAttackStatChange { get => selfMagicAttackStatChange; set => selfMagicAttackStatChange = value; }
        public virtual double SpeedStatChange { get => speedStatChange; set => speedStatChange = value; }
        public virtual double SelfSpeedStatChange { get => selfSpeedStatChange; set => selfSpeedStatChange = value; }
        public virtual string Name { get => name; set => name = value; }
        public CombatEntity Castor { get => castor; set { if (castor == null) { castor = value; } } }
        public CombatEntity Target { get => target; set { if (target == null) { target = value; } } }

        public virtual double ManaDrain { get => manaDrain; set => manaDrain = value; }
        public virtual double SelfManaDrain { get => selfManaDrain; set => selfManaDrain = value; }
        public virtual double Burn { get => burn; set => burn = value; }
        public AttackType Type { get => type; set => type = value; }
        public virtual DType DamageType { get => damageType; set => damageType = value; }
        public virtual  int Tired { get => tired; set => tired = value; }
        public virtual int SelfTired { get => selfTired; set => selfTired = value; }

        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            ExecuteAttack(castor, target);
            return CommandReturnResult;
        }

        /**
         * Heals the entity passed. If it is a negative value it will damage the player 
         */
        public virtual void Heal(CombatEntity targetIn, double healAmount)
        {
            targetIn.Health = targetIn.Health + healAmount;
            if(targetIn.Health > targetIn.MaxHealth)
            {
                targetIn.Health = targetIn.MaxHealth;
            }
        }
        public virtual double CalculateDamage(double damage)
        {
            return damage;
        }

        public virtual double CalculateResistance(CombatEntity target, double damage)
        {
            return   DamageType == DType.pierce ? 0 : DamageType == DType.physical? target.Defense : target.MagicResist;
        }
        public virtual void InflectDamage( CombatEntity target,double damage)
        {
            damage = CalculateDamage(damage);
            damage -= CalculateResistance(target,damage);
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
                target.Armor = Math.Max(0, target.Armor);
            }
            //no damage gets through the armor
            if(damage <= 0)
            {
                return;
            }

            target.Health -= damage;
        }

        public virtual double CalculatePoisonDamage(double poisonAmount)
        {
            return poisonAmount;
        }
        public virtual double TakePoisonDamge(double poisonAmount, CombatEntity target)
        {
            poisonAmount = CalculatePoisonDamage(poisonAmount);
            target.Health -= poisonAmount;
            return poisonAmount;
        }

        //THIS CANNOT CHANGE THE TARGET'S HEALTH. ONLY CALCULATE HOW MUCH IT SHOULD DO IN THEORY
        public virtual double CalculateBurnDamage(double burnAmount, CombatEntity target)
        {
            return target.MaxHealth * (burnAmount / 1000.0);
        }   

        public virtual double TakeBurnDamage(double burnAmount, CombatEntity target)
        {
            burnAmount = CalculateBurnDamage(burnAmount, target);
            target.Health -= burnAmount;
            return burnAmount;
        }

        /**
         *The AttackDecorator passed in here is the most outer attack decorater
         */
        public static Attack CreateAttack(CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            //dec.Parent = parent;
            if (dec == null)
            {
                return new Attack(parent);
            }
            Attack baseAttack = new Attack();

            dec.SetParent(parent);
            AttackDecorator attackDecoratorIt = dec;
            while (true)
            {
                attackDecoratorIt.MainAttack = baseAttack;
                if(attackDecoratorIt.LayerUpAttack ==null)
                {
                    attackDecoratorIt.LayerUpAttack = baseAttack;
                    return dec;
                }
                attackDecoratorIt = (AttackDecorator)attackDecoratorIt.LayerUpAttack;
            }
        }


        /**
         *  Creates another attack based on the parent.
         *  ONLY  CALL IN EXECUTE ATTACK, RQEUIRES THE ATTACK TO BE INITIALIZED WITH TARGET AND CASTOR
         */
        public static Attack CreateFollowupAttack(Attack parent, CommandIterator it, AttackDecorator dec= null)
        {
            Attack followUp = Attack.CreateAttack(parent.Castor, parent, it, parent.Target, dec?.Clone());
            followUp.Target = parent.Target;
            followUp.Castor = parent.Castor;
            return followUp;
        }


        public static Attack CreateSelfTargetAttack(Attack parent, CommandIterator it, CombatEntity selfTarget)
        {
            Attack followup = Attack.CreateAttack(selfTarget, parent, it, selfTarget, selfTarget.CreateEntityDecorator(null, true));
            followup.Target = selfTarget;
            return followup;
        }
    }
}
