using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.CombatStuff
{
    public class AdminCommands : Command
    {

        protected CommandIterator it;
        public AdminCommands(CommandIterator iteratorIn, Command parent=null):base(parent)
        {
            it = iteratorIn;
            Description = "YOU SHOULD NOT BE SEEING THIS";
        }
        
        protected int ExecuteNext(CombatEntity castor,CombatEntity target)
        {
            it.Next();
            return it.CurrentCommand.Execute(castor, target);
        }
        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            //this method actually does not want to public know something was execute so it will call the next function first
            ExecuteNext(castor,target);
            return Command.IGNORE_COMMAND;
           
        }
    }

    public abstract class FetchAttackCommand : AdminCommands
    {
        protected CombatEntity player;
        protected CombatEntity monster;
        public FetchAttackCommand(CommandIterator it, Command parent, CombatEntity entity1, CombatEntity entity2) : base(it, parent)
        {
            this.player = entity1.IsPlayer ? entity1 : entity2;
            this.monster = entity1.IsPlayer? entity2 : entity1;
        }
    }


    /**
     * This class is for setting up the turn. It does not directly fetch any attacks
     */ 
    public class StartOfTurnCommand : AdminCommands
    {
        protected CombatEntity castor;
        protected CombatEntity target;
        public StartOfTurnCommand(CommandIterator it, Command parent, CombatEntity castor, CombatEntity target) : base(it, parent)
        {
            this.castor = castor;
            this.target = target;
        }
        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            this.Next = new StartOfTurnCommand(it, null, castor, target);
            new FetchStartOfTurnAttack(it, this, castor, target);
            new GetAttacksCommand(it, this);
            return ExecuteNext(castor, target);
        }
    }

    public class FetchStartOfTurnAttack : FetchAttackCommand
    {
        public FetchStartOfTurnAttack(CommandIterator it, Command parent, CombatEntity player, CombatEntity monster) : base(it, parent, player, monster) { }

        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            monster.GetStartOfTurnAttack(it, this, player);
            player.GetStartOfTurnAttack(it, this, monster);

            new CheckForCombatEnd(it, this, player, monster);
            return ExecuteNext(castor, target);
        }
    }

    public class FetchEndOfTurnAttack : FetchAttackCommand
    {
        public FetchEndOfTurnAttack(CommandIterator it, Command parent, CombatEntity player, CombatEntity monster) : base(it, parent, player, monster) { }

        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            monster.GetEndOfCombatAttack(it, this, player);
            player.GetEndOfCombatAttack(it, this, monster);

            new CheckForCombatEnd(it, this, player, monster);
            return base.ExecuteNext(castor, target);
        }
    }
    public class GetAttacksCommand : AdminCommands
    {
        public GetAttacksCommand(CommandIterator iteratorIn, Command parent= null) : base(iteratorIn, parent){}
        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            //this assumes that neither of them are actually the castor or the target
            double castorSpeed = castor.Speed;
            double targetSpeed = target.Speed;
            
            CombatEntity faster;
            CombatEntity slower;
            if(castorSpeed > targetSpeed)
            {
                faster = castor;
                slower = target;
            }
            else
            {
                faster = target;
                slower = castor;
            }
            double fasterSpeed = faster.Speed;
            double slowerSpeed = slower.Speed;
            CreateGetAttackCommand(faster, slower);

            /*Attack next = faster.GetAttack(this); 
            next.Castor = faster;
            next.Target = slower;
            */
            fasterSpeed *= (double)(2.0 / 3.0);
            while(fasterSpeed > slowerSpeed)
            {
                
                CreateGetAttackCommand(faster, slower);
                fasterSpeed *= (double)(1.5 / 3.0);
            }
            /*next = slower.GetAttack(this);
            next.Castor = slower;
            next.Target = faster;*/
            CreateGetAttackCommand(slower, faster);
            return ExecuteNext(faster, slower);
        }

        private void CreateGetAttackCommand(CombatEntity castor, CombatEntity target)
        {
            if (castor.IsPlayer)
            {
                new FetchPlayerAttackCommand(it, this, castor, target);
            }
            else
            {
                new FetchMonsterAttackCommand(it, this, castor, target);
            }
            new CheckForCombatEnd(it, this, castor, target);
        }
    }

    public class CheckForCombatEnd : AdminCommands {
        protected CombatEntity castor;
        protected CombatEntity target;
        public CheckForCombatEnd(CommandIterator it,Command parent,CombatEntity castor, CombatEntity target): base(it, parent)
        {
            this.castor = castor;
            this.target = target;
        }
        public override int Execute(CombatEntity NO, CombatEntity IShouldRemoveThis)
        {
             if(castor.Health <= 0 || target.Health <= 0)
            {
                CombatEntity player = castor.IsPlayer ? castor : target;
                CombatEntity monster = !castor.IsPlayer ? castor : target;
                new FetchEndOfCombatCommand(it, this, player, monster);
            }
            return ExecuteNext(castor, target);
        }
    }

    public class FetchStartOfCombatAttack : AdminCommands
    {
        protected CombatEntity player;
        protected CombatEntity monster;
        public FetchStartOfCombatAttack(CommandIterator it, Command parent, CombatEntity player, CombatEntity monster): base(it, parent)
        {
            this.player = player;
            this.monster = monster;
        }
        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            monster.GetStartOfCombatAttack(it, this, player);
            player.GetStartOfCombatAttack(it, this, monster);
            this.Next = new StartOfTurnCommand(it, parent, player, monster);
            
            return base.ExecuteNext(monster,player);
        }
    }

 
    public class FetchEndOfCombatCommand: AdminCommands
    {

        protected CombatEntity player;
        protected CombatEntity monster;
        public FetchEndOfCombatCommand(CommandIterator it, Command parent, CombatEntity player, CombatEntity monster): base(it, parent)
        {
            this.player = player;
            this.monster = monster;
        }
        public override int Execute(CombatEntity NOTUSED, CombatEntity NOTUSED2)
        {
            Attack nextAttack = monster.GetEndOfCombatAttack(it,parent,player);
            nextAttack.Castor = monster;
            nextAttack.Target = player;

            Attack playerAttack = player.GetEndOfCombatAttack(it,parent,monster);
            playerAttack.Castor = player;
            playerAttack.Target = monster;

            new FinishCombatCombat(it, parent);
            return ExecuteNext(monster, player);
        }
    }

    public class FinishCombatCombat: AdminCommands
    {
        public FinishCombatCombat(CommandIterator iteratorIn, Command parent = null) : base(iteratorIn, parent){}
        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            return Command.COMBAT_FINISHED;
        }
    }


    public class FetchMonsterAttackCommand : AdminCommands
    {
        protected CombatEntity castor;
        protected CombatEntity target;
        public FetchMonsterAttackCommand(CommandIterator it, Command parent, CombatEntity castor, CombatEntity target) : base(it, parent)
        {
            this.castor = castor;
            this.target = target;
        }
        public override int Execute(CombatEntity NOTUSED, CombatEntity NOTUSED2)
        {
            Attack nextAttack = castor.GetAttack(this,it,target);
            nextAttack.Castor = castor;
            nextAttack.Target = target;
            ExecuteNext(castor, target);
            return Command.IGNORE_COMMAND;
        }
    }

    public class FetchPlayerAttackCommand:FetchMonsterAttackCommand
    {
        public FetchPlayerAttackCommand(CommandIterator it, Command parent, CombatEntity castor, CombatEntity target) : base(it, parent,castor,target)
        {
            this.castor = castor;
            this.target = target;
        }
        public override int Execute(CombatEntity castor, CombatEntity target)
        {
            Description = "Select Player Attack (press space to tackle)";
            return Command.FETCH_PLAYER_ATTACK;
        }
    }
}
