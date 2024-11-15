using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Catacomb.CombatStuff.CombatEntity;

namespace Catacomb.CombatStuff.MonsterUtils
{
    //get attack is just random
    public class AttackAi
    {
        protected CombatEntity castor;
        public AttackAi(CombatEntity castor)
        {
            this.castor = castor;
        }

        public bool CanCast(AttackGenerator attack)
        {
            double manaCost = Math.Abs(attack(this.castor, null, null, null, null).SelfManaDrain);
            return manaCost <= castor.Mana;
        }

        public Attack CreateAttack(AttackGenerator gen, Command parentIn, CommandIterator it, CombatEntity other)
        {
            return castor.GetAttack(gen, parentIn, it, other);
        }

        public virtual Attack GetAttack(Command parentIn, CommandIterator it, CombatEntity other)
        {
            List<AttackGenerator> backupList = new List<AttackGenerator>(castor.TempGenerateAttacks);

            int index = rand.Next(0, castor.TempGenerateAttacks.Count);
            Attack temp = castor.TempGenerateAttacks[index](castor, null, null, null);
            while (Math.Abs(temp.SelfManaDrain) > castor.Mana)
            {
                castor.TempGenerateAttacks.RemoveAt(index);
                if (castor.TempGenerateAttacks.Count == 0)
                {
                    castor.TempGenerateAttacks = new List<AttackGenerator>(backupList);
                    return CreateAttack(AttackFactory.Tackle, parentIn, it, other);
                }
                index = rand.Next(0, castor.TempGenerateAttacks.Count);

                temp = castor.TempGenerateAttacks[index](castor, null, null, null);
            }
            castor.TempGenerateAttacks = new List<AttackGenerator>(backupList);
            temp = CreateAttack(castor.TempGenerateAttacks[index], parentIn, it, other);
            return temp;
        }
    }

    public class ConditionalAi :AttackAi
    {
        public delegate bool ConditionalCast(CombatEntity castor, CombatEntity target);

        private List<ConditionalCast> casts;
        private List<AttackGenerator> attacks;
        public ConditionalAi(CombatEntity castor) : base(castor) 
        {
            casts = new List<ConditionalCast>();
            attacks = new List<AttackGenerator>();
        }


        public void AddConditionalCast(ConditionalCast cast, AttackGenerator result)
        {
            casts.Add(cast);
            attacks.Add(result);
        }

        public override Attack GetAttack(Command parentIn, CommandIterator it, CombatEntity other)
        {
            for(int i =0; i < casts.Count; i++)
            {
                bool result1 = casts[i](this.castor, other);
                bool result2 = base.CanCast(attacks[i]);
                if (result1 && result2){
                    return base.CreateAttack(attacks[i], parentIn, it, other);
                }
            }

            return base.GetAttack(parentIn, it, other);
        }
    }
}
