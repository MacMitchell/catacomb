using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.CombatStuff.Class;
using Catacomb.CombatStuff.AttackFactories;
using Catacomb.Entities;

namespace Catacomb.CombatStuff
{
    public enum MonsterType
    {
        Slime,
        All,
        Imp,
    }
    class MonsterFactory
    {

        public static CombatEntity GeneratePlayer(string name = "Player")
        {
            CombatPlayer player = new CombatPlayer(name,510);
            player.MaxMagicStat = 50;
            player.CurrentCatClass = CatClassFactory.Mage(player);
            player.Armor = 0;
            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.FireBall);
            player.AddAttack(AttackFactory.Leech);
            player.AddAttack(AttackFactory.Bulster);

            player.IsPlayer = true;


            return player;
        }
    public static Monster GreenSlime(Player player)
        {
            Monster greenSlime = new Monster(35, 35, 10);
            greenSlime.MovementAI = new BasicMovement(greenSlime, player);
            greenSlime.Fighter = MonsterFactory.GreenSlimeCombat();
            greenSlime.Type = MonsterType.Slime;
            greenSlime.Clone = GreenSlime;
            return greenSlime;
        }
     public static CombatEntity GreenSlimeCombat()
        {
            CombatEntity slime = new CombatEntity("Slime",100);
            slime.XP = 100;
            slime.AddAttack(AttackFactory.Tackle);
            slime.AddAttack(AttackFactory.Leech);
            return slime;
        }

        public static Monster FireImp(Player player)
        {
            Monster fireImp = new Monster(35, 35, 100);
            fireImp.MovementAI = new BasicMovement(fireImp, player);
            fireImp.Type = MonsterType.Imp;
            fireImp.Clone = FireImp;

            fireImp.Fighter = FireImpCombat();
            return fireImp;
        }

        public static CombatEntity FireImpCombat()
        {
            CombatEntity fireImp = new CombatEntity("FireImp", 100);
            fireImp.MaxHealth = 90;
            fireImp.MaxMagicStat = 59;
            fireImp.Armor = 0;
            fireImp.MaxDefense = 39;
            fireImp.XP = 100;
            fireImp.AddAttack(AttackFactory.Tackle);
            fireImp.AddAttack(AttackFactory.FireBall);
            fireImp.AddAttack(AttackFactory.FireBlast);
            return fireImp;
        }
    
    }
}
