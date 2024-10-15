using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catacomb.Vectors;
using Catacomb.Global;
using Avalonia.Media;
using Avalonia.Controls.Shapes;
using Catacomb.Maze;
using Catacomb.CombatStuff;
using Avalonia.Controls;

namespace Catacomb.Entities
{
    public class Monster : Entity
    {
        private Movement movementAI;
        private List<MonsterType> types;
        
        public override CombatEntity Fighter
        {
            get { return fighter;}
            set { fighter = value; }
        }
        public Movement MovementAI
        {
            get { return movementAI; }
            set { movementAI = value; }
            
        }

        public delegate Monster MonsterCloner(Player playin);

        private double oldAngle;
        private MonsterCloner clone;
        private Ellipse outerEye;
        private Ellipse innerEye;

        public List<MonsterType> Types { get => types; set => types = value; }
        public MonsterType Type { set => types.Add(value); }
        public MonsterCloner Clone { get => clone; set => clone = value; }

        public Monster(double width, double height,double maxVelocity) : base(new Point(0,0), new CatRectangle(0, 0, width, height))
        {
            Width = width;
            Height = height;
            canvas.Width = Width;
            canvas.Height = Height;

            

            Ellipse monsterVisual = new Ellipse();
            monsterVisual.Width = Width;
            monsterVisual.Height = Height;
            monsterVisual.Fill = Brushes.DarkRed;            
            canvas.Children.Add(monsterVisual);

            
            DrawEye();

            Types = new List<MonsterType>();

            SetColor(Brushes.Transparent);
            Velocity = 0;
            MaxVelocity = maxVelocity;
            Accerlation = MaxVelocity;
            oldAngle = Angle;
        }
        
        private void DrawEye()
        {
            double outerEyeWidth = 15;
            double innerEyeWidth = 5;
            
            outerEye = new Ellipse();
            outerEye.Width = outerEyeWidth;
            outerEye.Height = outerEyeWidth;
            outerEye.Fill = Brushes.LightBlue;

            innerEye = new Ellipse();
            innerEye.Width = innerEyeWidth;
            innerEye.Height = innerEyeWidth;
            innerEye.Fill = Brushes.Red;

            canvas.Children.Add(outerEye);
            canvas.Children.Add(innerEye);
            UpdateEye();

        }
        private void UpdateEye()
        {
            double outerDistanceX = 15;
            double outerDistanceY = 15;
            double innerDistanceX = 18;
            double innerDistanceY = 18;
            //Canvas.SetLeft(outerEye, (Width / 2) - (outerEye.Width / 2));
            //Canvas.SetTop(outerEye, (Width / 2) - (outerEye.Width/2));
            double angle = TempAngle != null ? (double) TempAngle : Angle;

            Canvas.SetLeft(outerEye, (Width / 2) - (outerEye.Width/2) + (Math.Cos(angle) * outerDistanceX));
            Canvas.SetTop(outerEye, (Width / 2)- (outerEye.Height / 2) + (Math.Sin(angle) * outerDistanceY));


            Canvas.SetLeft(innerEye, (Width / 2) - (innerEye.Width/2) + (Math.Cos(angle) * innerDistanceX));
            Canvas.SetTop(innerEye, (Width / 2) - (innerEye.Height/2) + (Math.Sin(angle) * innerDistanceY));
        }
        public void PlaceMonster(Point spawnPoint)
        {
            Position = spawnPoint;
            Draw();
        }

        public override void Draw()
        {
            UpdateEye();
            base.Draw();
        }
        public override void Move(double time)
        {
            if(MovementAI != null)
            {
                if (movementAI.SetUpMove(time))
                {
                    double saveDifference = 0.5;
                    double worseCase = Math.PI;
                    double angleChange = Math.Abs(Angle - oldAngle);
                    Velocity -= Velocity * (angleChange / worseCase);
                    Velocity = Math.Max(0, Velocity);          
                    oldAngle = Angle;
                    Velocity += Accerlation * time;
                    Velocity = Math.Min(MaxVelocity, Velocity);
                    base.Move(time);
                    Draw();
                }
            }   
        }
        public Monster DoesCollideWithPlayer(List<Room> roomsToCheck, Player playIn)
        {
            if (!roomsToCheck.Contains(this.Container.parent))
            {
                return null;
            }
            if (this.IsWithin(playIn))
            {
                return this;
            }
            return null;
        }
    }
}
