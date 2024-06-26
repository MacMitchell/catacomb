﻿using Catacomb.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

using Catacomb.Visuals;
using Catacomb.CombatStuff;

namespace Catacomb.Entities
{
    public sealed class Player :Entity
    {

        private static double playerWidth = 25;
        private static double playerHeight= 25;
        private static Player instance = null;
        public static Player Instance
        {
            get { return instance; }
            set
            {
                if (instance == null)
                {
                    instance = value;
                }
            }
        }
        public override CombatEntity Fighter
        {
            get { if(fighter == null)
                    {
                        fighter = MonsterFactory.GeneratePlayer();
                    }
                    return fighter;
                }
            set { if(fighter == null)
                    {
                        fighter = value;
                    } 
            }
        }
        public Player(Point positionIn) : base(positionIn, new CatRectangle(0, 0, playerWidth, playerHeight)) {
            canvas.Width = Width;
            canvas.Height = Height;
            //canvas.Background = Brushes.Orange;
            SetColor(Brushes.Orange);
            Velocity = 0;
            MaxVelocity = 800;
            Instance = this;
            Draw();
        }


        public void KeyPress(Key e)
        {
            switch (e)
            {
                case Key.A:  Angle = Math.PI;  break;
                case Key.S:  Angle = Math.PI/2; break;
                case Key.D:  Angle = 0;  break;
                case Key.W:  Angle = 3*Math.PI/2; break;
            }
        }

        public override CombatEntity GetCombatEntity()
        {
            return new CombatEntity("Player",200);
        }

    }
}
