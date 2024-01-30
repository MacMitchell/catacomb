using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    class MonsterFactory
    {

        public static CombatEntity GeneratePlayer(string name = "Player")
        {
            CombatEntity player = new CombatEntity(name,510);
            player.Armor = 0;
            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.Tackle);
            player.AddAttack(AttackFactory.FireBall);
            player.AddAttack(AttackFactory.Leech);
            player.AddAttack(AttackFactory.Bulster);
            player.IsPlayer = true;
            return player;
        }
        public static CombatEntity GenerateSlime()
        {
            CombatEntity slime = new CombatEntity("Slime",100);
            //slime.AddAttack(AttackFactory.Tackle);
            slime.AddAttack(AttackFactory.Leech);
            return slime;
        }
    }
}
