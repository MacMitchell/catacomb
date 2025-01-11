using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Entities;
using Catacomb.CombatStuff.AttackFactories;
namespace Catacomb.CombatStuff.Class
{
    public class CatClassFactory
    {


        public static CatClass Enchanter(CombatPlayer play)
        {
            CatClass enchanter = new CatClass("Enchanter", "Specializes in buffing their weapons in the heat of combat", 10, play);
            CatClassLevelUpHelper levelUp = new CatClassLevelUpHelper(play);
            levelUp.manaChange = 3;
            levelUp.attackChange = 2;
            levelUp.magicChange = 2;
            levelUp.defenseChange = 1;
            levelUp.magicResistChange = 1;
            levelUp.CreateLevelUpForAllLevels(enchanter);

            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.EnchantmentPoison,enchanter, 3);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.EnchantmentBurn, enchanter, 5);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.EnchantmentCorrosive, enchanter, 9);

            return enchanter;

        }
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

        public static CatClass InfernoMage(CombatPlayer play)
        {
            CatClass infernoMage = new CatClass("Inferno Mage", "Loves fire, loves lighting things on fire, and loves hugging fire.", 10, play);

            CatClassLevelUpHelper helper = new CatClassLevelUpHelper(play);
            helper.manaChange = 2;
            helper.magicChange = 6;
            helper.magicResistChange = 2;
            helper.CreateLevelUpForAllLevels(infernoMage);

            CatClassLevelUpHelper.AddGainAttackLevelUp(TurnBasedAttackFactory.FlamingOutrage,infernoMage,3);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.Heatwave,infernoMage,9);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.SelfIgnition, infernoMage, 4);
            
            return infernoMage;
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

            CatClass temp = CatClassFactory.FireMage(play);
            CatClassLevelUpHelper.AddGainCatClassLevelUp(mage, temp , 4);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.FireLance, mage, 4);

            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.FrostLance, mage, 7);
            CatClassLevelUpHelper.AddGainCatClassLevelUp(mage, CatClassFactory.FrostMage(play), 7);

            CatClassLevelUpHelper.AddGainAttackLevelUp(TurnBasedAttackFactory.LesserManaRegen, mage, 10);
            
            return mage;
        }

        public static CatClass FrostMage(CombatPlayer play)
        {
            CatClass frostMage = new CatClass("Frost Mage", "Likes ice and ice cream", 10, play);

            CatClassLevelUpHelper helper = new CatClassLevelUpHelper(play);
            helper.healthChange = 2;
            helper.manaChange = 5;
            helper.magicChange = 3;
            helper.defenseChange = 1;
            helper.magicResistChange = 1;
            helper.speedChange = 3;
            helper.CreateLevelUpForAllLevels(frostMage);

            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.FrostBlast, frostMage, 4);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.Blizzard, frostMage, 8);
            CatClassLevelUpHelper.AddGainAttackLevelUp(AttackFactory.IceBeam, frostMage, 10);

            
            return frostMage;
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
