using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.CombatStuff.Class;
using Catacomb.CombatStuff.AttackFactories;
using Catacomb.Entities;
using Catacomb.CombatStuff.MonsterUtils;

namespace Catacomb.CombatStuff
{
    public enum MonsterType
    {
        Slime,
        All,
        Brute,
        Imp,
        Demon,
        Dragon,
        Fire,
        Goblin,
        Ethereal,
        Elemental,
        Orc,

        Wolf,
    }

    class MonsterFactory
    {
        private static int level1MonsterXp = 25;
        private static int level2MonsterXp = 50;
        private static int level3MonsterXp = 100;

        private static int level2BossXp = 200;

        public static CombatEntity GeneratePlayer(string name = "Player")
        {
            CombatPlayer player = new CombatPlayer(name,100);
            player.MaxHealth = 50;
            player.Health = 50;
            player.MaxAttackStat = 5;
            player.MaxMagicStat = 5;
            player.Armor = 0;
            player.MaxDefense = 1;
            player.MaxMagicResist = 1;

            player.IsPlayer = true;

            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.FireBall);

            player.CurrentCatClass = CatClassFactory.Mage(player);
            player.CurrentCatClass = CatClassFactory.Squire(player);


            return player;
        }
        public static Monster GreenSlime(Player player)
        {
            Monster greenSlime = new Monster(35, 35,35 );
            //greenSlime.MovementAI = new BasicMovement(greenSlime, player);
            greenSlime.MovementAI = new SmartMovement(greenSlime, player);
            greenSlime.Fighter = MonsterFactory.GreenSlimeCombat();
            greenSlime.Type = MonsterType.Slime;
            greenSlime.Clone = GreenSlime;
            return greenSlime;
        }
        public static CombatEntity GreenSlimeCombat()
        {
            CombatEntity slime = new CombatEntity("Slime",40);
            slime.MaxSpeed = 50;
            slime.MaxAttackStat = 3;
            slime.MaxMagicStat = 3;
            slime.Armor = 0;
            slime.MaxDefense = 0;
            slime.MaxMagicResist = 0;
            slime.XP = level1MonsterXp;
            slime.AddAttack(AttackFactory.Tackle);
            slime.AddAttack(AttackFactory.Leech);

            slime.AddAttackDecorator(AttackDecFactory.PoisonTouch);
            slime.AddAttackDecorator(AttackDecFactory.ShoulderGaurd);
            
            return slime;
        }


