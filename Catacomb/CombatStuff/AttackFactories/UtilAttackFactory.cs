using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.CombatStuff.Class;
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

        public static Attack GenerateFollowUpTextAttack(Command parent, String text)
        {
            Attack followUp = new Attack(parent);

            followUp.ExecuteAttack = (CombatEntity UNUSED, CombatEntity UNSUED2) =>
            {
                followUp.Description = text;
            };
            return followUp;
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

        public static Attack DefaultEndOfCombatAttack(CombatEntity defeated, Command parent, CommandIterator it, CombatEntity other, AttackDecorator dec = null)
        {
            if(defeated.Health <= 0)
            {
                return GenerateDefeatAttack(defeated,parent);
            }
            return null;
        }

        public static Attack DefaultStartOfCombatAttack(CombatEntity monster, Command parent, CommandIterator it, CombatEntity other, AttackDecorator notUSED = null)
        {
            monster.PrepAttack();
            return GenerateTextAttack(parent, "A " + monster.Name + "  appeared!");
        }


        public static Attack DefaultPlayerStartOfCombatAttack(CombatEntity monster, Command parent, CommandIterator it, CombatEntity other, AttackDecorator NOTUSED = null)
        {
            monster.PrepAttack();
            return GenerateTextAttack(parent, "You prepare for combat.");
        }
        public static Attack DefaultPlayerEndOfCombat(CombatEntity player, Command parent, CommandIterator it,CombatEntity monster, AttackDecorator dec = null)
        {
            if(player.Health <= 0)
            {
                return GenerateTextAttack(parent, "You were defeated....");
            }
            new CheckForLevelUpCommand(it, parent, (CombatPlayer)player, monster);
            var temp =  GenerateTextAttack(parent, "You continue to venture into the dark");
            return temp;
        }



        public static Attack SymmetricEndOfTurn(CombatEntity castor, Command parent, CommandIterator it, CombatEntity target, AttackDecorator dec = null)
        {
            CreatePoisonAttack(castor, parent, it, target, dec);
            return null;
        }

        private static void CreatePoisonAttack(CombatEntity castor, Command parent, CommandIterator it, CombatEntity target, AttackDecorator dec = null)
        {
            
            Attack utilAttack = Attack.CreateAttack(castor, parent, it, target, dec);
            utilAttack.ExecuteAttack = (CombatEntity no, CombatEntity nope) =>
            {
                if (castor.Poison > 0)
                {
                    Attack poisonAttack = Attack.CreateAttack(castor, parent, it, target, dec != null ?dec.Clone():null);
                    poisonAttack.ExecuteAttack = (CombatEntity innerNo, CombatEntity innerNope) =>
                    {
                        double amount = poisonAttack.TakePoisonDamge(castor.Poison, castor);
                        poisonAttack.Description = castor.Name + " took " + amount + " from poison!";
                    };
                }
                if(castor.Burn >0)
                {
                    Attack burnAttack = Attack.CreateAttack(castor, parent, it, target, dec != null ? dec.Clone() : null);
                    burnAttack.ExecuteAttack= (CombatEntity innerNo, CombatEntity innerNope) =>
                    {
                        double amount = burnAttack.TakeBurnDamage(castor.Burn, castor);
                        burnAttack.Description = castor.Name + " took " + amount + " from burning!";
                    };
                }
                utilAttack.CommandReturnResult = it.ExecuteNext(no,nope);
            };
        }
    }
}
