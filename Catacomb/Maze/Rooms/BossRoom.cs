using Catacomb.CombatStuff;
using Catacomb.CombatStuff.Class;
using Catacomb.CombatStuff.MonsterUtils;
using Catacomb.Entities;
using Catacomb.Vectors;
using Catacomb.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Maze.Rooms
{
    class BossRoom : BaseTreasureRoom
    {
        private Monster bossMonster;
        private CombatEntity.AttackGenerator attackReward = null;
        private CatClass classReward = null;

        public BossRoom(Monster bossMonster, CombatEntity.AttackGenerator reward, Treasure.CreateTreasureExecute executeIn) :base(executeIn)
        {
            this.bossMonster = bossMonster;
            this.attackReward = reward;
        }
        public BossRoom(Monster bossMonster, CatClass reward, Treasure.CreateTreasureExecute executeIn) : base(executeIn)
        {
            this.bossMonster = bossMonster;
            this.classReward = reward;
        }

        public override Room Clone()
        {
            if (attackReward != null)
            {
                return new BossRoom(bossMonster,attackReward,GetExecute);
            }
            else
            {
                return new BossRoom(bossMonster, classReward,GetExecute);
            }
        }

        public override void Create(Point p1, Point p2)
        {
            if (roomDrawn == null)
            {

                roomDrawn = new DrawnBossRoom(this, p1, p2,bossMonster,attackReward,classReward,GetExecute);
            }
        }
    }
    class DrawnBossRoom : BaseDrawnTreasureRoom
    {
        private Point spawnPoint;
        private Monster bossMonster;
        private CombatEntity.AttackGenerator attackReward = null;
        private CatClass classReward = null;
        public DrawnBossRoom(Room parent, Point p1, Point p2, Monster bossMonster, CombatEntity.AttackGenerator reward1, CatClass reward2, Treasure.CreateTreasureExecute getExecute) : base(parent,p1,p2,getExecute)
        {
            this.bossMonster = bossMonster;
            classReward = reward2;
            attackReward = reward1;
        }


        protected override void DrawRoom()
        {
            base.DrawFloor();
            base.DrawRep();

            double width = 150;
            double height = 250;
            double gap = 75;

            Point start = convertPointToLocal(this.Center).AddPoint(new Point(-width / 2, -height / 2));
            Point second = new Point(start.X + width, start.Y);
            Point third = new Point(second.X, start.Y + height - gap);
            Point fourth = new Point(third.X, start.Y + height);
            Point final = new Point(start.X, fourth.Y);
            base.AddChild(new Wall(start, second));
            base.AddChild(new Wall(second, third));
            base.AddChild(new Wall(fourth, final));
            base.AddChild(new Wall(final, start));
        }
        public override void MustExecute(CatMaze mazeIn)
        {
            this.spawnPoint = this.Center;
            Monster boss = bossMonster.Clone(mazeIn.Player);
            boss.MovementAI = new BasicMovement(boss, mazeIn.Player,100);
            if(this.attackReward != null)
            {
                MonsterJuicer.AddReward(attackReward, boss.Fighter);
            }
            if (this.classReward != null)
            {
                MonsterJuicer.AddReward(classReward, boss.Fighter);
            }
            boss.PlaceMonster(spawnPoint);
            boss.Container = this;
            mazeIn.AddMonster(boss);
            base.AddTreasure(this.convertPointToLocal(spawnPoint).MinusPoint(new Point(0,100)));


        }
    }
}
