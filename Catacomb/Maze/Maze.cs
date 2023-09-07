using Catacomb.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Catacomb.Vectors;

namespace Catacomb.Maze
{
    public class Maze
    {

        private Room start;
        private Canvas canvas;
        
        public Room Start{
            get{return start;}
            set{start=  value;}
        }

        public Maze(int size, int step)
        {
            MazeBuilder builder = new MazeBuilder();
            start = builder.BuildMaze(size, step);
            canvas = null;
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
            canvas.Width = 1000;
            canvas.Height = 1000;
            start.Draw(new Point(0,0));
            canvas.Children.Add(start.GetCanvas());
            for(int i =0; i < 4; i++)
            {
                Tuple<Point,Point> position = start.RoomDrawn.PlaceNeighbor(i, 20, new Point(0,0), new Point(0,0));
                start.GetConnectedRoom(i).Draw(position.Item1, position.Item2);
                canvas.Children.Add(start.GetConnectedRoom(i).GetCanvas());
            }
        }

        
    }
}
