using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Util
{
    public class SelectGrid :Grid
    {
        private int rowsSize = 6;
        private List<TextBlock> rows;

        private List<String> data;
        private List<Action> selectionFunctions;

        private int currentRow;
        private int currentColumn;
        private int maxRow;
        private int maxColumn;
        private int selected;
        private bool updateSelected;
        private Canvas select;

        private IImmutableBrush rowBackground;


        public SelectGrid(double width, double height, List<String> data, List<Action> selectFunctions, int selected =-1, bool updateSelected = false):base()
        {
            this.Width = width;
            this.Height = height;

            rows = new List<TextBlock>();

            select = new Canvas();
            select.Background = Brushes.Orange;

            this.data = data;
            this.selectionFunctions = selectFunctions;
            this.selected = selected;
            this.updateSelected = updateSelected;

            rowBackground = Global.Globals.MAZE_BACKGROUND_COLOR;

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
            column.Width = new GridLength(selectColumnWidth);
            base.ColumnDefinitions.Add(column);

            select.Width = selectColumnWidth;
            select.Height = gridHeight;

            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(base.Width * 0.8);
            base.ColumnDefinitions.Add(column2);

            for (int i = 0; i < rowsSize; i++)
            {
                RowDefinition bottomRow = new RowDefinition();
                bottomRow.Height = new GridLength(gridHeight);

                base.RowDefinitions.Add(bottomRow);
            }
            base.ShowGridLines = true;
            FillGrid();
            PopulateGrid(0);


            maxColumn = 1 + ((data.Count - 1) / rowsSize);


            currentRow = 0;
            currentColumn = 0;
            Grid.SetColumn(select, 0);
            Grid.SetRow(select, 0);
            base.Children.Add(select);
        }
        public void PopulateGrid(int column)
        {
            int start = rowsSize * column;
            int count = 0;

            for (; count < rowsSize; count++)
            {
                if (count + start >= data.Count)
                {
                    rows[count].Text = "";
                }
                else
                {
                    rows[count].Text = data[count + start];
                    if(count == selected)
                    {
                        rows[count].Background = Brushes.SteelBlue; 
                    }
                    else
                    {
                        rows[count].Background = rowBackground;
                    }
                }
            }
            if (data.Count - start >= rowsSize)
            {
                maxRow = rowsSize;
            }
            else
            {
                maxRow = (data.Count - start) % rowsSize;
            }
            if (currentRow >= maxRow)
            {
                currentRow = maxRow - 1;
                Select(currentRow);
            }

        }
        public void FillGrid()
        {
            int count = 0;
            for (; count < rowsSize; count++)
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
            if (currentRow + rowOffset >= maxRow || rowOffset + currentRow < 0 || currentColumn + columnOffset >= maxColumn || currentColumn + columnOffset < 0)
            {
                return;
            }

            currentRow += rowOffset;
            currentColumn += columnOffset;
            Select(currentRow);
            PopulateGrid(currentColumn);
        }

        public void ExecuteSelectFunction()
        {
            int index = currentColumn * rowsSize + currentRow;
            selectionFunctions[index]();
            if (updateSelected)
            {
                selected = index;
                PopulateGrid(currentColumn); //update the rows
            }
        }
        public void KeyRelease(Key keyIn)
        {
            if (keyIn == Key.W)
            {
                Select(-1, 0);
            }
            if (keyIn == Key.S)
            {
                Select(1, 0);
            }
            if (keyIn == Key.A)
            {
                Select(0, -1);
            }
            if (keyIn == Key.D)
            {
                Select(0, 1);
            }
            if (keyIn == Key.Space)
            {
                ExecuteSelectFunction();

                //indicate that you have finished executing your turn

                //execute the attack
            }   
        }
    }
}

