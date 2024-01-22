using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace Catacomb.CombatStuff
{
    class Combat
    {
        Canvas canvas;
        Grid combatGrid;

        TextBlock abilityText;
        TextBlock monsterText;
        TextBlock playerText;

        CombatEntity player;
        CombatEntity monster;
        public Canvas Canvas
        {
            get { return canvas; }
        }
        public Grid CombatGrid
        {
            get { return combatGrid; }
        }
        public Combat(double width, double height,CombatEntity playerIn, CombatEntity monsterIn)
        {
            player = playerIn;
            monster = monsterIn;

            combatGrid = new Grid();

            /*combatGrid.MaxHeight = Screen.PrimaryScreen.WorkingArea.Size.Height;
            combatGrid.MaxWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;*/
            combatGrid.Height = height;
            combatGrid.Width = width;
            combatGrid.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
            
            combatGrid.ShowGridLines = true;

            CreateGrid(width, height);
            playerText = new TextBlock();
            Grid.SetColumn(playerText, 2);
            Grid.SetRow(playerText, 0);
            Grid.SetRowSpan(playerText, 2);
            combatGrid.Children.Add(playerText);
            playerText.Foreground = Brushes.White;


            monsterText = new TextBlock();
            Grid.SetColumn(monsterText, 0);
            Grid.SetRow(monsterText, 0);
            Grid.SetRowSpan(monsterText, 2);
            combatGrid.Children.Add(monsterText);
            monsterText.Foreground = Brushes.White;

            SetUpEntity(player, true);
            SetUpEntity(monster, false);
            SetUpAbility();
        }
        void SetUpEntity(CombatEntity entity, Boolean player)
        {
            TextBlock toModify;
            if(player)
            {
                toModify = playerText;
            }
            else
            {
                toModify = monsterText;
            }
            toModify.Text = entity.Name;
        }
        void SetUpAbility()
        {
            abilityText = new TextBlock();

            Grid.SetColumn(abilityText, 1);
            Grid.SetRow(abilityText, 0);
            combatGrid.Children.Add(abilityText);



            abilityText.Text = player.Name + " VS. " + monster.Name;
            abilityText.Foreground = Brushes.White;
        }

         
        void CreateGrid(double width, double height)
        {
            //double width = Screen.PrimaryScreen.WorkingArea.Size.Height;
            //double height = Screen.PrimaryScreen.WorkingArea.Size.Height;
            
            RowDefinition topRow = new RowDefinition();
            topRow.Height = new System.Windows.GridLength(height*0.6666);
            combatGrid.RowDefinitions.Add(topRow);

            RowDefinition bottomRow = new RowDefinition();
            combatGrid.RowDefinitions.Add(bottomRow);

            ColumnDefinition sideRows = new ColumnDefinition();
            sideRows.Width = new System.Windows.GridLength(width * 0.25);
            combatGrid.ColumnDefinitions.Add(sideRows);

            ColumnDefinition main = new ColumnDefinition();
            main.Width = new System.Windows.GridLength(width * 0.5);
            combatGrid.ColumnDefinitions.Add(main);

            sideRows = new ColumnDefinition();
            sideRows.Width = new System.Windows.GridLength(width * 0.25);
            combatGrid.ColumnDefinitions.Add(sideRows);
        }
    }
}
