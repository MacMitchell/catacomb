using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Global;
using Catacomb.CombatStuff;
using Catacomb.CombatStuff.Class;

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

        public static void AddReward(CombatEntity.AttackGenerator reward, CombatEntity monster)
        {
            CombatEntity.AttackGenerator test2 = delegate (CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec) {
                Attack textAttack = new Attack(parent);
                textAttack.Type = AttackType.EOC;
                textAttack.ExecuteAttack = delegate (CombatEntity no, CombatEntity mope)
                {
                    textAttack.Description = "After the battle, you are rewarded with ..." + reward(textAttack.Target, null, null, null).Name;
                    textAttack.Target.AddAttack(reward);
                };
                return textAttack;
            };
            monster.AddAttack(test2);
        }
        public static void AddReward(CatClass reward, CombatEntity monster)
        {
            CombatEntity.AttackGenerator test2 = delegate (CombatEntity castor, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec) {
                Attack textAttack = new Attack(parent);
                textAttack.Type = AttackType.EOC;
                textAttack.ExecuteAttack = delegate (CombatEntity no, CombatEntity mope)
                {
                    textAttack.Description = "After the battle, you are rewarded with ..." + reward.Name;
                    ((CombatPlayer) textAttack.Target).AddClass(reward);
                };
                return textAttack;
            };
            monster.AddAttack(test2);
        }
    }
}
