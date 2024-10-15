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
using Catacomb.CombatStuff;
namespace Catacomb.Maze
{
    public class CatMaze
    {

        /**8
         *WHEN BUILDING A MAZE YOU MUST DECLARE:
         *  Size: total number of rooms
         *  Step: Number of filler rooms between key rooms
         *  NumberOfMonster: The number of monsters
         *  FillerRoom function to create filler rooms
         *  KeyRoom:  function to create key rooom
         *  StairRoom: function to create ONE stair room. If you want more, add them to key rooms
         *  
         */

        private Room start;
        private Canvas canvas;
        private Random rand;
        private Player player;
        public Point startPoint;

        private List<Room> allRooms;        
        private MazeBuilder builder;
        protected List<Monster> monsters; //thesea are the monsters currently roaming in the maze

        public MazeBuilder.CreateRoomFunction FillerRoom { get => Builder.FillerRoom; set => Builder.FillerRoom = value; }
        public MazeBuilder.CreateRoomFunction KeyRoom { get => Builder.KeyRoom; set => Builder.KeyRoom = value; }
        public MazeBuilder.CreateRoomFunction StairRoom { get => Builder.StairRoomCreator; set => Builder.StairRoomCreator = value; }

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

        private List<Monster> creatableMonsters; //the monsters can be added to maze. It is a Unique list


        public int Size { get => size; set => size = value; }
        public int Step { get => step; set => step = value; }
        public int NumberOfMonsters { get => numberOfMonsters; set => numberOfMonsters = value; }
        public List<Monster> CreatableMonsters { get => creatableMonsters; set => creatableMonsters = value; }
        public List<Room> InspawnableRooms { get => inspawnableRooms; set => inspawnableRooms = value; }
        public MazeBuilder Builder { get => builder; set => builder = value; }

        private List<Room> inspawnableRooms;

        //TODO currently the maze uses the MazeBuilder to help create itself
        //Ideally, when the user creates a maze, it uses a MazeBuilder to create a maze
        public CatMaze() {
            rand = new Random();
            Builder = new MazeBuilder();
            monsters = new List<Monster>();
            startPoint = new Point(150, 150);

            Step = 1;
            Size = 10;
            NumberOfMonsters = 10;
            CreatableMonsters = new List<Monster>();

            InspawnableRooms = new List<Room>();
            
        }
        public void Create(Player playIn)
        {
            
            start = Builder.BuildMaze(Size, Step);
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
                    CleanUp();
                    this.Create(playIn);
                    this.Draw();
                    AddSpawnAreaToInspawnableRooms();

                    allRooms.ForEach(room =>
                    {
                        room.RoomDrawn.MustExecute(this);
                    });

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

        public void CleanUp()
        {
            inspawnableRooms = new List<Room>();
            Builder.CleanUp();
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
            bool done = false;
            //TODO add the room that cannot spawn any monster to list of inspawnableRooms
            while (!done)
            {
                Room monsterRoom = GetMonsterRoom();
                List<Monster> possibleMonsters = monsterRoom.AcceptableMonsters(creatableMonsters);
                if(possibleMonsters.Count == 0)
                {
                    InspawnableRooms.Add(monsterRoom);
                    continue;
                }

                Monster createdMonster = possibleMonsters[rand.Next(0, possibleMonsters.Count)].Clone(player);
                
                Point spawnPoint = monsterRoom.RoomDrawn.GenerateSpawnPoint(createdMonster.Width, createdMonster.Height);
                if (spawnPoint != null)
                {
                    createdMonster.PlaceMonster(spawnPoint);
                    createdMonster.Container = monsterRoom.RoomDrawn;
                    AddMonster(createdMonster);
                    
                    done = true;
                }
            }
            
        }

        protected void AddSpawnAreaToInspawnableRooms()
        {
            InspawnableRooms = start.GetAllConnectedRooms(true);
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
            int index;
            
            while (true)
            {
                index = rand.Next(0, allRooms.Count);
                Room monsterRoom = allRooms[index];
                if (!InspawnableRooms.Contains(monsterRoom))
                {
                    break;
                }
            }
            return allRooms[index];
        }
        protected virtual Monster CreateMonster(Room roomIn)
        {
            double minSpeed = 100.0;
            double maxSpeed = 500.0;
            double speed = rand.NextDouble() * maxSpeed + minSpeed;
            //Monster newMonster = new Monster(35, 35, speed);
            //newMonster.MovementAI = new BasicMovement(newMonster, player);
            Monster newMonster = roomIn.AcceptableMonsters(CreatableMonsters)[0].Clone(player);
            newMonster.Container = roomIn.RoomDrawn;
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
