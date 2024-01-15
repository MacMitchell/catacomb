using Catacomb.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catacomb.Vectors;
using Catacomb.Entities;
using System.Windows.Input;
using System.Collections;

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

        //TODO currently the maze uses the MazeBuilder to help create itself
        //Ideally, when the user creates a maze, it uses a MazeBuilder to create a maze
        public CatMaze() {
            rand = new Random();
            builder = new MazeBuilder();
            monsters = new List<Monster>();
            startPoint = new Point(150, 150);
        }
        public void Create(int size, int step, Player playIn)
        {
            
            start = builder.BuildMaze(size, step);
            canvas = null;
            player = playIn;
            player.Position = startPoint;//new Point(150, 150);


            Draw();
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
            canvas.Background = Global.Globals.BACKGROUND_COLOR;
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
            int index = rand.Next(0, allRooms.Count);
            return allRooms[index];
        }

        protected virtual Monster CreateMonster(Room roomin)
        {
            Monster newMonster = new Monster(50, 50, 100);
            return newMonster;
        }
        public  void MoveKeyPress(object sender, KeyEventArgs e)
        {
            player.KeyPress(e.Key);
        }


    }
}