        public static Monster DireWolf(Player player)
        {
            Monster direWolf = new Monster(35, 35, 400);
            direWolf.MovementAI = new SmartMovement(direWolf, player,senseRange:200);
            direWolf.Type = MonsterType.Wolf;
            direWolf.Clone = DireWolf;

            CombatEntity fighter = new CombatEntity("Dire Wolf", 70);
            fighter.MaxSpeed = 200;
            fighter.MaxAttackStat = 8;
            fighter.MaxMagicStat = 0;
            fighter.Armor = 0;
            fighter.MaxDefense = 3;
            fighter.MaxMagicResist = 0;
            fighter.XP = level2MonsterXp;


            fighter.AddAttack(AttackFactory.Bite);
            fighter.AddAttack(AttackFactory.Growl);
            fighter.AddAttack(TurnBasedAttackFactory.SenseBlood);
            direWolf.Fighter = fighter;
            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] { "The direwolf snarls at you!" }, fighter);

            return direWolf;
        }

        public static Monster FireElemental(Player play)
        {
            Monster fireElemental = new Monster(35, 35, 50);
            fireElemental.MovementAI = new SmartMovement(fireElemental, play);
            fireElemental.Type = MonsterType.Fire;
            fireElemental.Type = MonsterType.Elemental;
            fireElemental.Clone = FireElemental;


            CombatEntity fighter = new CombatEntity("Fire Elemental", 145);
            fighter.MaxMana = 150;
            fighter.Mana = 150;
            fighter.Armor = 0;
            fighter.MaxAttackStat = 20;
            fighter.MaxMagicStat = 20;
            fighter.MaxDefense = 5;
            fighter.MaxMagicResist = 7;
            fighter.MaxSpeed = 40;
            fighter.XP = level2BossXp;
            //add attacks
            fighter.AddAttack(AttackFactory.FireBall);
            fighter.AddAttack(AttackFactory.Ignite);
            fighter.AddAttack(AttackFactory.Heatwave);

            fighter.AddAttack(TurnBasedAttackFactory.FlamingOutrage);

            ConditionalAi ai = new ConditionalAi(fighter);
            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity player) { return player.Burn < 50; }, AttackFactory.Ignite);
            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity player) { return player.Burn >= 50; }, AttackFactory.Heatwave);

            fighter.MyAttackAi = ai;

            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] { "A tornado of flames whirls around you both.", "The ancient fire being looms over you.", "The elemental body of flames flares up." }, fighter);
            fireElemental.Fighter = fighter;
            return fireElemental;

        }

        public static Monster FireImp(Player player)
        {
            Monster fireImp = new Monster(35, 35, 250);
            fireImp.MovementAI = new SmartMovement(fireImp, player);
            fireImp.Type = MonsterType.Imp;
            fireImp.Type = MonsterType.Fire;
            fireImp.Clone = FireImp;


            fireImp.Fighter = FireImpCombat();
            return fireImp;
        }

        public static CombatEntity FireImpCombat()
        {
            CombatEntity fireImp = new CombatEntity("FireImp", 100);
            fireImp.Health = 90;
            fireImp.MaxMana = 50;
            fireImp.Mana = 50;
            fireImp.MaxHealth = 90;
            fireImp.MaxMagicStat = 59;
            fireImp.MaxMagicResist = 3;
            fireImp.MaxDefense = 0;
            fireImp.Armor = 0;
            fireImp.XP = level2MonsterXp;
           // fireImp.AddAttack(AttackFactory.Tackle);
            fireImp.AddAttack(AttackFactory.FireBall);
            fireImp.AddAttack(AttackFactory.FireBlast);
            return fireImp;
        }

        public static Monster FireWisp(Player player)
        {
            Monster fireWisp = new Monster(35, 35, 25);
            fireWisp.MovementAI = new SmartMovement(fireWisp, player,senseRange:1);
            fireWisp.Type = MonsterType.Fire;
            fireWisp.Type = MonsterType.Ethereal;
            fireWisp.Clone = FireWisp;

            CombatEntity firewispCombat = new CombatEntity("Fire Wisp", 30);
            firewispCombat.MaxMana = 100;
            firewispCombat.Mana = 100;
            firewispCombat.MaxAttackStat = 0;
            firewispCombat.MaxMagicStat = 30;
            firewispCombat.MaxDefense = -2;
            firewispCombat.MaxMagicResist = 4;
            firewispCombat.MaxSpeed = 20;

            firewispCombat.AddAttack(AttackFactory.FireBall);
            firewispCombat.AddAttack(AttackFactory.Combustion);
            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] { "It's just floating there.. menacingly", "It's hovering...", "It threatens you...?", "It looks like a floating explosion." }, firewispCombat);
            firewispCombat.XP = level2MonsterXp;
            fireWisp.Fighter = firewispCombat;
            return fireWisp;
            
        }

        public static Monster GoblinKing(Player player)
        {
            Monster goblinKing = new Monster(35, 35, 150);
            goblinKing.MovementAI = new SmartMovement(goblinKing, player);
            goblinKing.Clone = GoblinKing;
            goblinKing.Type = MonsterType.Goblin;

            CombatEntity fighter = new CombatEntity("Goblin King", 120);
            fighter.Armor = 0;
            fighter.MaxAttackStat = 15;
            fighter.MaxMagicStat = 6;
            fighter.MaxMana = 60;
            fighter.Mana = 60;
            fighter.MaxDefense = 8;
            fighter.MaxMagicResist = 8;
            fighter.MaxSpeed = 60;
            fighter.XP = level2BossXp;

            fighter.AddAttack(TurnBasedAttackFactory.GoblinBackupArmy);
            fighter.AddAttack(AttackFactory.DefensiveStance);
            fighter.AddAttack(AttackFactory.ArmorBreak);
            fighter.AddAttack(AttackFactory.RaiseGoblinArmylDamage);
            fighter.AddAttack(AttackFactory.RaiseGoblinArmylPoison);
            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] { "The king's army readys their weapons", "The king army surrounds you" }, fighter);


            ConditionalAi ai = new ConditionalAi(fighter);
            //even turns 
            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity p) { 
                return (!castor.metadata.ContainsKey("goblin_army_attack_count") || ((int)castor.metadata["goblin_army_attack_count"]) %2 ==0 ) && Global.Globals.Rand.NextDouble() >0.5; 
            }, AttackFactory.RaiseGoblinArmylDamage);
            
            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity p) {
                return (!castor.metadata.ContainsKey("goblin_army_attack_count") || ((int)castor.metadata["goblin_army_attack_count"]) % 2 == 0);
            }, AttackFactory.RaiseGoblinArmylPoison);


            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity p) {
                return (!castor.metadata.ContainsKey("goblin_army_attack_count") || ((int)castor.metadata["goblin_army_attack_count"]) % 2== 1) && Global.Globals.Rand.NextDouble() > 0.5;
            }, AttackFactory.ArmorBreak);

            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity p) {
                return (!castor.metadata.ContainsKey("goblin_army_attack_count") || ((int)castor.metadata["goblin_army_attack_count"]) % 2 == 1);
            }, AttackFactory.DefensiveStance);

            fighter.MyAttackAi = ai;
            goblinKing.Fighter = fighter;
            return goblinKing;
        }

        public static Monster GoblinMage(Player player)
        {
            Monster goblinMage = new Monster(35, 35, 150);
            goblinMage.MovementAI = new SmartMovement(goblinMage, player);
            goblinMage.Type = MonsterType.Goblin;
            goblinMage.Clone = GoblinMage;

            CombatEntity fighter = new CombatEntity("Goblin Mage", 40);
            fighter.Armor = 0;
            fighter.MaxAttackStat = 4;
            fighter.MaxDefense = 4;
            fighter.MaxMagicStat = 10;
            fighter.MaxMagicResist = 4;
            fighter.MaxSpeed = 50;
            fighter.XP = level2MonsterXp;


            fighter.AddAttack(AttackFactory.SummonMeteor);
            fighter.AddAttack(AttackFactory.MakeshiftMagic);

            goblinMage.Fighter = fighter;
            return goblinMage;
        }


        public static Monster GoblinScout(Player player)
        {
            Monster goblinScout = new Monster(35, 35, 200);
            //fireImp.MovementAI = new BasicWonderWithHuntingMovement(fireImp, player,150,Math.PI/4,500,Math.PI/2);
            goblinScout.MovementAI = new SmartMovement(goblinScout, player);
            goblinScout.Type = MonsterType.Goblin;
            goblinScout.Clone = GoblinScout;

            goblinScout.Fighter = GoblinScoutCombat();
            return goblinScout;
        }


        public static CombatEntity GoblinScoutCombat()
        {
            CombatEntity goblinScout = new CombatEntity("Goblin Scout", 100);
            goblinScout.Health = 40;
            goblinScout.MaxHealth = 40;
            goblinScout.MaxAttackStat = 4;
           
            goblinScout.MaxMagicStat = 0;
            goblinScout.MaxMagicResist = 0;
            goblinScout.MaxDefense = 4;
            goblinScout.Armor = 0;
            goblinScout.MaxSpeed = 50;
            goblinScout.XP = level1MonsterXp;
            goblinScout.AddAttack(AttackFactory.PoisonSlash);
            goblinScout.AddAttack(AttackFactory.Slash);
            goblinScout.AddAttack(TurnBasedAttackFactory.Rage);
            return goblinScout;
        }

        public static Monster GoblinWarrior(Player player)
        {
            Monster goblinWarrior = new Monster(35, 35, 120);
            goblinWarrior.MovementAI = new SmartMovement(goblinWarrior, player);
            goblinWarrior.Type = MonsterType.Goblin;
            goblinWarrior.Clone = GoblinWarrior;

            CombatEntity fighter = new CombatEntity("Goblin Warrior", 80);
            fighter.Armor = 0;
            fighter.MaxAttackStat = 15;
            fighter.MaxMagicStat = 0;
            fighter.MaxMana = 15;
            fighter.MaxDefense = 8;
            fighter.MaxMagicStat = 6;
            fighter.MaxSpeed = 50;
            fighter.XP = level2MonsterXp;
            fighter.AddAttack(AttackFactory.PoisonSlash);
            fighter.AddAttack(AttackFactory.Slash);
            fighter.AddAttack(AttackFactory.PoisonSlash);

            goblinWarrior.Fighter = fighter;

            return goblinWarrior;
        }


        public static Monster HellHound(Player player)
        {
            Monster hellHound = new Monster(35, 35, 350);
            hellHound.Type = MonsterType.Wolf;
            hellHound.Type = MonsterType.Elemental;
            hellHound.Clone = HellHound;
            hellHound.MovementAI = new SmartMovement(hellHound, player);

            CombatEntity fighter = new CombatEntity("Hell Hound", 100);
            fighter.MaxMana = 50;
            fighter.Mana = 50;

            fighter.MaxAttackStat = 20;
            fighter.MaxMagicStat = 15;
            fighter.MaxDefense = 6;
            fighter.MaxMagicResist = 6;
            fighter.Armor = 0;
            fighter.MaxSpeed = 200;

            fighter.AddAttack(AttackFactory.FlamingCharge);
            fighter.AddAttack(AttackFactory.FieryBite);
            fighter.AddAttack(TurnBasedAttackFactory.SenseFire);
            hellHound.Fighter = fighter;

            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] {"The hell hound drools fire", "Flarm flare up around the hell hounds body"}, fighter);


            return hellHound;
        }

        public static Monster LittleDevil(Player player)
        {
            Monster littleDevil = new Monster(35, 35, 300);
            littleDevil.MovementAI = new SmartMovement(littleDevil, player);
            littleDevil.Type = MonsterType.Imp;
            littleDevil.Type = MonsterType.Fire;
            littleDevil.Clone = LittleDevil;

            CombatEntity littleDevilFighter = new CombatEntity("Little Devil", 50);
            littleDevilFighter.MaxAttackStat = 10;
            littleDevilFighter.MaxMagicStat = 2;
            littleDevilFighter.MaxDefense = 4;
            littleDevilFighter.MaxSpeed = 150;
            littleDevilFighter.Armor = 0;
            littleDevilFighter.XP = level2MonsterXp;
            littleDevil.Fighter = littleDevilFighter;


            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] {"The Little devil sharpens its claws", "The little devil gets ready to pounce"},littleDevilFighter);
            littleDevilFighter.AddAttack(AttackFactory.HeatingClaws);

            return littleDevil;
        }

        public static Monster Salamander(Player player)
        {
            Monster salamander = new Monster(35, 35, 250);
            salamander.Type = MonsterType.Dragon;
            salamander.Clone = Salamander;
            salamander.MovementAI = new SmartMovement(salamander, player);

            //IDEA:
            // It burns both itself and the player. It gets stronger with burn. Burn still hits it though
            // It will reset its stats as burn
            CombatEntity fighter = new CombatEntity("Salamander", 150);
            fighter.Armor = 0;
            fighter.MaxMana = 250;
            fighter.Mana = 250;
            fighter.MaxAttackStat = 15;
            fighter.MaxMagicStat = 20;
            fighter.MaxDefense = 1;
            fighter.MaxMagicResist = 1;
            fighter.MaxSpeed = 100;
            fighter.XP = level3MonsterXp;
            

            fighter.AddAttack(AttackFactory.LavaBomb);
            fighter.AddAttack(AttackFactory.SalamderReset);
            fighter.AddAttack(AttackFactory.FireBall);



            fighter.AddAttack(TurnBasedAttackFactory.BurnLover);
            fighter.AddAttack(TurnBasedAttackFactory.HotBod);



            ConditionalAi ai = new ConditionalAi(fighter);
            //even turns 
            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity p) {
                return castor.Health * 2 < castor.MaxHealth;
            }, AttackFactory.SalamderReset);

            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity p) {
                return castor.Burn < 50;
            }, AttackFactory.LavaBomb);


            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity p) {
                return true;
            }, AttackFactory.FireBall);

            fighter.MyAttackAi = ai;
            salamander.Fighter = fighter;

            return salamander;
        }
        public static Monster Troll(Player player)
        {
            Monster troll = new Monster(35, 35, 150);
            troll.MovementAI = new SmartMovement(troll, player,senseRange:150);
            troll.Type = MonsterType.Brute;
            troll.Type = MonsterType.Orc;
            troll.Clone = Troll;

            CombatEntity trollFighter = new CombatEntity("Troll", 100);
            trollFighter.MaxAttackStat = 16;
            trollFighter.MaxMagicStat = 0;
            trollFighter.Armor = 0;
            trollFighter.MaxDefense = 7;
            trollFighter.MaxSpeed = 20;
            trollFighter.Armor = 0;
            trollFighter.XP = level2MonsterXp;
            troll.Fighter = trollFighter;


            trollFighter.AddAttack(AttackFactory.Slam);
            trollFighter.AddAttack(AttackFactory.Smash);

            trollFighter.AddAttackDecorator(AttackDecFactory.ShoulderGaurd);

            MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] { "The troll's thick fat makes them more resistent" }, trollFighter);



            return troll;
        }


        public static Monster Whelp(Player player)
        {
            Monster whelp = new Monster(35, 35,250);
            whelp.Type = MonsterType.Dragon;
            whelp.Clone = Whelp;
            whelp.MovementAI = new SmartMovement(whelp, player);

            CombatEntity fighter = new CombatEntity("Whelp", 90);
            fighter.MaxMana = 100;
            fighter.Mana = 100;
            fighter.MaxAttackStat = 16;
            fighter.MaxMagicStat = 26;
            fighter.MaxDefense = 9;
            fighter.MaxMagicResist = 9;
            fighter.MaxSpeed = 150;
            fighter.Armor = 0;
            fighter.XP = level3MonsterXp;

            fighter.AddAttack(AttackFactory.FireBreath);
            fighter.AddAttackDecorator(AttackDecFactory.Sleepy);

            MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] { "The baby dragon happy flaps its wings. Happy for a fight."}, fighter);


            whelp.Fighter = fighter;

            return whelp;
        }
    }
}
