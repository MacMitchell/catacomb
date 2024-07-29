using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using Catacomb.Entities;
using Catacomb.CombatStuff;
namespace Catacomb.Maze
{
    class ExploreCanvas : Canvas, DisplayMode
    {

        private Player player;
        private CatMaze maze;

        public SwitchDisplayMode Finished
        {
            get { return null; }
            set { return; }
        }
        //list of keys pressed
        int left;
        int right;
        int up;
        int down;
        public ExploreCanvas(double width, double height, Player player):base()
        {
            base.Width = width;
            base.Height = height;
            base.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
            this.player = player;

        }
        
        public Panel GetDisplay(){
            return this;
        }
        public void SetUpMaze(int numberOfRooms = 100, int numberOfMonsters = 100 )
        {
            int stepSize = numberOfRooms/10;
            double width = base.Width;
            double height = base.Height;
            //currentMaze = new CatMaze(25, 1,player);
            Boolean done = false;
            //TEMPORARY MEASURE. Building the maze sometimes fails. It seems like it just fails to place one room, sometimes. One room will take all the available parents room and still fail
            //Got tired of looking through the maze builder and building rooms
            while (!done)
            {
                try
                {
                    maze = new CatMaze();

                    maze.startPoint = new Vectors.Point((width / 2), (height / 2));

                    maze.Create(numberOfRooms, stepSize, player);
                    maze.Draw();

                    for (int i = 0; i < numberOfMonsters; i++)
                    {
                        maze.CreateMonster();
                    }
                    done = true;
                }
                catch (Exception sadness)
                {
                    Console.WriteLine("FAILED TO BUILD MAZE\n");
                }
            }
            
            base.Children.Add(maze.GetCanvas());
            base.Children.Add(player.GetCanvas());
        }

        public void Destroy()
        {
            base.Children.Remove(player.GetCanvas());
        }
        void RemoveMonster(Monster deadMonster)
        {
            maze.RemoveMonster(deadMonster);
        }


        public DisplayMode Update(double time)
        {
            SetPlayerAngle();
            maze.move(time);
            Monster combat = maze.CheckForCombat();
            if (combat != null)
            {
                ResetMovement();
                return SetUpCombat(player, combat);
            }
            return this;
        }


        private void ResetMovement()
        {
            left = 0;
            right = 0;
            up = 0;
            down = 0;
        }
        Combat SetUpCombat(Player playIn, Monster monsterIn)
        {
            Combat currentCombat = new Combat(base.ActualWidth, base.ActualHeight, playIn.GetPlayerFighter, monsterIn.Fighter,this);
            Monster currentCombatMonster = monsterIn;
            RemoveMonster(monsterIn);
            return currentCombat;
            //base.Content = currentInControl;


        }

        void SetPlayerAngle()
        {
            int divider = right + down + left + up;
            if (divider == 0 || (divider == 2 && 2 == up + down) || (divider == 2 && 2 == left + right))
            {
                player.Velocity = 0;
                return;
            }
            else
            {
                player.Velocity = player.MaxVelocity;
            }

            int x = 0 + 5 * right - 5 * left;
            int y = 0 + 5 * down - 5 * up;
            int deltaX = x - 0;
            int deltaY = y - 0;
            double angle = Math.Atan2(deltaY, deltaX);

            player.Angle = angle;
        }

        public void KeyPress(Key e)
        {

            switch (e)
            {
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

        public void KeyRelease(Key e)
        {
            switch (e)
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
                case Key.Space:
                    player.Interact();
                    break;
            }
        }
    }
}
