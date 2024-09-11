using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;


using Catacomb.Maze;
using Catacomb.Vectors;
using Catacomb.Global;
using Catacomb.Visuals;

namespace Catacomb.Entities
{
    public abstract class Movement
    {
        protected Monster monster = null;
        protected Player player = null;
        protected static int calculateTime = 10;
        public abstract bool SetUpMove(double time);
        /**
        * Not a very efficient way to calculate the angle 
        */
        protected double CalculateAngleFromPlayer()
        {
            double angle = Point.GetAngleBetweenPoints(monster.Position, player.Position);
            return -angle;
        }

        protected bool DoesMonsterSeePoint(Point dest, double senseRange = -1, double senseAngle = -1)
        {
            double distance = dest.GetDistance(monster.Position);
            double angle = Point.GetAngleBetweenPoints(monster.Position, dest);

            if (senseRange != -1 && distance >= senseAngle)
            {
                return false;
            }
            if (senseAngle != -1 && Math.Abs(angle - monster.Angle) > senseAngle)
            {
                return false;    
            }
            //if it can move the total distance then it can 'see' the player
            double oldAngle = monster.Angle;
            monster.Angle = angle;
            bool playerSeen = monster.Container.EntityMove(monster, distance - monster.Width);
            monster.Angle = oldAngle;
            return playerSeen;
        }

        protected double CalcuateDistanceFromPoint(Point point)
        {
            return monster.Position.GetDistance(point);
        }
    }


    public class BasicMovement : Movement
    {
        protected Point movementPoint;
        protected double senseRange;
        private int movementCounter;
        public BasicMovement(Monster parentIn, Player playIn, double senseRangeIn = 500)
        {
            movementCounter = 0;
            monster = parentIn;
            player = playIn;
            senseRange = senseRangeIn;
        }

        private bool DoesMonsterSeePlayer()
        {
            double distance = player.Position.GetDistance(monster.Position);
            if (distance < senseRange)
            {
                double angle = base.CalculateAngleFromPlayer();
                //if it can move the total distance then it can 'see' the player
                double oldAngle = monster.Angle;
                monster.Angle = angle;
                bool playerSeen = monster.Container.EntityMove(monster, distance - monster.Width);
                monster.Angle = oldAngle;
                return playerSeen;
            }
            return false;
        }



        public override bool SetUpMove(double time)
        {
            if (DoesMonsterSeePlayer())
            {
                monster.Angle = base.CalculateAngleFromPlayer();
                movementCounter = 100;
                movementPoint = player.Position;
                return true;
            }
            //DOES not see the player but has recently seen them
            if (movementCounter > 0)
            {
                if (movementPoint.GetDistance(monster.Position) < 25)
                {
                    movementCounter = 0;
                    return false;
                }

                //movementCounter--;
                return true;
            }
            return false;
        }

    }
    public class BasicWonderingMovement : Movement
    {
        protected int wonderRange;
        protected Point destination;
        protected bool pointReady = false;
        double angleOffset;
        public BasicWonderingMovement(Monster parentIn, Player playIn, int wonderRange = 500, double angleOffset = Math.PI / 12.0)
        {
            monster = parentIn;
            player = playIn;
            this.wonderRange = wonderRange;
            this.angleOffset = angleOffset;
        }

        public void ResetPoint()
        {
            pointReady = false;
        }

        private bool hitPoint()
        {
            double minDistance = 50;
            double distance = destination.GetDistance(monster.Center);
            return minDistance > Math.Abs(distance);
        }
        public virtual void CreateBrandNewPoint()
        {
            /* int xDistance = Global.Globals.Rand.Next(-wonderRange, wonderRange);
             int yDistance = Global.Globals.Rand.Next(-wonderRange, wonderRange);
             destination = monster.Center.AddPoint(new Point(xDistance, yDistance));*/
            CatRectangle room = (CatRectangle) monster.GetCurrentRoom().Representive;
            destination = room.CreatePointInRectangle();
            monster.Angle = -Point.GetAngleBetweenPoints(monster.Center, destination);
            pointReady = true;
        }

