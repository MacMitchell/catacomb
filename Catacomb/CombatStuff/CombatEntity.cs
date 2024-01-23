﻿using System;
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
        public double MaxArmor { get => maxArmor; set => maxArmor = value; }
        public bool IsPlayer { get => isPlayer; set => isPlayer = value; }

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
        private double maxArmor;

        private Boolean isPlayer;

        public delegate Attack AttackGenerator(CombatEntity castor, Command parent);
        protected List<AttackGenerator> generateAttacks;
        public CombatEntity(string nameIn, double defaultValue = 0)
        {
            isPlayer = false;
            Name = nameIn;
            generateAttacks = new List<AttackGenerator>();
            InializeValues(defaultValue);
        }
        public virtual void InializeValues(double defaultValue = 0)
        {
            MaxArmor = defaultValue;
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
        }

        public void AddAttack(AttackGenerator newAttack) 
        {
            generateAttacks.Add(newAttack);
        }
        public Attack GetAttack(Command parentIn)
        {
            return generateAttacks[0](this,parentIn);
        }

        public virtual string GenerateStats()
        {
            string output = Name + "\nHealth: " +
                            Health + "/" + MaxHealth +
                            "\nArmor: " + Armor + "/" + MaxArmor +
                            "\nAttack: " + AttackStat + "/" + maxAttackStat +
                            "\nMagic: " + MagicStat + "/" + MaxMagicResist +
                            "\nDefense: " + Defense + "/" + MaxDefense +
                            "\nMagic Resist: " + MagicResist + "/" + MaxMagicResist +
                            "\nSpeed: " + Speed + "/" + MaxSpeed;
            return output;
        }
    }
}
