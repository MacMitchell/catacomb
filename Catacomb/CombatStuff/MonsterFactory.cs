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
        Imp,
        Demon,
        Fire,
        Ethereal,
        Elemental,
    }
    class MonsterFactory
    {

        public static CombatEntity GeneratePlayer(string name = "Player")
        {
            CombatPlayer player = new CombatPlayer(name,100);
            player.MaxHealth = 500;
            player.Health = 500;
            player.MaxMagicStat = 50;
            player.CurrentCatClass = CatClassFactory.Mage(player);
            player.Armor = 0;
            player.MaxDefense = 5;
            player.MaxMagicResist = 5;

            player.IsPlayer = true;

            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.FireBall);
            player.AddAttack(AttackFactory.Leech);
            player.AddAttack(AttackFactory.Bulster);

            //testing
            player.AddAttackDecorator(AttackDecFactory.CoolFlames);
            player.AddAttack(AttackFactory.SelfIgnition);
            

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
            slime.XP = 100;
            slime.AddAttack(AttackFactory.Tackle);
            slime.AddAttack(AttackFactory.Leech);

            slime.AddAttackDecorator(AttackDecFactory.PoisonTouch);
            slime.AddAttackDecorator(AttackDecFactory.ShoulderGaurd);
            
            return slime;
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
            fireImp.XP = 100;
           // fireImp.AddAttack(AttackFactory.Tackle);
            fireImp.AddAttack(AttackFactory.FireBall);
            fireImp.AddAttack(AttackFactory.FireBlast);
            return fireImp;
        }

        public static Monster FireWisp(Player player)
        {
            Monster fireWisp = new Monster(35, 35, 25);
            fireWisp.MovementAI = new SmartMovement(fireWisp, player);
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
            firewispCombat.XP = 75;
            fireWisp.Fighter = firewispCombat;
            return fireWisp;
            
        }
        public static Monster GoblinScout(Player player)
        {
            Monster goblinScout = new Monster(35, 35, 200);
            //fireImp.MovementAI = new BasicWonderWithHuntingMovement(fireImp, player,150,Math.PI/4,500,Math.PI/2);
            goblinScout.MovementAI = new SmartMovement(goblinScout, player);
            goblinScout.Type = MonsterType.Imp;
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
            goblinScout.XP = 100;
            goblinScout.AddAttack(AttackFactory.PoisonSlash);
            goblinScout.AddAttack(AttackFactory.Slash);
            goblinScout.AddAttack(TurnBasedAttackFactory.Rage);
            return goblinScout;
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
            littleDevilFighter.XP = 100;
            littleDevil.Fighter = littleDevilFighter;


            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] {"The Little devil sharpens its claws", "The little devil gets ready to pounce"},littleDevilFighter);
            littleDevilFighter.AddAttack(AttackFactory.HeatingClaws);

            return littleDevil;
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
            //add attacks
            fighter.AddAttack(AttackFactory.FireBall);
            fighter.AddAttack(AttackFactory.Ignite);
            fighter.AddAttack(AttackFactory.Heatwave);

            fighter.AddAttack(TurnBasedAttackFactory.FlamingOutrage);

            ConditionalAi ai = new ConditionalAi(fighter);
            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity player) { return player.Burn < 50; }, AttackFactory.Ignite);
            ai.AddConditionalCast(delegate (CombatEntity castor, CombatEntity player) { return player.Burn >= 50; }, AttackFactory.Heatwave);

            fighter.MyAttackAi = ai;

            MonsterUtils.MonsterJuicer.GenerateRandomStartOfTurnVoice(new[] {"A tornado of flames whirls around you both.", "The ancient fire being looms over you.", "The elemental body of flames flares up."}, fighter);
            fireElemental.Fighter = fighter;
            return fireElemental;

        }
    }
}
