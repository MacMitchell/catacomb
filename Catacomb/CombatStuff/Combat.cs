using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

using Catacomb.Maze;

namespace Catacomb.CombatStuff
{
    class Combat : Grid, DisplayMode
    {
        //Grid combatGrid;

        TextBlock abilityText;
        TextBlock monsterText;
        TextBlock playerText;
        MainCombatView currentView;
        private protected CombatEntity player;
        private CombatEntity monster;

        private double mainCellHeight;
        private double mainCellWidth;
        private DisplayMode previousDisplay;

        public Grid CombatGrid
        {
            get { return this; }
        }

        public DisplayMode PreviousDisplay
        {
            get { return previousDisplay; }
            set { previousDisplay = value; }
        }

        public CombatEntity Player { get => player; }
        public CombatEntity Monster { get => monster;  }
        public int ReportStatus { get
            {
                if (currentView == null)
                {
                    return Command.IGNORE_COMMAND;
                }

                return currentView.ReportStatus;
            } set
            {
                if (currentView != null)
                {
                    currentView.ReportStatus = value;
                }
            }
        }


        private MainCombatView CurrentView
        {
            set { if(currentView != value)
                {
                    if (currentView != null)
                    {
                        base.Children.Remove(currentView);
                    }
                    currentView = value;
                    Grid.SetColumn(currentView, 0);
                    Grid.SetColumn(currentView, 1);
                    base.Children.Add(currentView);
                } }
            get { return currentView; }
        }

