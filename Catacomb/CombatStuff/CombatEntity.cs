using Catacomb.CombatStuff.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{


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
        private double xp;
        private Boolean isPlayer;

        private CatClass currentClass;

        public delegate Attack AttackGenerator(CombatEntity castor, Command parent, CommandIterator it,CombatEntity other, AttackDecorator dec =null);
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

        public List<AttackGenerator> TempGenerateAttacks { get => tempGenerateAttacks; set => tempGenerateAttacks = value; }
        protected List<AttackGenerator> StartOfCombatAttacks { get => startOfCombatAttacks; set => startOfCombatAttacks = value; }
        public List<AttackGenerator> StartOfTurnAttacks { get => startOfTurnAttacks; set => startOfTurnAttacks = value; }
        public List<AttackGenerator> TempStartOfTurnAttacks { get => tempStartOfTurnAttacks; set => tempStartOfTurnAttacks = value; }
        public List<AttackGenerator> EndOfTurnAttacks { get => endOfTurnAttacks; set => endOfTurnAttacks = value; }
        public List<AttackGenerator> TempEndOfTurnAttacks { get => tempEndOfTurnAttacks; set => tempEndOfTurnAttacks = value; }
        public double Poison { get => poison; set => poison = value; }
        protected List<AttackGenerator> SymmetricAttacks { get => symmetricAttacks; set => symmetricAttacks = value; }

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
            generateAttacks.Add(newAttack);
        }

        public void AddTempAttack(AttackGenerator newTempAttack)
        {
            tempGenerateAttacks.Add(newTempAttack);
        }
        public Attack GetAttack(Command parentIn,CommandIterator it, CombatEntity other)
        {
            int index = rand.Next(0, TempGenerateAttacks.Count);
            return GetAttack(index, parentIn, it,other);
        }

        public Attack GetEndOfCombatAttack(CommandIterator it, Command parentIn,CombatEntity other)
        {
            return EndOfCombatAttack(this, parentIn,it,other);
        }

        public List<Attack> GetStartOfCombatAttack(CommandIterator it, Command parentIn, CombatEntity other)
        {
            List<Attack> attacks = new List<Attack>();
            foreach(AttackGenerator att in startOfCombatAttacks)
            {
                Attack temp = att(this, parentIn, it, other);
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
                Attack temp = att(this, parentIn, it, other);
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
                Attack temp = att(this, parentIn, it, other);
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
                Attack temp = att(this, parentIn, it, other);
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
            return TempGenerateAttacks[index](this, parent,it, other);
        }
        
        public void PrepAttack()
        {
            tempGenerateAttacks = new List<AttackGenerator>(generateAttacks);
            tempStartOfTurnAttacks = new List<AttackGenerator>(startOfTurnAttacks);
            tempEndOfTurnAttacks = new List<AttackGenerator>(endOfTurnAttacks);
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
                             "\nXP: " + XP;
            return output;
        }
    }
}
