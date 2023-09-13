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

namespace Catacomb.Maze
{
    public class CatMaze
    {

        private Room start;
        private Canvas canvas;
        private Random rand;
        private Player player;
        public Room Start{
            get{return start;}
            set{start=  value;}
        }

        public CatMaze(int size, int step)
        {
            rand = new Random();
            MazeBuilder builder = new MazeBuilder();
            start = builder.BuildMaze(size, step);
            canvas = null;
            player = new Player(new Point(200, 200));
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
            Point p1 = new Point(200, 200);
            Point p2 = new Point(350, 350);
            start.Draw(p1,p2 );
            canvas.Children.Add(start.GetCanvas());
            
            for(int i =0; i < 4; i++)
            {
                if (start.HasConnection(i))
                {
                    double length =  rand.NextDouble() * 1000+100;
                    double width = rand.NextDouble() * 1000 + 100;
                    Point random = new Point(length, width);
                    Tuple<Point, Point> position = start.RoomDrawn.PlaceNeighbor(i, 25, p1, p2.AddPoint(random));
                    start.GetConnectedRoom(i).Draw(position.Item1, position.Item2);
                    canvas.Children.Add(start.GetConnectedRoom(i).GetCanvas());
                }
            }
            SetUpPlayer();
        }

        private void SetUpPlayer()
        {
            player.Draw();
            player.Container = Start.RoomDrawn;
            canvas.Children.Add(player.GetCanvas());
        }

        public void move(double time)
        {
            canvas.Children.Remove(player.GetCanvas());
            player.Move(time);
            canvas.Children.Add(player.GetCanvas());
        }

        public  void MoveKeyPress(object sender, KeyEventArgs e)
        {
            player.KeyPress(e.Key);
        }

    }
}