        public virtual void CreatePointWithoutChangingDirection(int range, double angle)
        {
            double newAngle = Global.Globals.GetRandomNumber(monster.Angle - angle, monster.Angle + angle);
            double distance = Global.Globals.Rand.Next(5, range);
            monster.Angle = newAngle;
            destination = monster.Position.AddPoint(new Point(distance * Math.Cos(newAngle), distance * Math.Sin(newAngle)));
        }

        protected virtual void adjustPoint()
        {
            double newAngle = Global.Globals.GetRandomNumber(monster.Angle - angleOffset, monster.Angle + angleOffset);
            double distance = Global.Globals.Rand.Next(50, wonderRange);
            monster.Angle = newAngle;
            destination = destination.AddPoint(new Point(distance * Math.Cos(newAngle), distance * Math.Sin(newAngle)));
        }
        public override bool SetUpMove(double time)
        {
            if (pointReady && !monster.Container.EntityMove(monster, time * monster.Velocity))
            {
                CreateBrandNewPoint();
            }
            else if (pointReady && hitPoint())
            {
                adjustPoint();
            }
            if (!pointReady)
            {
                CreateBrandNewPoint();
            }
            return true;
        }
    }
    public class BasicWonderWithHuntingMovement : BasicWonderingMovement
    {
        protected double senseRange;
        protected double calculateTimer;
        protected double senseAngle;
        protected double onTheHuntSenseBonus = 4.0;
        protected bool playerFound;
        public bool Hunting
        {
            get { return playerFound; }
        }
        public BasicWonderWithHuntingMovement(Monster parentIn, Player playIn, int wonderRange = 500, double angleOffset = Math.PI / 12.0, double senseRange = 500, double senseAngle = Math.PI / 2.0) : base(parentIn, playIn, wonderRange, angleOffset)
        {
            this.senseRange = senseRange;
            this.senseAngle = senseAngle;
            calculateTimer = calculateTime;
            playerFound = false;
        }

        private bool DoesMonsterSeePlayer()
        {
            double distance = player.Position.GetDistance(monster.Position);
            if (distance >= senseRange)
            {
                return false;
            }

            double angle = base.CalculateAngleFromPlayer();
            //is the player in the monster's line of sight
            if (Math.Abs(angle - monster.Angle) > senseAngle)
            {
                return false;
            }

            //if it can move the total distance then it can 'see' the player
            double oldAngle = monster.Angle;
            monster.Angle = angle;
            bool playerSeen = monster.Container.EntityMove(monster, distance - monster.Width);
            monster.Angle = oldAngle;
            return playerSeen;

        }

        protected void MoveToPlayer()
        {
            if (!playerFound)
            {
                senseAngle *= onTheHuntSenseBonus;

            }
            playerFound = true;
            monster.Angle = base.CalculateAngleFromPlayer();
            destination = player.Center;
        }
        public override bool SetUpMove(double time)
        {
            calculateTimer--;
            //if monster does not see player, just wonder
            if (calculateTimer == 0 && DoesMonsterSeePlayer())
            {
                MoveToPlayer();
            }
            else if (calculateTimer == 0)
            {
                if (playerFound)
                {
                    senseAngle /= onTheHuntSenseBonus;
                }
                playerFound = false;
            }
            if (calculateTimer == 0)
            {
                calculateTimer = calculateTime;
            }

            return base.SetUpMove(time);


        }
    }

    public class WonderInRoomOnly : BasicWonderingMovement
    {
        double safeDistance = 25;

        public WonderInRoomOnly(Monster parentIn, Player playIn, int wonderRange = 500, double angleOffset = Math.PI / 12.0):base(parentIn, playIn, wonderRange, angleOffset) 
        {
            
        }
        public override void CreateBrandNewPoint()
        {
            CatRectangle monsterRoom = (CatRectangle)monster.Container.Representive;

            destination = monsterRoom.CreatePointInRectangle(50);
            monster.Angle = -Point.GetAngleBetweenPoints(monster.Center, destination);
            pointReady = true;
        }

        public override void CreatePointWithoutChangingDirection(int range, double angle)
        {
            CreateBrandNewPoint();
        }

