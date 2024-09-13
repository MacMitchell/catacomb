using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;

using Avalonia.Input;
using Catacomb.Entities;
using Catacomb.Entities;
using Catacomb.Util;
namespace Catacomb.Menu
{
    public class ClassMenu : Avalonia.Controls.Canvas, DisplayMode
    {
        private Player player;
        private bool closing;
        private DisplayMode previous;
        SelectGrid selectGrid;
        public ClassMenu(double width, double height, Player player, DisplayMode previous) : base()
        {
            base.Width = width;
            base.Height = height;
            base.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
            this.player = player;
            closing = false;
            this.previous = previous;

            this.Background = Brushes.Black;
            CreateGrid();
            
        }

        private void CreateGrid()
        {
            List<string> data = new List<string>();
            List<Action> functions = new List<Action>();
            int selectedIndex = -1;
            int counter = 0;
            foreach( var x in player.GetPlayerFighter.AllClasses){
                data.Add(x.Name + "\n" + x.Description);
                functions.Add(() => player.GetPlayerFighter.CurrentCatClass = x);
                if(player.GetPlayerFighter.CurrentCatClass == x)
                {
                    selectedIndex = counter;
                }
                counter++;
            }
            selectGrid = new SelectGrid(Width, Height, data, functions,selectedIndex,true);
            this.Children.Add(selectGrid);
        }
        public SwitchDisplayMode Finished { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Destroy()
        {
            this.Children.Clear();
        }

        public Panel GetDisplay()
        {
            return this;
        }

        public void KeyPress(Key Keyin)
        {
            previous.KeyPress(Keyin);

        }

        public void KeyRelease(Key keyIn)
        {
            if(keyIn == Key.Escape)
            {
                closing = true;
            }
            else
            {
                previous.KeyRelease(keyIn);
                selectGrid.KeyRelease(keyIn);
            }
        }

        public DisplayMode Update(double time)
        {
            return closing ? previous : this;
        }
    }
}
