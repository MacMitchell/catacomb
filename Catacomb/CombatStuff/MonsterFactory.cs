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
            CombatEntity player = new CombatEntity(name,2100);
            player.AddAttack(AttackFactory.Tackle);
            player.IsPlayer = true;
            return player;
        }
        public static CombatEntity GenerateSlime()
        {
            CombatEntity slime = new CombatEntity("Slime",100);
            slime.AddAttack(AttackFactory.Tackle);
            return slime;
        }
    }
}
