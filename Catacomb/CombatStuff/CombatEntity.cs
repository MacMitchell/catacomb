using Catacomb.CombatStuff.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{

    public enum AttackType
    {
        SOC,
        SOT,
        EOT,
        EOC,
        A,
        DD, //attack decorator defense
        DA, //attack decorator attack
    }

    public class CombatEntity {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public static Random rand = new Random();

        public double MaxHealth { get => maxHealth; set => maxHealth = value; }
        public double Health { get => health; set => health = value; }
        public double AttackStat { get => attackStat; set => attackStat = value; }
        public double MaxAttackStat { get => maxAttackStat; set => maxAttackStat = value; }
        public double MagicStat { get => magicStat; set => magicStat = value; }
        public double MaxMagicStat { get => maxMagicStat; set => maxMagicStat = value; }
        public double Defense { get => defense; set => defense = value; }
        public double MaxDefense { get => maxDefense; set => maxDefense = value; }
        public double MagicResist { get => magicResist; set => magicResist = value; }
        public double MaxMagicResist { get => maxMagicResist; set => maxMagicResist = value; }
        public double Speed { get => speed; set => speed = value; }
        public double MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public double Armor { get => armor; set => armor = value; }

        public double XP { get => xp; set => xp = value; }
        public virtual bool IsPlayer { get => false; set => isPlayer = value; }

        public CatClass GetClass { get => currentClass; }

        private double maxHealth;
        private double health;
        private double mana;
        private double attackStat;
        private double maxAttackStat;
        private double magicStat;
        private double maxMagicStat;
        private double defense;
        private double maxDefense;
        private double magicResist;
        private double maxMagicResist;
        private double speed;
        private double maxSpeed;
        private double armor;
        private double poison;
        private double burn;
        private double xp;
        private Boolean isPlayer;

        private CatClass currentClass;

        public delegate Attack AttackGenerator(CombatEntity castor, Command parent, CommandIterator it,CombatEntity other, AttackDecorator dec =null);
        public delegate AttackDecorator DecoratorGenerator(Attack parentAttack, Command parent = null);
        protected List<AttackGenerator> generateAttacks;
        private List<AttackGenerator> tempGenerateAttacks; //this is used to keep track of attacks that can be used in one combat. It allows for attack to be in one combat and removed for the next

        private List<AttackGenerator> startOfCombatAttacks;

        private List<AttackGenerator> startOfTurnAttacks;
        private List<AttackGenerator> tempStartOfTurnAttacks;

        private List<AttackGenerator> endOfTurnAttacks;
        private List<AttackGenerator> tempEndOfTurnAttacks;

        private List<AttackGenerator> symmetricAttacks;

        protected AttackGenerator endOfCombatAttack;
        public AttackGenerator EndOfCombatAttack
        {
            get { return endOfCombatAttack; }
            set { endOfCombatAttack = value; }
        }

        private List<DecoratorGenerator> attackingAttackDecs;
        private List<DecoratorGenerator> tempAttackingAttackDecs;

        private List<DecoratorGenerator> defenseAttackDecs;
        private List<DecoratorGenerator> tempDefenseAttackDecs;

        public List<AttackGenerator> TempGenerateAttacks { get => tempGenerateAttacks; set => tempGenerateAttacks = value; }
        protected List<AttackGenerator> StartOfCombatAttacks { get => startOfCombatAttacks; set => startOfCombatAttacks = value; }
        public List<AttackGenerator> StartOfTurnAttacks { get => startOfTurnAttacks; set => startOfTurnAttacks = value; }
        public List<AttackGenerator> TempStartOfTurnAttacks { get => tempStartOfTurnAttacks; set => tempStartOfTurnAttacks = value; }
        public List<AttackGenerator> EndOfTurnAttacks { get => endOfTurnAttacks; set => endOfTurnAttacks = value; }
        public List<AttackGenerator> TempEndOfTurnAttacks { get => tempEndOfTurnAttacks; set => tempEndOfTurnAttacks = value; }
        public double Poison { get => poison; set => poison = value; }
        protected List<AttackGenerator> SymmetricAttacks { get => symmetricAttacks; set => symmetricAttacks = value; }
        public double Burn { get => burn; set => burn = value; }
        public double Mana { get => mana; set => mana = value; }
        public List<DecoratorGenerator> AttackingAttackDecs { get => attackingAttackDecs; set => attackingAttackDecs = value; }
        public List<DecoratorGenerator> TempAttackingAttackDecs { get => tempAttackingAttackDecs; set => tempAttackingAttackDecs = value; }
        public List<DecoratorGenerator> DefenseAttackDecs { get => defenseAttackDecs; set => defenseAttackDecs = value; }
        public List<DecoratorGenerator> TempDefenseAttackDecs { get => tempDefenseAttackDecs; set => tempDefenseAttackDecs = value; }

        public CombatEntity(string nameIn, double defaultValue = 0,bool isPlayer =false)
        {
            this.isPlayer = isPlayer;
            Name = nameIn;
            generateAttacks = new List<AttackGenerator>();
            tempGenerateAttacks = new List<AttackGenerator>();

            startOfCombatAttacks = new List<AttackGenerator>();

            startOfTurnAttacks = new List<AttackGenerator>();
            tempStartOfTurnAttacks = new List<AttackGenerator>();

            endOfTurnAttacks = new List<AttackGenerator>();
            tempEndOfTurnAttacks = new List<AttackGenerator>();

            SymmetricAttacks = new List<AttackGenerator>();

            attackingAttackDecs = new List<DecoratorGenerator>();
            tempAttackingAttackDecs = new List<DecoratorGenerator>();

            defenseAttackDecs = new List<DecoratorGenerator>();
            tempDefenseAttackDecs = new List<DecoratorGenerator>();

            InializeValues(defaultValue);
            InitilzeGenericValues();
            InitializeSymmetric();
        }
        public virtual void InializeValues(double defaultValue = 0)
        {
            MaxAttackStat = defaultValue;
            MaxMagicStat = defaultValue;
            MaxHealth = defaultValue;
            MaxMagicResist = defaultValue;
            mana = defaultValue;
            MaxDefense = defaultValue;
            MaxSpeed = defaultValue;

            armor = defaultValue;
            attackStat = defaultValue;
            magicStat = defaultValue;
            health = defaultValue;
            magicResist = defaultValue;
            defense = defaultValue;
            speed = defaultValue;
            xp = 0;
            Poison = 0.0;
            Burn = 0.0;
        }

        public virtual void InitilzeGenericValues()
        {
            if (!IsPlayer)
            {
                EndOfCombatAttack = UtilAttackFactory.DefaultEndOfCombatAttack;                
            }
            startOfCombatAttacks.Add(UtilAttackFactory.DefaultStartOfCombatAttack);
        }

        protected virtual void InitializeSymmetric()
        {
            SymmetricAttacks.Add(UtilAttackFactory.SymmetricEndOfTurn);
        }


        public void AddAttack(AttackGenerator newAttack)
        {
            AttackType type = newAttack(this,null,null,null,null).Type;
            switch (type)
            {
                case AttackType.A: generateAttacks.Add(newAttack); break;
                case AttackType.SOC: StartOfCombatAttacks.Add(newAttack); break;
                case AttackType.SOT: StartOfTurnAttacks.Add(newAttack); break;
                case AttackType.EOT: EndOfTurnAttacks.Add(newAttack); break;
            }
        }
        public void AttackTempAttack(AttackGenerator newTempAttack)
        {
            AttackType type = newTempAttack(this, null, null, null, null).Type;
            switch (type)
            {
                case AttackType.A: tempGenerateAttacks.Add(newTempAttack); break;
                case AttackType.SOT: TempStartOfTurnAttacks.Add(newTempAttack); break;
                case AttackType.EOT: TempEndOfTurnAttacks.Add(newTempAttack); break;
            }
        }
        public void AddTempAttack(AttackGenerator newTempAttack)
        {
            tempGenerateAttacks.Add(newTempAttack);
        }
        public void AddTempAttackDecorator(DecoratorGenerator gen)
        {
            if (gen(null, null).Type == AttackType.DA)
            {
                tempAttackingAttackDecs.Add(gen);
            }
            else
            {
                tempDefenseAttackDecs.Add(gen);
            }
        }
        public void AddAttackDecorator(DecoratorGenerator gen)
        {
            if (gen(null,null).Type == AttackType.DA)
            {
                attackingAttackDecs.Add(gen);
            }
            else
            {
                DefenseAttackDecs.Add(gen);
            }
        }
        
        public AttackDecorator CreateEntityDecorator(AttackDecorator prev = null, bool defending = false)
        {
            if (tempAttackingAttackDecs == null)
            {
                return prev;
            }
            AttackDecorator toReturn = prev;
            if (defending)
            {
                foreach (DecoratorGenerator dec in TempDefenseAttackDecs)
                {
                    toReturn = dec(prev);
                    prev = toReturn;
                }
            }
            else
            {
                foreach (DecoratorGenerator dec in TempAttackingAttackDecs)
                {
                    toReturn = dec(prev);
                    prev = toReturn;
                }
            }
            return toReturn;
        }

        public AttackDecorator SetUpDecs(CombatEntity other)
        {
            //attack than reciever
            if(other == null)
            {
                return null;
            }
            AttackDecorator dec = CreateEntityDecorator(null,false);
            dec = other.CreateEntityDecorator(dec,true);
            return dec;
        }
        public Attack GetAttack(Command parentIn,CommandIterator it, CombatEntity other)
        {
            int index = rand.Next(0, TempGenerateAttacks.Count);
            return GetAttack(index, parentIn, it,other);
        }

        public Attack GetEndOfCombatAttack(CommandIterator it, Command parentIn,CombatEntity other)
        {
            return EndOfCombatAttack(this, parentIn,it,other,SetUpDecs(other));
        }

        public List<Attack> GetStartOfCombatAttack(CommandIterator it, Command parentIn, CombatEntity other)
        {
            List<Attack> attacks = new List<Attack>();
            foreach(AttackGenerator att in startOfCombatAttacks)
            {
                Attack temp = att(this, parentIn, it, other,SetUpDecs(other));
                temp.Castor = this;
                temp.Target = other;
                attacks.Add(temp);
            }
            return attacks;
        }

        public List<Attack> GetStartOfTurnAttack(CommandIterator it, Command parentIn, CombatEntity other)
        {
            List<Attack> attacks = new List<Attack>();
            foreach (AttackGenerator att in TempStartOfTurnAttacks)
            {
                Attack temp = att(this, parentIn, it, other, SetUpDecs(other));
                temp.Castor = this;
                temp.Target = other;
                attacks.Add(temp);
            }
            return attacks;
        }

        public List<Attack> GetEndOfTurnAttack(CommandIterator it, Command parentIn, CombatEntity other)
        {
            List<Attack> attacks = new List<Attack>();
            foreach (AttackGenerator att in TempEndOfTurnAttacks)
            {
                Attack temp = att(this, parentIn, it, other, SetUpDecs(other));
                if(temp == null)
                {
                    continue;
                }
                temp.Castor = this;
                temp.Target = other;
                attacks.Add(temp);
            }
            return attacks;
        }

        public List<Attack> GetSymmetricEndOfTurnAttack(CommandIterator it, Command parentIn, CombatEntity other)
        {
            List<Attack> attacks = new List<Attack>();
            foreach (AttackGenerator att in SymmetricAttacks)
            {
                Attack temp = att(this, parentIn, it, other, SetUpDecs(other));
                if (temp == null)
                {
                    continue;
                }
                temp.Castor = this;
                temp.Target = other;
                attacks.Add(temp);
            }
            return attacks;
        }

        public List<Attack> GetListOfAttacks()
        {
            List<Attack> attacks = new List<Attack>();
            for(int i =0; i < TempGenerateAttacks.Count; i++)
            {
                attacks.Add(TempGenerateAttacks[i](this, null,null,null));
            }
            return attacks;
        }

        public void Reset()
        {
            AttackStat = MaxAttackStat;
            MagicStat = MaxMagicStat;
            Defense = MaxDefense;
            MagicResist = MaxMagicResist;
            Speed = MaxSpeed;
        }

        public Attack GetAttack(int index, Command parent,CommandIterator it,CombatEntity other)
        {
            if(index >= TempGenerateAttacks.Count)
            {
                return null;
            }
             return TempGenerateAttacks[index](this, parent,it, other,SetUpDecs(other));
        }


        public void PrepAttack()
        {
            tempGenerateAttacks = new List<AttackGenerator>(generateAttacks);
            tempStartOfTurnAttacks = new List<AttackGenerator>(startOfTurnAttacks);
            tempEndOfTurnAttacks = new List<AttackGenerator>(endOfTurnAttacks);
            tempAttackingAttackDecs = new List<DecoratorGenerator>(attackingAttackDecs);
            tempDefenseAttackDecs = new List<DecoratorGenerator>(defenseAttackDecs);
        }
       

        public virtual string GenerateStats()
        {
            string output = Name + "\nHealth: " +
                            Health + "/" + MaxHealth +
                            "\nArmor: " + Armor  +
                            "\nAttack: " + AttackStat + "/" + MaxAttackStat +
                            "\nMagic: " + MagicStat + "/" + MaxMagicStat + 
                            "\nDefense: " + Defense + "/" + MaxDefense +
                            "\nMagic Resist: " + MagicResist + "/" + MaxMagicResist +
                            "\nSpeed: " + Speed + "/" + MaxSpeed +
                            "\nPoison: " + Poison +
                            "\nBurn: " + Burn +
                             "\nXP: " + XP;
            return output;
        }
    }
}
