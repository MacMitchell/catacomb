using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Timers;

using Catacomb.Maze;
using Catacomb.Visuals;
using Catacomb.Vectors;
using Catacomb.Entities;
namespace Catacomb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public delegate void SetContent(Canvas newContent);

    public class Start
    {
        private static Label textBox1;
        private static Label textBox2;
        private static CatMaze testMaze;
        private static Button left;
        private static Button right;
        private static Button up;
        private static Button down;
        private static Canvas roomCanvas;
        private static Canvas visibleCanvas;
        private static Player player;
        [STAThread]
        static void Main(string[] args)

        {
            Application test = new Application();
            CatacombMain cat = new CatacombMain();
            //MainWindow window = new MainWindow();
            //test.MainWindow = window;
            //Window mazeWindow = GetMazeTest();
            test.MainWindow = cat;
            //test.MainWindow = mazeWindow;
            //mazeWindow.Show();


            //mazeWindow.KeyDown += MoveKeyPress;
            //Thread mainThread = new Thread(Update);

            
            cat.StartUp();
            test.Run();
        }





        public static void Update(object sender, EventArgs e)

        {
                DateTime currentTime = DateTime.Now;

                DateTime temp = DateTime.Now;
                TimeSpan timeDifference = temp - currentTime;
                currentTime = DateTime.Now;
                testMaze.move(1);
        }
        public static void MoveKeyPress(object sender, KeyEventArgs e)
        {
            /*Canvas.SetLeft(roomCanvas, -40);
            Canvas.SetTop(roomCanvas, -40);*/

        }
        private static double currentTime; 



        
        private static Window GetMazeTest()
        {

            Window returnWindow = new Window();
            Canvas mainCanvas2 = new Canvas();
            mainCanvas2.Background = Brushes.Black;
            mainCanvas2.Width = returnWindow.Width;
            mainCanvas2.Height = returnWindow.Height;

            MazeBuilder builder = new MazeBuilder();
            testMaze = new CatMaze();
            testMaze.Create(15, 3, new Player(new Vectors.Point(225,225)));

            textBox1 = new Label();
            textBox1.Foreground = Brushes.White;
            textBox2 = new Label();
            textBox2.Foreground = Brushes.White;

            
            /*Line test = new Line();
            test.X1 = 0.0;
            test.Y1 = 0.0;
            test.X2 = 500;
            test.Y2 = 500;
            test.Stroke = Brushes.Red;
            mainCanvas.Children.Add(test);
            */

            //this.MouseDown += testF;



            left = new Button();
            left.Content = "Left";
            left.Margin = new Thickness(15, 15, 10, 0);
            left.Width = 200;
            left.Height = 50;
            left.HorizontalAlignment = HorizontalAlignment.Left;
            left.Click += (sender, EventArgs) => { Move(3); };



            right = new Button();
            right.Content = "right";
            right.Margin = new Thickness(15, 15, 10, 0);
            right.Width = 200;
            right.Height = 50;
            right.HorizontalAlignment = HorizontalAlignment.Right;
            right.Click += (sender, EventArgs) => { Move(1); };



            up = new Button();
            up.Content = "Up";
            up.Margin = new Thickness(15, 15, 10, 0);
            up.Width = 200;
            up.Height = 50;
            up.Click += (sender, EventArgs) => { Move(0); };


            down = new Button();
            down.Content = "Down";
            down.Margin = new Thickness(15, 15, 10, 0);
            down.Width = 200;
            down.Height = 50;
            down.Click += (sender, EventArgs) => { Move(2); };


            StackPanel verticalStack = new StackPanel();
            verticalStack.Orientation = Orientation.Vertical;
            StackPanel horizontalStack = new StackPanel();
            horizontalStack.Orientation = Orientation.Horizontal;

            mainCanvas2.Children.Add(verticalStack);
            
            verticalStack.Children.Add(up);
            verticalStack.Children.Add(horizontalStack);
            verticalStack.Children.Add(textBox1);
            verticalStack.Children.Add(textBox2);
            horizontalStack.Children.Add(left);
            horizontalStack.Children.Add(down);
            
            horizontalStack.Children.Add(right);

            returnWindow.Content = mainCanvas2;

            roomCanvas = new Canvas();
            visibleCanvas = new Canvas();
            verticalStack.Children.Add(visibleCanvas);

            roomCanvas.Width = 500;
            roomCanvas.Height = 500;

            visibleCanvas.Background = Brushes.Blue;

            visibleCanvas.Width = 1000;
            visibleCanvas.Height = 1000;

            visibleCanvas.Children.Add(roomCanvas);
            roomCanvas.Width = 500;
            roomCanvas.Height = 500;


            Wall test = new Wall(new Vectors.Point(5, 5), new Vectors.Point( 50, 50));
            //roomCanvas.Children.Add(test.GetCanvas());

            updateLabels();

            roomCanvas.Children.Clear();
            roomCanvas.Children.Add(testMaze.GetCanvas());
            //roomCanvas.Children.Add(player.GetCanvas());
            //player.Container = testMaze.Start.RoomDrawn;
            return returnWindow;
        }

        public static void updateRoomCanvas()
        {
            //testMaze.Draw(new Vectors.Point(50,50),new Vectors.Point(250,250));
            roomCanvas.Children.Clear();
            //player.Container = testMaze.Start.RoomDrawn;
            roomCanvas.Children.Add(testMaze.GetCanvas());
            //roomCanvas.Children.Add(player.GetCanvas());
        }
        public static void updateLabels()
        {
           // left.Background = testMaze.HasConnection(3) ? Brushes.White : Brushes.Red;
            //right.Background = testMaze.HasConnection(1) ? Brushes.White : Brushes.Red;
            //up.Background = testMaze.HasConnection(0) ? Brushes.White : Brushes.Red;
            //down.Background = testMaze.HasConnection(2) ? Brushes.White : Brushes.Red;
            //textBox1.Content = testMaze.GetRoomType();
            //textBox2.Content = testMaze.getId();
        }
        public static void Move(int direction)
        {
            
            /*if (!testMaze.HasConnection(direction))
            {
                return;
            }
            testMaze = testMaze.GetConnectedRoom(direction);
            updateLabels();
            updateRoomCanvas();*/
        }
    }
   
    public partial class MainWindow : Window
    {
        Canvas mainCanvas;

        
        public MainWindow()
        {
            mainCanvas = new Canvas();
            mainCanvas.Background = Brushes.Black;
            mainCanvas.Width = this.Width;
            mainCanvas.Height = this.Height;
            
            /*Line test = new Line();
            test.X1 = 0.0;
            test.Y1 = 0.0;
            test.X2 = 500;
            test.Y2 = 500;
            test.Stroke = Brushes.Red;
            mainCanvas.Children.Add(test);
            */
            this.Content = mainCanvas;
            this.Title = "HELLO WORLD";
            this.Show();
            //this.MouseDown += testF;

            StackPanel tester3 = new StackPanel();
            
            mainCanvas.Children.Add(tester3);

            Button left = new Button();
            left.Content = "Left";
            left.Margin = new Thickness(15, 15, 10, 0);
            left.Width = 200;
            left.Height = 50;
            left.HorizontalAlignment = HorizontalAlignment.Left;


            Button right = new Button();
            right.Content = "right";
            right.Margin = new Thickness(15, 15, 10, 0);
            right.Width = 200;
            right.Height = 50;
            right.HorizontalAlignment = HorizontalAlignment.Right;


            Button up = new Button();
            up.Content = "Up";
            up.Margin = new Thickness(15, 15, 10, 0);
            up.Width = 200;
            up.Height = 50;

            Button down = new Button();
            down.Content = "Down";
            down.Margin = new Thickness(15, 15, 10, 0);
            down.Width = 200;
            down.Height = 50;

            tester3.Children.Add(left);
            tester3.Children.Add(down);
            tester3.Children.Add(up);
            tester3.Children.Add(right);


            this.Show();
            


        }
        void testF(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Message here");
        }

        
    }



}
