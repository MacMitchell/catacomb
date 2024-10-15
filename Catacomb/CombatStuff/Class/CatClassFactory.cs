﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Entities;
namespace Catacomb.CombatStuff.Class
{
    public class CatClassFactory
    {
        public static CatClass FireMage(CombatPlayer play)
        {
            CatClass fireMage = new CatClass("Fire Mage", "Knows about fire. Hopefully it will help keep you warm and your enemies burning", 10, play);


            CatClassLevelUpHelper mageLevelUp = new CatClassLevelUpHelper(play);
            mageLevelUp.manaChange = 7;
            mageLevelUp.magicChange = 5;
            mageLevelUp.magicResistChange = 2;
            mageLevelUp.CreateLevelUpForAllLevels(fireMage);
            return fireMage;
        }
        public static CatClass Mage(CombatPlayer play)
        {
            CatClass mage = new CatClass("Mage", "A basic mage user. Knows a little, not much", 10, play);
            mage.LevelBase = 100;

            CatClassLevelUpHelper mageLevelUp = new CatClassLevelUpHelper(play);
            mageLevelUp.healthChange = 3;
            mageLevelUp.manaChange = 5;
            mageLevelUp.magicChange = 3;
            mageLevelUp.magicResistChange = 1;
            mageLevelUp.CreateLevelUpForAllLevels(mage);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.FrostLance, mage, 5);

            CatClass temp = CatClassFactory.FireMage(play);
            CatClassLevelUpHelper.AddGainCatClassLevelUp(mage, temp , 3);
            return mage;
        }

        public static CatClass Squire(CombatPlayer play)
        {
            CatClass squire = new CatClass("Squire", "Learning to use the sword", 10 ,play);

            CatClassLevelUpHelper squireHelper = new CatClassLevelUpHelper(play);
            squireHelper.attackChange = 1;
            squireHelper.healthChange = 2;
            squireHelper.defenseChange = 1;
            squireHelper.magicResistChange = 1;
            squireHelper.speedChange = 1;
            squireHelper.CreateLevelUpForAllLevels(squire);

            return squire;
        }
        
    }
}
