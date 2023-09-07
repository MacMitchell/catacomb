using Catacomb.Visuals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;

namespace Catacomb.Maze
{
    
    public class MazeBuilder
    {
        private static Random rand;

        public MazeBuilder()
        {
            rand = new Random();
               
        }

        public Room BuildMaze(int size, int step)
        {
            ArrayList found = new ArrayList();
            found.Add(new Room());

            while (found.Count < size)
            {

                int index = rand.Next(found.Count);
                Room current = (Room) found[index];
                int[] placement = GetVertAndHoriDistance(1, step);
                int direction = rand.Next(4);
                for(int i = 0; i < placement[0]; i++)
                {
                    if (!current.HasConnection(direction))
                    {                    
                        //TODO: make another function to make the rooms

                        Room newRoom = i == placement[0]-1?  new Room() : new Hallway();
                        found.Add(newRoom);
                        current.connect(newRoom,direction);
                    }
                    current = current.GetConnectedRoom(direction);
                }

                direction = direction % 2 == 0 ? 2 * rand.Next(2) + 1 : 2 * rand.Next(2);
                for (int i = 0; i < placement[1]; i++)
                {
                    if (!current.HasConnection(direction))
                    {
                        //TODO: make another function to make the rooms
                        Room newRoom = i == placement[1] - 1 ? new Room() : new Hallway();
                        found.Add(newRoom);
                        current.connect(newRoom, direction);
                    }
                    current = current.GetConnectedRoom(direction);
                }


            }
            return ((Room)found[0]);
        }


        
        private int[] GetVertAndHoriDistance(int min, int max)
        {
            int[] direction = new int[2];
            int index = rand.Next(2);

            int move = random(min, max);
            direction[index] = move;
            
            index = index == 0 ? index = 1 : index = 0;
            int move2 = random(min, max - Math.Abs(move));

            direction[index] = move2;
            return direction;

        }

 /** Create a random number that will be equal or less than max. It will be equal or greater than the min
  *     The number has an equal chance to be negative or positive*/
        private int random(int min, int max)
        {
            int sign = (2 * (rand.Next(2) + 1)) - 3;
            int number = rand.Next(max) + min;
            int result = max >= min ? number * sign : 0;
            return result;
        }
    }
}
