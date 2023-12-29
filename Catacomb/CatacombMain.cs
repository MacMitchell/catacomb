using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Catacomb.Vectors;

using Catacomb.Maze;
using Catacomb.Entities;
using System.Windows.Controls;

namespace Catacomb
{
    class CatacombMain : Window
    {
        int up;
        int left;
        int right;
        int down;
        Player player;
        CatMaze currentMaze;
        //Window mainWindow;
        System.Windows.Forms.Timer time;
        DateTime currentTime;
        private bool updateFinish = true;
        Canvas mainCanvas;

        public CatacombMain() : base()
        {

            base.Activate();
            mainCanvas = new Canvas();

            base.Height = Screen.PrimaryScreen.WorkingArea.Size.Height;
            base.Width = Screen.PrimaryScreen.WorkingArea.Size.Width;

            base.Title = "UwU";
            mainCanvas.Width = base.Width;
            mainCanvas.Height = base.Height;
            Console.WriteLine("width: " + base.Width + " Height: " + base.Height);
            up = 0;
            down = 0;
            left = 0;
            right = 0;

            base.WindowState = (WindowState)FormWindowState.Maximized;
            base.Background = Global.Globals.BACKGROUND_COLOR;
            time = new System.Windows.Forms.Timer();

            time.Interval = 10;
            time.Enabled = true;
            time.Tick += Update;
            currentTime = DateTime.Now;

            this.KeyDown += MoveKeyPress;
            this.KeyUp += MoveKeyRelease;
            this.Show();
        }

        public void StartUp()
        {

            player = new Player(new Vectors.Point(0, 0));
            mainCanvas.Children.Remove(player.GetCanvas());

            SetUpMaze();
            mainCanvas.Children.Add(player.GetCanvas());

        }

        void SetUpMaze()
        {
            int numberOfRooms = 25;
            int stepSize = 2;
            double width = base.ActualWidth;
            double height = base.ActualHeight;

            //currentMaze = new CatMaze(25, 1,player);
            currentMaze = new CatMaze();

            currentMaze.startPoint = new Vectors.Point((width / 2), (height /2));

            currentMaze.Create(numberOfRooms, stepSize, player);
            currentMaze.Draw();
            base.Content = mainCanvas;
            mainCanvas.Children.Add(currentMaze.GetCanvas());
            //base.Content = currentMaze.GetCanvas();
        }

        void Update(object sender, EventArgs e)
        {
            if (!updateFinish)
            {
                return;
            }
            updateFinish = false;
            DateTime temp = DateTime.Now;
            double dif = (temp - currentTime).TotalSeconds;
            
            currentTime = DateTime.Now;
            SetPlayerAngle();
            currentMaze.move(dif);
            
            
            updateFinish = true;
        }

        void SetPlayerAngle()
        {
            int divider = right + down + left + up;
            if (divider == 0 || (divider == 2 && 2 == up + down) || (divider ==2 && 2 == left + right))
            {
                player.Velocity = 0;
                return;
            }
            else {
                player.Velocity = player.MaxVelocity;
            }
            
            int x = 0 + 5 * right - 5 * left;
            int y = 0 + 5 * down - 5 * up;
            int deltaX = x -0;
            int deltaY = y -0;
            double angle = Math.Atan2(deltaY, deltaX);

            player.Angle = angle;
        }
        void MoveKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
            switch (e.Key) {
                case Key.D:
                    right = 1;
                    break;
                case Key.S:
                    down = 1;
                    break;
                case Key.A:
                    left = 1;
                    break;
                case Key.W:
                    up = 1;
                    break;
            }
            
        }

        void MoveKeyRelease(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D:
                    right = 0;
                    break;
                case Key.S:
                    down = 0;
                    break;
                case Key.A:
                    left = 0;
                    break;
                case Key.W:
                    up = 0;
                    break;
            }
        }

    }
}
