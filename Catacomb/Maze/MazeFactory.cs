using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Global;
using Catacomb.CombatStuff;
using Catacomb.Entities;
using Catacomb.Visuals;
using Catacomb.CombatStuff.AttackFactories;
using Catacomb.Maze.Rooms;
using Catacomb.CombatStuff.Class;

namespace Catacomb.Maze
{
    public class MazeFactory
    {
        public static int noRoomLevel = 1; //if the currentRooms are this, that means there are no constructed rooms
        public static MazeBuilder.CreateRoomFunction CreateAllRoomFunction(Room[] rooms,int[] roomCount, MazeBuilder builder, bool filler = false)
        {
            List<Room> allRooms = new List<Room>();
            int size = 0;
            for(int i = 0; i < rooms.Count(); i++) 
            {
                for(int  j=0; j< roomCount[i]; j++)
                {
                    allRooms.Add(rooms[i].Clone());
                    size++;
                }
            }
            List<Room> tempList = new List<Room>();
            MazeBuilder.CreateRoomFunction functionReturn = (List<Room> currentRooms) =>
           {
               if ((filler ? builder.fillerRoomCount : builder.keyRoomCount) == 0) {
                   //if no rooms created create
                   tempList = allRooms.ToList<Room>();
               }
               int index = Globals.Rand.Next(tempList.Count);
               Room toReturn = tempList[index];
               tempList.RemoveAt(index);
               return toReturn;
           };
            return functionReturn;
        }

        public static CatMaze BasicMaze(Player playIn)
        {
            CatMaze maze = new CatMaze();
            maze.Size = 6;
            maze.Step = 1;
            maze.NumberOfMonsters = 0;//3;

            maze.CreatableMonsters.Add(MonsterFactory.GreenSlime(playIn));
            maze.CreatableMonsters.Add(MonsterFactory.GoblinScout(playIn));

            Room[] fillerRooms = { new Hallway() };
            int[] fillerCount = { 20 };
            maze.FillerRoom = CreateAllRoomFunction(fillerRooms, fillerCount, maze.Builder, true);

            RandomTreasureRoom treasureRoom = new RandomTreasureRoom(Treasure.CreateBasicAttackTreasure(TurnBasedAttackFactory.SharpStick));
            
            Room[] keyRooms = { new Room(), treasureRoom };

            int[] keyCount = { 5,1 };
            maze.KeyRoom = CreateAllRoomFunction(keyRooms, keyCount, maze.Builder, false);
            maze.StairRoom = (List<Room> rooms) => new StairRoom(BasicFireMaze(playIn));


            return maze;
        }
        public static CatMaze BasicFireMaze(Player playIn)
        {
            CatMaze maze = new CatMaze();
            maze.Size = 25;
            maze.Step = 1;
            maze.NumberOfMonsters = 0;//8;

            maze.CreatableMonsters.Add(MonsterFactory.FireImp(playIn));
            maze.CreatableMonsters.Add(MonsterFactory.LittleDevil(playIn));
            maze.CreatableMonsters.Add(MonsterFactory.FireWisp(playIn));
            Room[] fillerRooms = { new Hallway() };
            int[] fillerCount = { 20 };
            maze.FillerRoom = CreateAllRoomFunction(fillerRooms, fillerCount, maze.Builder, true);

            TreasureRoom treasureRoom = new TreasureRoom(Treasure.CreateBasicAttackTreasure(TurnBasedAttackFactory.ToxicAura));
            BossRoom bossRoom = new BossRoom(MonsterFactory.FireElemental(playIn), CatClassFactory.InfernoMage(playIn.GetPlayerFighter), Treasure.CreateAttackDecTreasure(AttackDecFactory.CoolFlames));
            Room[] keyRooms = { new Room(), treasureRoom,bossRoom };

            int[] keyCount = { 13, 1,4 };
            maze.KeyRoom = CreateAllRoomFunction(keyRooms, keyCount, maze.Builder, false);
            maze.StairRoom = (List<Room> rooms) => new StairRoom(BasicMaze(playIn));


            return maze;
        }
    }
}
