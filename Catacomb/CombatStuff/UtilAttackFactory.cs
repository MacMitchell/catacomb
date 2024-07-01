using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    class UtilAttackFactory
    {
        public static Attack GenerateTextAttack(Command parent, String text)
        {
            Attack newAttack = new Attack(parent);
            newAttack.ExecuteAttack = (CombatEntity no, CombatEntity nope) =>
            {
                newAttack.Description = text;
            };
            return newAttack;
        }
        public static Attack GenerateDefeatAttack(CombatEntity castor, Command parent)
        {
            String defeatText = castor.Name + " was defeated!";
            Attack defeatMessage = GenerateTextAttack(parent, defeatText);
            GenerateXPAttack(castor, defeatMessage);
            return defeatMessage;
        }
        public static Attack GenerateXPAttack(CombatEntity defeated, Command parent)
        {
            String xp = "You gained " + defeated.XP + "XP!";
            return GenerateTextAttack(parent, xp);
        }

        public static Attack DefaultEndOfCombatAttack(CombatEntity defeated, Command parent)
        {
            if(defeated.Health <= 0)
            {
                return GenerateDefeatAttack(defeated,parent);
            }
            return null;
        }
        public static Attack DefaultPlayerEndOfCombat(CombatEntity player, Command parent)
        {
            if(player.Health <= 0)
            {
                return GenerateTextAttack(parent, "You were defeated....");
            }
            return GenerateTextAttack(parent, "You continue to venture into the dark");
        }
    }
}