        protected override void adjustPoint()
        {
            
        }
    }
    public class SmartMovement : Movement
    {
        private Vector[] collisionTest;
        private bool[] collisionResults;
        private static double collisionOffset = 18;
        private double calculationTimer;
        private bool moving;
        private Rectangle[] debugDrawing;
        private Point destination;
        private bool started;

        private DrawnRoom currentRoom;
        private int lastConnectionPoint;
        private double oldAngle;

        private MovementState state;

        public SmartMovement(Monster parentIn, Player playIn)
        {
            monster = parentIn;
            player = playIn;

            debugDrawing = new Rectangle[4];


            calculationTimer = 0;
            moving = false;
            started = false;

            collisionTest = new Vector[Globals.CONNECTION_LIMIT];
            collisionResults = new bool[Globals.CONNECTION_LIMIT];

            state = new Wondering(this, monster, playIn);
        }

        public void SetMonsterAngle(double angle)
        {
            
            monster.Angle = angle;
            oldAngle = monster.Angle;
        }
        private void DefaultMove()
        {
            monster.Angle = Math.PI / 12;
            for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
            {
                collisionResults[i] = false;
                double skinnyOffset = 15;

                collisionTest[i] = monster.Representive.Clone();
                Point offset = ((CatRectangle)collisionTest[i]).TopLeft;//monster.Position.MinusPoint(new Point(monster.Width/2.0, monster.Height/2.0));
                ((CatRectangle)collisionTest[i]).expand(i, collisionOffset / 2.0);
                ((CatRectangle)collisionTest[i]).expand(Room.GetOppositeDirection(i), i % 2 == 0 ? -monster.Height / 2.0 : -monster.Width / 2.0);
                ((CatRectangle)collisionTest[i]).expand(Room.GetOppositeDirection((i+1) % 4), -skinnyOffset / 2.0);

                ((CatRectangle)collisionTest[i]).expand(Room.GetOppositeDirection((i+3) % 4), -skinnyOffset / 2.0);


                if (Globals.SHOW_COLLISION_TESTERS)
                {
                    debugDrawing[i] = new Rectangle();
                    debugDrawing[i].Width = ((CatRectangle)collisionTest[i]).GetWidth();
                    debugDrawing[i].Height = ((CatRectangle)collisionTest[i]).GetHeight();
                    debugDrawing[i].Fill = Brushes.Orange;
                    debugDrawing[i].Opacity = 0.15;
                    monster.GetCanvas().Children.Add(debugDrawing[i]);
                }
                collisionTest[i].OffsetX = -offset.X;
                collisionTest[i].OffsetY = -offset.Y;
            }

            started = true;
        }

        private bool IsCollisionTestColliding(int direction)
        {
            double previousOffsetX = collisionTest[direction].OffsetX;
            double previousOffsetY = collisionTest[direction].OffsetY;

            Point monsterOffset = monster.TopLeft;

            collisionTest[direction].OffsetX += monsterOffset.X;
            collisionTest[direction].OffsetY += monsterOffset.Y;

            if (Globals.SHOW_COLLISION_TESTERS)
            {
                Canvas.SetLeft(debugDrawing[direction], ((CatRectangle)collisionTest[direction]).GetTopLeft().X - monster.Position.GetX() + (monster.Width / 2.0));
                Canvas.SetTop(debugDrawing[direction], ((CatRectangle)collisionTest[direction]).GetTopLeft().Y - monster.Position.GetY() + (monster.Height / 2.0));
            }

            bool results = monster.Container.DoesVectorIntersectConnectingRooms(collisionTest[direction]);

            collisionTest[direction].OffsetX = previousOffsetX;
            collisionTest[direction].OffsetY = previousOffsetY;

            currentRoom = monster.Container;
            lastConnectionPoint = -1;
            return results;
        }

        private void TestCollisions()
        {
            for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
            {
                bool result = IsCollisionTestColliding(i);
                collisionResults[i] = result;
            }
        }

