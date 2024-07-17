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
        private double xp;
        private Boolean isPlayer;

        private CatClass currentClass;

        public delegate Attack AttackGenerator(CombatEntity castor, Command parent, CommandIterator it,CombatEntity other);
        protected List<AttackGenerator> generateAttacks;

        protected AttackGenerator endOfCombatAttack;
        public AttackGenerator EndOfCombatAttack
        {
            get { return endOfCombatAttack; }
            set { endOfCombatAttack = value; }
        }
        public CombatEntity(string nameIn, double defaultValue = 0,bool isPlayer =false)
        {
            this.isPlayer = isPlayer;
            Name = nameIn;
            generateAttacks = new List<AttackGenerator>();
            
            InializeValues(defaultValue);
            InitilzeGenericValues();
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
        }

        public virtual void InitilzeGenericValues()
        {
            if (!IsPlayer)
            {
                EndOfCombatAttack = UtilAttackFactory.DefaultEndOfCombatAttack;
            }
            //else
            //{
              //  EndOfCombatAttack = UtilAttackFactory.DefaultPlayerEndOfCombat;
            //}
        }

        public void AddAttack(AttackGenerator newAttack) 
        {
            generateAttacks.Add(newAttack);
        }
        public Attack GetAttack(Command parentIn,CommandIterator it, CombatEntity other)
        {
            int index = rand.Next(0, generateAttacks.Count);
            return GetAttack(index, parentIn, it,other);
        }

        public Attack GetEndOfCombatAttack(CommandIterator it, Command parentIn,CombatEntity other)
        {
            return EndOfCombatAttack(this, parentIn,it,other);
        }

        public List<Attack> GetListOfAttacks()
        {
            List<Attack> attacks = new List<Attack>();
            for(int i =0; i < generateAttacks.Count; i++)
            {
                attacks.Add(generateAttacks[i](this, null,null,null));
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
            if(index >= generateAttacks.Count)
            {
                return null;
            }
            return generateAttacks[index](this, parent,it, other);
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
