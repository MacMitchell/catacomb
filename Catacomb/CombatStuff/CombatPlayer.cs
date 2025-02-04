﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.CombatStuff.Class;
namespace Catacomb.CombatStuff
{
    public class CombatPlayer:CombatEntity
    {
        private CatClass currentCatClass;
        private List<CatClass> allClasses;
        
        public override bool IsPlayer { get => true;  }
        public List<CatClass> AllClasses { get => allClasses; set => allClasses = value; }

        public CatClass CurrentCatClass { get { return currentCatClass; } set
            {
                if (!AllClasses.Contains(value))
                {
                    AllClasses.Add(value);
                }
             currentCatClass = value; } }

        public void AddClass(CatClass c)
        {
            AllClasses.Add(c);
        }
        public CombatPlayer(string nameIn, double defaultValue = 0):base(nameIn, defaultValue)
        {
            AllClasses = new List<CatClass>();
        }
        public override void InitilzeGenericValues()
        {
            base.EndOfCombatAttack.Add(UtilAttackFactory.DefaultPlayerEndOfCombat);
            base.StartOfCombatAttacks.Add(UtilAttackFactory.DefaultPlayerStartOfCombatAttack);
        }

        public override void PrepAttack()
        {
            Armor = 0;
            base.PrepAttack();
        }


        public Attack GetAttack(int index, Command parent, CommandIterator it, CombatEntity other)
        {
           if(index >= TempGenerateAttacks.Count)
             {
                return null;
            }
            return CreateAttack(TempGenerateAttacks[index], parent, it, other);
        }
        public override string GenerateStats()
        {
            return base.GenerateStats() +"\n" + currentCatClass.Name;
        }


        public void HealForNextFloor()
        {
            Health = Math.Min(Health + (MaxHealth / 2.0), MaxHealth);
            Mana = MaxMana;
        }
    }

    
}