        private void UpdateAngleBasedOnCollisions(double newAngle, int yIndex, int xIndex)
        {
            
            if(collisionResults[yIndex] && collisionResults[xIndex])
            {
                state = state.Fail();
                return;
            }
            else if (collisionResults[yIndex])
            {
                monster.TempAngle = xIndex == Globals.LEFT ? Math.PI : 0.0;

            }
            else if (collisionResults[xIndex])
            {
                monster.TempAngle = yIndex == Globals.BOTTOM ? Math.PI/2 : 3*Math.PI/2;

            }
        }
        private void UpdateAngleBasedOnCollisions()
        {
            monster.TempAngle = null;
            double angle = monster.Angle;
            while(angle < 0)
            {
                angle += 2 * Math.PI;
            }
            if(angle <= Math.PI/2)
            {
                UpdateAngleBasedOnCollisions(angle,Globals.BOTTOM, Globals.RIGHT);
            }
            else if (angle <= Math.PI)
            {
                UpdateAngleBasedOnCollisions(angle,Globals.BOTTOM, Globals.LEFT);
            }
           else if (angle <= 3*Math.PI / 2)
            {
                UpdateAngleBasedOnCollisions(angle,Globals.TOP, Globals.LEFT);
            }
            else
            {
                UpdateAngleBasedOnCollisions(angle, Globals.TOP, Globals.RIGHT);
            }

        }
        public override bool SetUpMove(double time)
        {
            if (started == false)
            {
                DefaultMove();
            }
            TestCollisions();
            this.state = state.Execute(time);
            UpdateAngleBasedOnCollisions();


            return true;
        }
    }

    public abstract class MovementState:Movement
    {
        public abstract MovementState Execute(double time);
        public abstract void Init();
        public abstract MovementState Fail();
        public override bool SetUpMove(double time)
        {
            return true;
        }
    }

    public class Wondering: MovementState
    {
        MovementState next;
        public double timeToNextState;
        BasicWonderWithHuntingMovement wonderingMovement;
        public double timeBetweenStates = 100;
        SmartMovement smartMovement;
            
        public Wondering(SmartMovement smartMovement, Monster monster, Player play, int wonderRange = 500, double angleOffset = Math.PI / 12.0, double senseRange = 500, double senseAngle = Math.PI / 2.0)
        {
            this.monster = monster;
            this.player = play;
            this.next = new MoveToExit(smartMovement, monster, player, this);

            timeToNextState = timeBetweenStates;
            wonderingMovement = new BasicWonderWithHuntingMovement(monster, play, wonderRange, angleOffset,senseRange,senseAngle);
            this.smartMovement = smartMovement;
               
        }
        public override void Init()
        {
            timeToNextState = timeBetweenStates;
            wonderingMovement.CreatePointWithoutChangingDirection(100,Math.PI/12);

        }
        public override MovementState Fail()
        {
            timeToNextState = timeBetweenStates;
            wonderingMovement.CreateBrandNewPoint();
            wonderingMovement.ResetPoint();
            return this;
        }
        public override MovementState Execute(double time)
        {
            if (timeToNextState == 0 && !wonderingMovement.Hunting)
            {
                next.Init();
                timeToNextState = timeBetweenStates;
                return next.Execute(time);
            }
            else
            {
                wonderingMovement.SetUpMove(time);
                timeToNextState--;
                return this;
            }
        }
}
    public class MoveToExit : MovementState
    {
        double safeDistence = 25;
        double currentDistance;
        int distanceCheckCounter; //used to keep track to see if the monster is moving toward the exit

        SmartMovement smartMovement;
        MovementState prev;
        MovementState next;
        int lastConnectionPoint;
        DrawnRoom currentRoom;
        int failToResetExit = 5;
        int failCounter;

        public DrawnRoom CurrentRoom { get => currentRoom; set => currentRoom = value; }
        public int ConnectionDirection { get => connectionDirection; set => connectionDirection = value; }
        public int LastConnectionPoint { get => lastConnectionPoint; set => lastConnectionPoint = value; }

        bool found;

