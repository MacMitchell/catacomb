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
using Catacomb.CombatStuff;
using System.Windows.Controls;

namespace Catacomb
{
    public sealed class CatacombManager : Window
    {
        private static CatacombManager instance = null;
        public static CatacombManager Instance
        {
            get { return instance; }
            set { if(instance == null)
                {
                    instance = value;
                } }
        }



        Player player;
        //Window mainWindow;
        System.Windows.Forms.Timer time;
        DateTime currentTime;
        private bool updateFinish = true;



        //Main settings
        int numberOfRooms = 100; //the value set here is the base value
        int numberOfMonsters = 10; //the value set here is the base value

        DisplayMode display; 
        private DisplayMode Display
        {
            set {
                display = value;
                
                base.Content = display.GetDisplay();  }
            get { return display; }
        }        

        
        public CatacombManager() : base()
        {

            base.Activate();

            base.Height = Screen.PrimaryScreen.WorkingArea.Size.Height;
            base.Width = Screen.PrimaryScreen.WorkingArea.Size.Width;

            base.Title = "UwU";
            base.WindowState = (WindowState)FormWindowState.Maximized;
   
            base.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
            time = new System.Windows.Forms.Timer();

            time.Interval = 10;
            time.Enabled = true;
            time.Tick += Update;
            currentTime = DateTime.Now;

            this.KeyDown += MoveKeyPress;
            this.KeyUp += MoveKeyRelease;
            Instance = this;
            this.Show();
        }

        public void StartUp()
        {

            player = new Player(new Vectors.Point(0, 0));

            ExploreCanvas maze = new ExploreCanvas(base.ActualWidth, base.ActualHeight, player);
            maze.SetUpMaze(numberOfRooms,numberOfMonsters);

            Display = maze;
        }

        void Update(object sender, EventArgs e)
        {
            if (!updateFinish)
            {
                return;
            }
            DateTime temp = DateTime.Now;
            double dif = (temp - currentTime).TotalSeconds;

            currentTime = DateTime.Now;
            Display = display.Update(dif);

            updateFinish = true;
        }

        void MoveKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (display != null)
            {
                display.KeyPress(e.Key);
            }
            if(e.Key == Key.Space)
            {
                //NextFloor();
            }
        }

        void MoveKeyRelease(object sender, System.Windows.Input.KeyEventArgs e)
        {
           if (display != null)
            {
                display.KeyRelease(e.Key);
            }
        }

        public void NextFloor()
        {
            Display.Destroy();
            
            ExploreCanvas maze = new ExploreCanvas(base.ActualWidth, base.ActualHeight, player);
            maze.SetUpMaze(numberOfRooms, numberOfMonsters);

            Display = maze;
        }

        void ScaleLevel()
        {
            numberOfRooms += 10;
        }

    }
    public delegate DisplayMode SwitchDisplayMode();
    public interface DisplayMode
    {
        SwitchDisplayMode Finished { get; set; }
        System.Windows.Controls.Panel GetDisplay();
        DisplayMode Update(double time);
        void KeyPress(Key Keyin);
        void KeyRelease(Key keyIn);
        void Destroy();
    }



   
}
