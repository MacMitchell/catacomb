using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Entities;

namespace Catacomb.CombatStuff.Class
{
    public class CatClass
    {
        private int min = 1;
        private int max = 10;


        private double[] xpToLevelUp; //the xp at [0] is the xp needed to get to level 2. I.e. xp to get out of level 1

        private int currentLevel;
        private double currentXp; //xp is at the current level, it does not carry between levls

        private double basicLevelMulitplier = 1.10;
        private double levelBase = 10;

        private string name;
        private string description;


        
        public delegate Command LevelUp(CommandIterator it, Command parent, CombatPlayer player);
        public List<LevelUp>[] levelUps;

        private CombatPlayer player;
        public CatClass(string name, string description, int maxLevel, CombatPlayer player)
        {
            Name = name;
            Description = description;

            Max = maxLevel;
            CurrentLevel = 1;
            currentXp = 0;
            this.player = player;

            initalizeLevelUps();
            GenerateDefaultLevelUp();
        }
        private double getXpForLevel(int level)
        {
            return XpToLevelUp[level - 1];
        }
        private bool CheckForLevelUp()
        {
            if(currentLevel >= Max)
            {
                return false;
            }
            return currentXp >= getXpForLevel(currentLevel);
        }

        public void GainXp(CommandIterator it, Command parent, double xpAmount)
        {
            
            currentXp += xpAmount;
            while (CheckForLevelUp()) {
                currentXp -=getXpForLevel(currentLevel);
                currentLevel++;
                ExecuteLevelUps(it, parent, currentLevel);
            }
        }

        private void ExecuteLevelUps(CommandIterator it, Command parent, int levelUp)
        {
            foreach (LevelUp lvl in levelUps[levelUp-1])
            {
                lvl(it, parent, player);
            }
        }
        public void AddLevelUpEffect(int level, LevelUp effect)
        {
            levelUps[level-1].Add(effect);
        }

        private void initalizeLevelUps()
        {
            levelUps = new List<LevelUp>[max];
            for (int i = 0; i < levelUps.Length; i++)
            {
                levelUps[i] = new List<LevelUp>();
            }
        }
        private void GenerateDefaultLevelUp()
        {
            double currentXp = LevelBase;
            XpToLevelUp = new double[max - 1];
            for (int i = 0; i < max - 1; i++)
            {
                XpToLevelUp[i] = currentXp;
                currentXp *= BasicLevelMulitplier;
            }
        }


        public int Min { get => min; set => min = value; }
        public int Max { get => max; set => max = value; }
        public double[] XpToLevelUp { get => xpToLevelUp; set => xpToLevelUp = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public double BasicLevelMulitplier { get => basicLevelMulitplier; set => basicLevelMulitplier = value; }
        public double LevelBase { get => levelBase; set => levelBase = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
    }
    
    public class CatClassLevelUpHelper
    {
        public int healthChange;
        public int manaChange;
        public int attackChange;
        public int magicChange;
        public int defenseChange;
        public int magicResistChange;
        public int speedChange;

        private CombatPlayer player;
        public CatClassLevelUpHelper(CombatPlayer player)
        {
            healthChange = 0;
            manaChange = 0;
            attackChange = 0;
            magicChange = 0;
            defenseChange = 0;
            magicResistChange = 0;
            speedChange = 0;
            this.player = player;
        }

        public void CreateLevelUp(CatClass classIn, int level)
        {
            classIn.AddLevelUpEffect(level, (CommandIterator it, Command parent, CombatPlayer player) => this.CreateBasicCommand(it, parent,player,level));
        }

        public void CreateLevelUpForAllLevels(CatClass classIn)
        {
            int min = classIn.Min;
            int max = classIn.Max;
            for(; min <= max; min++)
            {
                CreateLevelUp(classIn, min);
            }

        }
        public Command CreateBasicCommand(CommandIterator it, Command parent, CombatPlayer play, int lvl)
        {
            var levelUpString = "LEVEL UP !!\n"
                            + (lvl-1) +"->"+lvl +"\n"
                            + "HP:\t+ " + healthChange + "\n"
                            + "MP:\t+ " + manaChange + "\n"
                            + "ATK:\t+ " + attackChange + "\n"
                            + "MAG:\t+ " + magicChange + "\n"
                            + "DEF:\t+ " + defenseChange + "\n"
                            + "MR:\t+ " + magicResistChange + "\n"
                            + "SPD:\t+ " + speedChange + "\n";
            
             Attack temp =  UtilAttackFactory.GenerateTextAttack(parent, levelUpString);
            temp.ExecuteAttack += (CombatEntity no, CombatEntity nope) =>
            {
                play.MaxHealth += healthChange;
                play.MaxAttackStat += attackChange;
                play.MaxMagicStat += magicChange;
                play.MaxDefense += defenseChange;
                play.MaxMagicResist += magicResistChange;
                play.MaxSpeed += speedChange;
            };
            return temp;

        }

        public static void AddGainAttackLevelUp(CombatEntity.AttackGenerator attack, CatClass catclass, int lvl)
        {
            catclass.AddLevelUpEffect(lvl, (CommandIterator it, Command parent, CombatPlayer player) =>
            {
                Attack gainAttack = new Attack(parent);
                gainAttack.ExecuteAttack = (CombatEntity no, CombatEntity nope) =>
                 {
                     Attack basicInfo = attack(player, null, null, null);
                     gainAttack.Description = "You learned the skill " + basicInfo.Name + "!";
                     player.AddAttack(attack);
                 };
                return gainAttack;
            });

        }

        public static void AddGainCatClassLevelUp(CatClass currentCatClass, CatClass gainClass, int lvl)
        {
            currentCatClass.AddLevelUpEffect(lvl, (CommandIterator it, Command parent, CombatPlayer player) =>
            {
                Attack gainAttack = new Attack(parent);
                gainAttack.ExecuteAttack = (CombatEntity no, CombatEntity nope) =>
                {
                    gainAttack.Description = "You gained the class: " + gainClass.Name + "!";
                    player.AllClasses.Add(gainClass);
                };
                return gainAttack;
            });
        }

    }


    public class CheckForLevelUpCommand: AdminCommands
    {
        private CombatEntity monster;
        private CombatPlayer player;
        public CheckForLevelUpCommand(CommandIterator iteratorIn, Command parent,CombatPlayer player, CombatEntity monster) : base(iteratorIn, parent){
            this.player = player;
            this.monster = monster;
        }

        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            if (player.CurrentCatClass != null)
            {
                player.CurrentCatClass.GainXp(it, this, monster.XP);
            }
            return base.Execute(castor, target);
        }
    }
}
