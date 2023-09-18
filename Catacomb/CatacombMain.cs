using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using Catacomb.Maze;
namespace Catacomb
{
    class CatacombMain : Window
    {
        bool up;
        bool left;
        bool right;
        bool down;
        CatMaze currentMaze;
        //Window mainWindow;
        System.Windows.Forms.Timer time;
        DateTime currentTime;
        public CatacombMain() : base()
        {

            base.Activate();

            base.Height = Screen.PrimaryScreen.WorkingArea.Size.Height;
            base.Width = Screen.PrimaryScreen.WorkingArea.Size.Width;
            base.Title = "UwU";
            up = false;
            down = false;
            left = false;
            right = false;

            base.WindowState = (WindowState)FormWindowState.Maximized;
            base.Background = Global.Globals.BACKGROUND_COLOR;
            time = new System.Windows.Forms.Timer();

            time.Interval = 10;
            time.Enabled = true;
            time.Tick += Update;
            currentTime = DateTime.Now;

            this.KeyDown += MoveKeyPress;
            this.Show();
        }

        public void StartUp()
        {
            SetUpMaze();
        }

        void SetUpMaze()
        {
            currentMaze = new CatMaze(25, 1);
            currentMaze.Draw();
            base.Content = currentMaze.GetCanvas();
        }

        void Update(object sender, EventArgs e)
        {
            DateTime temp = DateTime.Now;
            double dif = (temp - currentTime).TotalSeconds;
            currentTime = DateTime.Now;
            //Console.WriteLine("TIME: " + dif);
            //Console.WriteLine("HELLO THERE");
        }
        void MoveKeyPress(object sender, System.Windows.Input.KeyEventArgs e ) 
        {
                   
        }

    }
}
