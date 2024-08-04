using Catacomb.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;
using Catacomb.Entities;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Input;
namespace Catacomb.Maze
{
    public class CatMaze
    {

        private Room start;
        private Canvas canvas;
        private Random rand;
        private Player player;
        public Point startPoint;

        private List<Room> allRooms;        
        private MazeBuilder builder;
        protected List<Monster> monsters;

        private int size;
        private int step;
        private int numberOfMonsters;
        public Room Start{
            get{return start;}
            set{start=  value;}
        }
        private Point cameraPos;
        public Point CameraPos
        {
            get { return cameraPos; }
            set { cameraPos = value; }
        }

        public int Size { get => size; set => size = value; }
        public int Step { get => step; set => step = value; }
        public int NumberOfMonsters { get => numberOfMonsters; set => numberOfMonsters = value; }

        //TODO currently the maze uses the MazeBuilder to help create itself
        //Ideally, when the user creates a maze, it uses a MazeBuilder to create a maze
        public CatMaze() {
            rand = new Random();
            builder = new MazeBuilder();
            monsters = new List<Monster>();
            startPoint = new Point(150, 150);

            Step = 1;
            Size = 10;
            NumberOfMonsters = 10;
        }
        public void Create(Player playIn)
        {
            
            start = builder.BuildMaze(Size, Step);
            canvas = null;
            player = playIn;
            player.Position = startPoint;//new Point(150, 150);


            Draw();
        }


        public void ConstructMaze(Player playIn)
        {
            bool done = false;
            while (!done)
            {
                try
                {
                    this.Create(playIn);
                    this.Draw();

                    for (int i = 0; i < NumberOfMonsters; i++)
                    {
                        this.CreateMonster();
                    }
                    done = true;
                }
                catch (Exception sadness)
                {
                    Console.WriteLine("FAILED TO BUILD MAZE\n");
                }
            }
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }
        public void Draw()
        {
            if(canvas != null)
            {
                return;
            }
            canvas = new Canvas();
            canvas.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
            canvas.Width = 1000;
            canvas.Height = 1000;

            cameraPos = new Point(0, 0);

            Point p1 = new Point(200, 200);
            Point p2 = new Point(350, 350);
            //start.Create(p1, p2);
            //start.Draw( );
            //canvas.Children.Add(start.GetCanvas());
            List<Room> createdRooms = new List<Room>();
            //createdRooms.Add(start);
            MazeBuilder builder = new MazeBuilder();

            
            allRooms = builder.BuildRooms(start,canvas,startPoint.AddPoint(new Point(-50,-50)));
            SetUpPlayer();
        }

        private void SetUpPlayer()
        {
            player.Draw();
            player.Container = Start.RoomDrawn;            
        }

        public void move(double time)
        {
            double posNowX = player.Position.X;
            double posNowY = player.Position.Y;
            player.Move(time);

            Point posAfter = player.Position;
            Point differentPoint = new Point((posAfter.X - posNowX)*-1,( posAfter.Y - posNowY)*-1);
            CameraPos = CameraPos.AddPoint(differentPoint);
            Canvas.SetLeft(canvas,CameraPos.X);
            Canvas.SetTop(canvas,CameraPos.Y);

            foreach(Monster m in monsters)
            {
                m.Move(time);
            }
        }


        public Monster CheckForCombat()
        {
            Room playerRoom = player.Container.parent;
            List<Room> roomsToCheck = new List<Room>();
            roomsToCheck.Add(playerRoom);
            for(int i =0;i< Global.Globals.CONNECTION_LIMIT; i++)
            {
                if (playerRoom.HasConnection(i))
                {
                    roomsToCheck.Add(playerRoom.GetConnectedRoom(i));
                }
            }
            foreach(Monster m in monsters)
            {
                Monster temp = m.DoesCollideWithPlayer(roomsToCheck, player);
                if(temp != null)
                {
                    return m;
                }
            }
            return null;

        }
        public virtual void CreateMonster() 
        {
            //find a room
            Room monsterRoom = GetMonsterRoom();

            Monster createdMonster = CreateMonster(monsterRoom);
            createdMonster.MovementAI = new BasicMovement(createdMonster, player);
            createdMonster.Container = monsterRoom.RoomDrawn;

            Point spawnPoint = monsterRoom.RoomDrawn.GenerateSpawnPoint(createdMonster.Width, createdMonster.Height);
            if(spawnPoint == null)
            {
                return;
            }
            createdMonster.PlaceMonster(spawnPoint);

            AddMonster(createdMonster);
        }

        protected void AddMonster(Monster monsterIn)
        {
            canvas.Children.Add(monsterIn.GetCanvas());
            monsters.Add(monsterIn);

        }
        /**
         *Returns a room to create a monster in. Needs the maze to be built first 
         */
        protected virtual Room GetMonsterRoom()
        {
            List<Room> dontSpawnHere = GetInspawnableRooms();
            int index;
            
            while (true)
            {
                index = rand.Next(0, allRooms.Count);
                Room monsterRoom = allRooms[index];
                if (!dontSpawnHere.Contains(monsterRoom))
                {
                    break;
                }
            }
            return allRooms[index];
        }
        /**
         * Returns a list of rooms that you do not want ANY monster to spawn in
         * This base method returns the start room and all rooms connecting to the start room
         */
        protected virtual List<Room> GetInspawnableRooms()
        {
            return start.GetAllConnectedRooms(true);
            
        }
        protected virtual Monster CreateMonster(Room roomin)
        {
            double minSpeed = 100.0;
            double maxSpeed = 500.0;
            double speed = rand.NextDouble() * maxSpeed + minSpeed;
            Monster newMonster = new Monster(35, 35, speed);
            return newMonster;
        }

        public void RemoveMonster(Monster deadMonster)
        {
            for(int i =0; i < monsters.Count; i++)
            {
                if (monsters[i].equals(deadMonster))
                {
                    canvas.Children.Remove(monsters[i].GetCanvas());
                    monsters.RemoveAt(i);
                    break;
                }
            }
        }
        public  void MoveKeyPress(object sender, KeyEventArgs e)
        {
            player.KeyPress(e.Key);
        }


    }
}