        Point destination;
        private int connectionDirection; //which connectionPoint am I going to
        public MoveToExit(SmartMovement smartMovement, Monster monster, Player player, Wondering prev)
        {
            this.smartMovement = smartMovement;
            this.monster = monster;
            this.player = player;
            this.prev = prev;
            this.next = new ExitRoom(smartMovement, monster, player, this,prev);
            
            CurrentRoom = monster.Container;
            LastConnectionPoint = -1;
            found = false;
            failCounter = 0;

        }
        private bool CreateDestination()
        {
          
            int offset = Globals.Rand.Next(0, 5);
            for (int i = 0; i < Globals.CONNECTION_LIMIT; i++)
            {
                int index = (i + offset) % Globals.CONNECTION_LIMIT;
                if (index == LastConnectionPoint)
                {
                    continue;
                }
                Point connectionPoint = CurrentRoom.GetLocalConnectionPointCenter(index, true);
                if (connectionPoint == null)
                {
                    continue;
                }
                if (!base.DoesMonsterSeePoint(connectionPoint))
                {
                    continue;
                }
                smartMovement.SetMonsterAngle(-Point.GetAngleBetweenPoints(monster.Position, connectionPoint));
                currentDistance = monster.Position.GetDistance(connectionPoint);
                destination = connectionPoint;
                connectionDirection = index;
                failCounter = 0;
                return true;
            }
            failCounter++;
            if(failCounter  == failToResetExit)
            {
                lastConnectionPoint = -1;
                failCounter = 0;
            }
            return false;
        }

        private bool CheckToMoveToNextState()
        {
            double distance = monster.Position.GetDistance(destination);
            return distance < safeDistence;
        }

        private bool CheckForFail()
        {
            double distance = monster.Position.GetDistance(destination);
            if(destination != null && Globals.AreDoublesEqual(distance,currentDistance))
            {
                distanceCheckCounter++;
                if(distanceCheckCounter >= 5)
                {
                    return true;
                }
            }
            else
            {
                currentDistance = distance;
                distanceCheckCounter = 0;
            }
            return false;
        }
        public override MovementState Execute(double time)
        {
            //Should I set the destination to nearby exit?
            if (DoesMonsterSeePoint(player.Position))
            {
                return Fail();
            }
            if (!found)
            {
               
                bool foundExit = CreateDestination();
                found = foundExit;
               //did I successfully find an exit?
                if (!foundExit)
                {
                    prev.Init();
                    return prev.Execute(time);
                }
            }
            //is it time to move to the next phase.
            if (CheckToMoveToNextState())
            {
                next.Init();
                return next.Execute(time);
            }
            else if (CheckForFail())
            {
                return this.Fail();
            }
            return this;
        }

        public override MovementState Fail()
        {
            prev.Init();
            distanceCheckCounter = 0;
            lastConnectionPoint = -1;
            return prev;
        }
        public override void Init()
        {
            //need a better way to set the currentRoom
            CurrentRoom = monster.GetCurrentRoom();

            distanceCheckCounter = 0;
            destination = null;
            found = false;
        }

    }
    public class ExitRoom : MovementState
    {
        MoveToExit prev;
        Wondering next;
        SmartMovement smartMovement;
        Point destination;
        double safeDistance = 15; //once it is this distance from the exit, it assumes it is good to go.
        public ExitRoom(SmartMovement smartMovement, Monster monster, Player player, MoveToExit prev, Wondering next)
        {
            this.smartMovement = smartMovement;
            this.monster = monster;
            this.player = player;
            this.prev = prev;
            this.next = next;
        }


        public override void Init()
        {
            DrawnRoom currentRoom = prev.CurrentRoom;
            DrawnRoom nextRoom = currentRoom.GetConnection(prev.ConnectionDirection);

            destination = nextRoom.GetLocalConnectionPointCenter(Room.GetOppositeDirection(prev.ConnectionDirection),true);
            smartMovement.SetMonsterAngle(-Point.GetAngleBetweenPoints(monster.Position, destination));
        }

        public override MovementState Fail()
        {
            next.Init();
            return next;
        }
        public override MovementState Execute(double time)
        {
            if (DoesMonsterSeePoint(player.Position))
            {
                return Fail();
            }
            if(CalcuateDistanceFromPoint(destination) <= safeDistance)
            {
                next.Init();
                prev.CurrentRoom = prev.CurrentRoom.GetConnection(prev.ConnectionDirection);
                prev.LastConnectionPoint = Room.GetOppositeDirection(prev.ConnectionDirection);

                return next.Execute(time);
            }
            return this;
        }
    }
}