        public SwitchDisplayMode Finished { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        CommandIterator it;
        public Combat(double width, double height,CombatEntity playerIn, CombatEntity monsterIn,DisplayMode previous) :base()
        {
            previousDisplay = previous;
            
            it = new CommandIterator(null);



            player = playerIn;
            monster = monsterIn;

            //combatGrid = new Grid();
            base.Focus();
            base.KeyDown += (object sender, System.Windows.Input.KeyEventArgs e) => { KeyRelease(e.Key); };
           

            /*combatGrid.MaxHeight = Screen.PrimaryScreen.WorkingArea.Size.Height;
            combatGrid.MaxWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;*/
            base.Height = height;
            base.Width = width;
            base.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
            
            base.ShowGridLines = true;

            CreateGrid(width, height);
            playerText = new TextBlock();
            Grid.SetColumn(playerText, 2);
            Grid.SetRow(playerText, 0);
            Grid.SetRowSpan(playerText, 2);
            base.Children.Add(playerText);
            playerText.Foreground = Global.Globals.COMBAT_FONT_COLOR;


            monsterText = new TextBlock();
            Grid.SetColumn(monsterText, 0);
            Grid.SetRow(monsterText, 0);
            Grid.SetRowSpan(monsterText, 2);
            base.Children.Add(monsterText);
            monsterText.Foreground = Global.Globals.COMBAT_FONT_COLOR;
            
            SetUpEntity(Player, true);
            SetUpEntity(Monster, false);
            SetUpAbility();

            UpdateStats();
        }
        void SetUpEntity(CombatEntity entity, Boolean player)
        {
            entity.Reset();
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
            CurrentView = new DisplayCommand(mainCellWidth, mainCellHeight, player, monster, it,monsterText,playerText);

            abilityText = new TextBlock();

            //Grid.SetColumn(currentView,0);
            //Grid.SetColumn(currentView, 1);
            //base.Children.Add(currentView);


            abilityText.Text = Player.Name + " VS. " + Monster.Name;
            abilityText.Foreground = Brushes.White;
        }

         
        void CreateGrid(double width, double height)
        {
            //double width = Screen.PrimaryScreen.WorkingArea.Size.Height;
            //double height = Screen.PrimaryScreen.WorkingArea.Size.Height;
            
            RowDefinition topRow = new RowDefinition();
            mainCellHeight = height * 0.75;
            topRow.Height = new System.Windows.GridLength(mainCellHeight);
            
            base.RowDefinitions.Add(topRow);

            RowDefinition bottomRow = new RowDefinition();
            base.RowDefinitions.Add(bottomRow);

            ColumnDefinition sideRows = new ColumnDefinition();
            sideRows.Width = new System.Windows.GridLength(width * 0.25);
            base.ColumnDefinitions.Add(sideRows);

            ColumnDefinition main = new ColumnDefinition();
            mainCellWidth = width * 0.5;
            main.Width = new System.Windows.GridLength(mainCellWidth);
            base.ColumnDefinitions.Add(main);

            sideRows = new ColumnDefinition();
            sideRows.Width = new System.Windows.GridLength(width * 0.25);
            base.ColumnDefinitions.Add(sideRows);
        }


        public void UpdateStats()
        {
            monsterText.Text = Monster.GenerateStats();
            playerText.Text = Player.GenerateStats();
        }
        

        public void KeyRelease(Key keyIn)
        {
            CurrentView = currentView.KeyRelease(keyIn);
        }

        public void KeyPress(Key keyIn)
        {
            CurrentView = currentView.KeyPress(keyIn);
        }

        public System.Windows.Controls.Panel GetDisplay()
        {
            return this;
        }

        public DisplayMode Update(double time)
        {
            if(IsCombatOver())
            {
                
                return previousDisplay;
            }
            return this;
        }

       

  

        public Boolean IsCombatOver()
        {
            return player.Health <= 0 || monster.Health <= 0;
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        private abstract class MainCombatView : Grid
        {
            protected CombatEntity player;
            protected CombatEntity monster;
            protected CommandIterator it;

            protected TextBlock monsterText;
            protected TextBlock playerText;
            private int reportStatus;
            public int ReportStatus { get => reportStatus; set => reportStatus = value; }

            public MainCombatView(double width, double height, CombatEntity player, CombatEntity monster, CommandIterator it, TextBlock leftSide, TextBlock rightSide) : base()
            {
                base.Width = width;
                base.Height = height;
                this.player = player;
                this.monster = monster;
                this.it = it;
                this.monsterText = leftSide;
                this.playerText = rightSide;
                ReportStatus = Command.IGNORE_COMMAND;
            }


            public abstract MainCombatView KeyRelease(Key keyIn);
            public virtual MainCombatView KeyPress(Key keyIn) { return this; }

        }
        private class DisplayCommand: MainCombatView
        {
            private TextBlock actionText;
            

            public DisplayCommand(double width, double height, CombatEntity player, CombatEntity monster, CommandIterator it,TextBlock leftSide, TextBlock rightSide): base(width, height, player, monster, it,leftSide,rightSide)
            {
                actionText = new TextBlock();
                actionText.Foreground = Global.Globals.COMBAT_FONT_COLOR;
                base.Children.Add(actionText);
                
                actionText.Text = player.Name + " VS " + monster.Name;
            }
            public override MainCombatView KeyRelease(Key keyIn)
            {
                int result = Command.IGNORE_COMMAND;
                if(keyIn == Key.Space)
                {
                    result = ExecuteNext();
                }
                ReportStatus = result;
                if(result == Command.FETCH_PLAYER_ATTACK)
                {
                    return new AttackSelect(base.Width, base.Height, player,monster,it, monsterText, playerText);
                }
                return this;
            }

            public void UpdateStats()
            {
                monsterText.Text = monster.GenerateStats();
                playerText.Text = player.GenerateStats();
            }

            public void SetUpTurn()
            {
                //Eventually this will do more than just attacks
                SetUpAttacks();
            }

            public int ExecuteNext()
            {
                if (it.CurrentCommand == null)
                {
                    SetUpTurn();
                }
                int result = it.CurrentCommand.Execute(player, monster);
                actionText.Text = it.CurrentCommand.Description;
                UpdateStats();
                it.Next();
                if (monster.Health <= 0)
                {
                    return Command.MONSTER_DIED;
                }
                if (player.Health <= 0)
                {
                    return Command.PLAYER_DIED;
                }
                return result;
            }
            public void SetUpAttacks()
            {
                //in the fruture this will not change the current command, but currentCommand.Next
                GetAttacksCommand getAttacks = new GetAttacksCommand(it);
                it.CurrentCommand = getAttacks;
            }

        }

        private class AttackSelect : MainCombatView
        {
            private int rowsSize = 6;
            private List<TextBlock> rows;
           
            
            
            private int currentRow;
            private int currentColumn;
            private int maxRow;
            private int maxColumn;

            private Canvas select;
            public AttackSelect(double width, double height, CombatEntity player, CombatEntity monster, CommandIterator it,TextBlock leftSide, TextBlock rightSide) : base(width,height,player,monster,it,leftSide,rightSide)
            {
                rows = new List<TextBlock>();

                select = new Canvas();
                select.Background = Brushes.Orange;

                CreateGrid();
                currentRow = 0;
                currentColumn = 0;
                
            }

            

            public void CreateGrid()
            {
                //int gridHeight = 80;
                //rowsSize = (int) base.Height / gridHeight;

                double gridHeight = base.Height / rowsSize;

                ColumnDefinition column = new ColumnDefinition();
                double selectColumnWidth = base.Width * 0.2;
                column.Width = new System.Windows.GridLength(selectColumnWidth);
                base.ColumnDefinitions.Add(column);

                select.Width = selectColumnWidth;
                select.Height = gridHeight;

                ColumnDefinition column2 = new ColumnDefinition();
                column2.Width = new System.Windows.GridLength(base.Width * 0.8);
                base.ColumnDefinitions.Add(column2);

                for (int i =0; i < rowsSize; i++)
                {
                    RowDefinition bottomRow = new RowDefinition();
                    bottomRow.Height = new System.Windows.GridLength(gridHeight);
                    
                    base.RowDefinitions.Add(bottomRow);
                }
                base.ShowGridLines = true;
                FillGrid();
                PopulateGrid(0);

                List<Attack> attacks = player.GetListOfAttacks();
                maxColumn = 1+((attacks.Count-1) / rowsSize);

                
                currentRow = 0;
                currentColumn = 0;
                Grid.SetColumn(select, 0);
                Grid.SetRow(select, 0);
                base.Children.Add(select);
            }
            public void PopulateGrid(int column)
            {
                List<Attack> attacks = player.GetListOfAttacks();
                int start = rowsSize * column;
                int count = 0;
                
                for (; count < rowsSize; count++)
                {
                    if(count +start >= attacks.Count)
                    {
                        rows[count].Text = "";
                    }
                    else
                    {
                        rows[count].Text = attacks[count + start].Name;
                    }
                }
                if (attacks.Count - start >= rowsSize)
                {
                    maxRow = rowsSize;
                }
                else
                {
                    maxRow = (attacks.Count - start) % rowsSize;
                }
                if (currentRow >= maxRow)
                {
                    currentRow = maxRow - 1;
                    Select(currentRow);
                }
                
            }
            public void FillGrid()
            {
                List<Attack> attacks = player.GetListOfAttacks();
                int count = 0;
                for(;count < rowsSize; count++)
                {
                    TextBlock toFill = CreateSlot();
                    rows.Add(toFill);
                    Grid.SetColumn(toFill, 1);
                    Grid.SetRow(toFill, count);
                    base.Children.Add(toFill);
                }

            }

            public void Select(int row)
            {
                base.Children.Remove(select);

                Grid.SetColumn(select, 0);
                Grid.SetRow(select, row);
                base.Children.Add(select);

            }

            public TextBlock CreateSlot()
            {
                TextBlock temp = new TextBlock();
                temp.Foreground = Global.Globals.COMBAT_FONT_COLOR;
                return temp;
            }

            public void Select(int rowOffset, int columnOffset)
            {
                if(currentRow + rowOffset >= maxRow || rowOffset+currentRow < 0 || currentColumn + columnOffset >= maxColumn || currentColumn + columnOffset < 0)
                {
                    return;
                }

                currentRow += rowOffset;
                currentColumn += columnOffset;
                Select(currentRow);
                PopulateGrid(currentColumn);
            }

            public void SelectAttack()
            {
                int index = currentColumn * rowsSize + currentRow;
                Attack selectedAttack = player.GetAttack(index, it.CurrentCommand);
                selectedAttack.Castor = player;
                selectedAttack.Target = monster;
            }
            public override MainCombatView KeyRelease(Key keyIn)
            {
                if(keyIn == Key.W)
                {
                    Select(-1, 0);
                }
                if(keyIn == Key.S)
                {
                    Select(1, 0);
                }
                if(keyIn == Key.A)
                {
                    Select(0, -1);
                }
                if(keyIn == Key.D)
                {
                    Select(0, 1);
                }
                if(keyIn == Key.Space)
                {
                    SelectAttack();
                    DisplayCommand nextDisplay = new DisplayCommand(base.Width, base.Height, player, monster, it, monsterText, playerText);
                    ReportStatus = nextDisplay.ExecuteNext();
                    nextDisplay.ReportStatus = ReportStatus;
                    return nextDisplay;
                }
                return this ;
            }
        }
    }

  
}
