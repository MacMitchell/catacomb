using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catacomb.Vectors;
using Avalonia.Media;
using Catacomb.Maze;
using Catacomb.Entities;
using Catacomb.CombatStuff;
using Avalonia.Platform;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Threading;

namespace Catacomb
{
    public sealed class CatacombManager : Avalonia.Controls.Window
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
        DispatcherTimer time;
        DateTime currentTime;
        private bool updateFinish = true;


        

        public CatPopUp currentPopUp;

        //Main settings
        int numberOfRooms = 10; //the value set here is the base value
        int numberOfMonsters = 5; //the value set here is the base value

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

            //base.Height =  Screen.PrimaryScreen.WorkingArea.Size.Height;
            //base.Width = Screen.PrimaryScreen.WorkingArea.Size.Width;

            base.Title = "UwU";
            base.WindowState = Avalonia.Controls.WindowState.FullScreen;
   
            base.Background = Global.Globals.MAZE_BACKGROUND_COLOR;
            time = new DispatcherTimer();

            time.Interval = new TimeSpan(10);
            time.IsEnabled = true;
            
            time.Tick += Update;
            currentTime = DateTime.Now;

            this.KeyDown += MoveKeyPress;
            this.KeyUp += MoveKeyRelease;
            Instance = this;

            currentPopUp = null; //null means that there is no popUp
            this.Show();
        }

        public void StartUp()
        {

            player = new Player(new Vectors.Point(0, 0));

            ExploreCanvas maze = new ExploreCanvas(base.Bounds.Size.Width, base.Bounds.Size.Height, player);
            //maze.SetUpMaze(numberOfRooms,numberOfMonsters);
            maze.SetUpMaze(MazeFactory.BasicMaze(player));
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
            if(currentPopUp != null && currentPopUp.UpdateBlocking)
            {
                return;
            }
            Display = display.Update(dif);

            updateFinish = true;
        }

        void MoveKeyPress(object sender, Avalonia.Input.KeyEventArgs e)
        {
            //temporary

            
            if (e.Key == Avalonia.Input.Key.Space && currentPopUp == null)
            {
                CatPopUp testPopUp = new CatPopUp();
                testPopUp.Message = "Hello World";
                testPopUp.Title = "TITLE HERE";
                testPopUp.UpdateBlocking = true;
                //DisplayPopUp(testPopUp);
            }

            else if (currentPopUp != null)
            {
                currentPopUp.KeyPress(e.Key);
            }
            if (display != null && (currentPopUp == null || !currentPopUp.KeyBlocking))
            {
                display.KeyPress(e.Key);
            }

        }

        void MoveKeyRelease(object sender, Avalonia.Input.KeyEventArgs e)
        {
            if(currentPopUp != null && currentPopUp.KeyBlocking)
            {
                return;
            }
           if (display != null)
            {
                display.KeyRelease(e.Key);
            }
        }

        public void NextFloor(CatMaze newFloor)
        {
            Display.Destroy();
            ExploreCanvas maze = new ExploreCanvas(base.Bounds.Size.Width, base.Bounds.Size.Height, player);
            maze.SetUpMaze(newFloor);

            Display = maze;
        }

        void ScaleLevel()
        {
            numberOfRooms += 10;
        }

        public void DisplayPopUp(CatPopUp popUp)
        {
            popUp.Create(Display.GetDisplay());
            currentPopUp = popUp;
        }

    }
    public delegate DisplayMode SwitchDisplayMode();
    public interface DisplayMode
    {
        SwitchDisplayMode Finished { get; set; }
        Avalonia.Controls.Panel GetDisplay();
        DisplayMode Update(double time);
        void KeyPress(Avalonia.Input.Key  Keyin);
        void KeyRelease(Avalonia.Input.Key keyIn);
        void Destroy();
    }


    public class CatPopUp : Avalonia.Controls.Canvas
    {
        Action destory;
        TextBlock bodyText = null;
        TextBlock titleText = null;
        public Action onFinish = null;

        private double textOffset = 20;

        //stops the update loop from occuring
        bool updateBlocking = true;
        public bool UpdateBlocking
        {
            set { updateBlocking = value; }
            get { return updateBlocking; }
        }

        //does not anything else besides the popup to accept key input
        bool keyBlocking = false;
        public bool KeyBlocking
        {
            set { keyBlocking = value; }
            get { return keyBlocking; }
        }

        public string Message
        {
            set 
            { if(bodyText == null)
                {
                    bodyText = new TextBlock();
                    bodyText.Background = Avalonia.Media.Brushes.Transparent;
                    bodyText.Foreground = Avalonia.Media.Brushes.LightGray;
                    bodyText.Width = this.Width - (2.0 * textOffset);

                    bodyText.TextAlignment = Avalonia.Media.TextAlignment.Center;
                    bodyText.FontSize = 18;
                    bodyText.FontFamily = new Avalonia.Media.FontFamily("Times New Roman");

                    Canvas.SetLeft(bodyText, textOffset);
                    Canvas.SetTop(bodyText, this.Height / 2.3);
                    this.Children.Add(bodyText);
                }
                bodyText.Text = value;
            }
            get 
            { if(bodyText != null)
                {
                    return bodyText.Text;
                }
                return null;
            }
        }

        public string Title
        {
            set
            {
                if (titleText == null)
                {
                    titleText = new TextBlock();
                    titleText.Background = Avalonia.Media.Brushes.Transparent;
                    titleText.Foreground = Avalonia.Media.Brushes.LightGray;
                    titleText.Width = this.Width - (2.0 * textOffset);

                    titleText.TextAlignment = Avalonia.Media.TextAlignment.Center;
                    titleText.FontSize = 50;
                    titleText.FontFamily = new Avalonia.Media.FontFamily("Times New Roman Bold");

                    Canvas.SetLeft(titleText, textOffset);
                    Canvas.SetTop(titleText, this.Height / 10);
                    this.Children.Add(titleText);
                }
                titleText.Text = value;
            }
            get
            {
                if (titleText != null)
                {
                    return titleText.Text;
                }
                return null;
            }
        }

        private BrushConverter converter; 
        public CatPopUp():base()
        {
            
             converter = new Avalonia.Media.BrushConverter();

            var brush = Avalonia.Media.Brushes.DarkBlue;//(Avalonia.Media.Brush) converter.ConvertFromString("#02427D");
            this.Background = brush;
            //this.Background = "";
            this.Opacity = 0.9;
            this.Width = 550;
            this.Height = 250;

            CreateOutline();
            
        }

        public void Create(Avalonia.Controls.Panel parent)
        {
            double leftOffset = (parent.Width / 2.0) - (this.Width / 2.0);
            double topOffset = (parent.Height / 2.0) - (this.Height / 2.0);

            destory = () => { parent.Children.Remove(this); };

            Canvas.SetLeft(this, leftOffset);
            Canvas.SetTop(this, topOffset);
            parent.Children.Add(this);
        }

        public void KeyPress(Avalonia.Input.Key  e)
        {
           if(e == Avalonia.Input.Key.Space)
            {
                destory();
                CatacombManager.Instance.currentPopUp = null;
                if(onFinish != null){
                    onFinish();
                }
            }
        }
        

        void CreateOutline()
        {
            var outlineColor = Avalonia.Media.Brushes.Gold;
            Rectangle outline = new Rectangle();
            outline.Stroke = outlineColor;
            outline.StrokeThickness = 8;
            outline.Fill = Avalonia.Media.Brushes.Transparent;
            outline.Width = this.Width;
            outline.Height = this.Height;
            this.Children.Add(outline);

            Line sep = new Line();
            sep.StartPoint = new Avalonia.Point(0, this.Height / 2.8);
            sep.EndPoint = new Avalonia.Point(this.Width, this.Height / 2.8);

            
            sep.Stroke = outlineColor;
            sep.StrokeThickness = 8;
            this.Children.Add(sep);

            
        }

    }
   
}
