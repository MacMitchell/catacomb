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

        public abstract bool SetUpMove(double time);
        
    }


    public class BasicMovement : Movement 
    {
        protected Point movementPoint;
        protected double senseRange;
        private int movementCounter;
        public BasicMovement(Monster parentIn, Player playIn, double senseRangeIn = 800)
        {
            movementCounter = 0;
            monster = parentIn;
            player = playIn;
            senseRange = senseRangeIn;
        }
        private bool doesMonsterSeePlayer()
        {
            double distance = player.Position.GetDistance(monster.Position);
            if (distance < senseRange)
            {
                double angle = CalculateAngleFromPlayer();
                //if it can move the total distance then it can 'see' the player
                double oldAngle = monster.Angle;
                monster.Angle = angle;
                bool playerSeen = monster.Container.EntityMove(monster, distance);
                monster.Angle = oldAngle;
                return playerSeen;
            }
            return false;
        }


        /**
         * Not a very efficient way to calculate the angle 
         */
        private double CalculateAngleFromPlayer()
        {
            double angle = Point.GetAngleBetweenPoints(monster.Position, player.Position);
            return -angle;
            

        }
        public override bool SetUpMove(double time)
        {
            if (doesMonsterSeePlayer())
            {
                monster.Angle = CalculateAngleFromPlayer();
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

}
