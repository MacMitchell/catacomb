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
        int mode = 0;


        Combat currentCombat;
        Monster currentCombatMonster = null;
        System.Windows.Controls.Panel currentInControl;
        private System.Windows.Controls.Panel CurrentInControl
        {
            set { currentInControl = value;
                base.Content = value;  }
            get { return currentInControl; }
        }

        public CatacombMain() : base()
        {

            base.Activate();
            mainCanvas = new Canvas();

            base.Height = Screen.PrimaryScreen.WorkingArea.Size.Height;
            base.Width = Screen.PrimaryScreen.WorkingArea.Size.Width;

            base.Title = "UwU";
            mainCanvas.Width = base.Width;
            mainCanvas.Height = base.Height;
            up = 0;
            down = 0;
            left = 0;
            right = 0;

            base.WindowState = (WindowState)FormWindowState.Maximized;
            base.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
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
            int numberOfRooms = 100;
            int stepSize = 5;
            double width = base.ActualWidth;
            double height = base.ActualHeight;

            //currentMaze = new CatMaze(25, 1,player);
            
                currentMaze = new CatMaze();

                currentMaze.startPoint = new Vectors.Point((width / 2), (height / 2));

                currentMaze.Create(numberOfRooms, stepSize, player);
                currentMaze.Draw();
                
            for(int i =0; i < 100; i++)
            {
                currentMaze.CreateMonster();
            }
            CurrentInControl = mainCanvas;
            //base.Content = currentInControl;
            
            mainCanvas.Children.Add(currentMaze.GetCanvas());
            //base.Content = currentMaze.GetCanvas();
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
            if (mode == 0)
            {
                updateFinish = false;
                
                SetPlayerAngle();
                currentMaze.move(dif);
                Monster combat = currentMaze.CheckForCombat();
                if (combat != null)
                {
                    SetUpCombat(player, combat);
                    mode = 1;
                }
            }
            if(mode == 1)
            {
                    if(currentCombat.ReportStatus == Command.MONSTER_DIED || currentCombat.ReportStatus == Command.PLAYER_DIED)
                    {
                        mode = 0;
                        RemoveMonster(currentCombatMonster);
                        PlaceBackMaze();
                    }
            }

            updateFinish = true;
        }


        void RemoveMonster(Monster deadMonster)
        {
            currentMaze.RemoveMonster(deadMonster);
        }

        void PlaceBackMaze()
        {
            CurrentInControl = mainCanvas;
            //base.Content = currentInControl;
        }
        void SetUpCombat(Player playIn, Monster monsterIn)
        {
            currentCombat = new Combat(base.ActualWidth,base.ActualHeight,playIn.Fighter, monsterIn.Fighter);
            currentCombatMonster = monsterIn;

            CurrentInControl = currentCombat;
            //base.Content = currentInControl;
           

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
            var localEvent = Keyboard.KeyDownEvent;

            currentInControl.RaiseEvent(new System.Windows.Input.KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(this), 0, e.Key) { RoutedEvent = localEvent }); ;
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
