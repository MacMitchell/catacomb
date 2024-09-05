using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Maze;
using Catacomb.Vectors;
namespace Catacomb.Entities
{
    public abstract class Movement
    {
        protected Monster monster =  null;
        protected Player player = null;
        protected static int calculateTime = 100;
        public abstract bool SetUpMove(double time);
        /**
        * Not a very efficient way to calculate the angle 
        */
        protected double CalculateAngleFromPlayer()
        {
            double angle = Point.GetAngleBetweenPoints(monster.Position, player.Position);
            return -angle;
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
                bool playerSeen = monster.Container.EntityMove(monster, distance- monster.Width);
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
            if(movementCounter >0)
            {
                
                if(movementPoint.GetDistance(monster.Position) < 25)
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
        protected  Point destination;
        bool pointReady = false;
        double angleOffset;
        public BasicWonderingMovement(Monster parentIn, Player playIn, int wonderRange = 500, double angleOffset= Math.PI/12.0)
        {
            monster = parentIn;
            player = playIn;
            this.wonderRange = wonderRange;
            this.angleOffset = angleOffset;
        }

        private bool hitPoint()
        {
            double minDistance = 50;//100; //100 works great testing lower values
            double distance = destination.GetDistance(monster.Center);
            return minDistance > Math.Abs(distance);
        }
        private void createBrandNewPoint()
        {
            int xDistance = Global.Globals.Rand.Next(-wonderRange, wonderRange);
            int yDistance = Global.Globals.Rand.Next(-wonderRange, wonderRange);
            destination = monster.Center.AddPoint(new Point(xDistance, yDistance));
            monster.Angle = -Point.GetAngleBetweenPoints(monster.Center, destination);
            pointReady = true;
        }

        private void adjustPoint()
        {
            double newAngle = Global.Globals.GetRandomNumber(monster.Angle - angleOffset, monster.Angle + angleOffset);
            double distance = Global.Globals.Rand.Next(50, wonderRange);
            monster.Angle = newAngle;
            destination = destination.AddPoint(new Point(distance * Math.Cos(newAngle), distance * Math.Sin(newAngle)));
        }
        public override bool SetUpMove(double time)
        {
            if(pointReady && !monster.Container.EntityMove(monster,time * monster.Velocity))
            {
                pointReady = false;
            }
            else if(pointReady && hitPoint())
            {
                adjustPoint();
            }
            if (!pointReady)
            {
                createBrandNewPoint();
            }
            return true;
        }
    }
    public class BasicWonderWithHuntingMovement: BasicWonderingMovement
    {
        protected double senseRange;
        protected double calculateTimer;
        protected double senseAngle;
        protected double onTheHuntSenseBonus = 4.0;
        protected bool playerFound;
        public BasicWonderWithHuntingMovement(Monster parentIn, Player playIn, int wonderRange = 500, double angleOffset = Math.PI / 12.0, double senseRange = 500, double senseAngle = Math.PI/2.0) : base(parentIn, playIn,wonderRange, angleOffset)
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
            if(Math.Abs(angle - monster.Angle) > senseAngle)
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
            else if(calculateTimer == 0) {
                if (playerFound)
                {
                    senseAngle /= onTheHuntSenseBonus;
                }
                playerFound = false;
            }
            if(calculateTimer == 0)
            {
                calculateTimer = calculateTime;
            }
            
            return base.SetUpMove(time);
            
            
        }
    }

}
