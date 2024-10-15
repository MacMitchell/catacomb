using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Global;
using Catacomb.CombatStuff;
namespace Catacomb.CombatStuff.MonsterUtils
{
    /**
     * A class made just to add more flavor to mosnters such as giving them lines at the start of turn
     */
    public class MonsterJuicer
    {
        public static void GenerateRandomStartOfTurnVoice(string[] lines, CombatEntity monster)
        {
            CombatEntity.AttackGenerator test2 = delegate (CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec){
                Attack textAttack = new Attack(parent);
                textAttack.Type = AttackType.SOT;
                textAttack.ExecuteAttack += delegate (CombatEntity no, CombatEntity mope)
                {
                    textAttack.Description = lines[Globals.Rand.Next(0, lines.Length)];
                };
                return textAttack;
            };
            monster.AddAttack(test2);
            return;
        }
    }
}
