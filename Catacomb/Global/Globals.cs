﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Catacomb.Global
{
    public static class Globals
    {

        public const bool DEBUG = false;
        public const double TOLERANCE = 0.00005;


        //Visual globals
        public const double LINE_THICKNESS = 10.0;
        public  static SolidColorBrush LINE_COLOR = Brushes.White;
        public static SolidColorBrush MAZE_BACKGROUND_COLOR = Brushes.Black;
        public static SolidColorBrush FLOOR_COLOR = Brushes.DarkBlue;
        public static SolidColorBrush COMBAT_FONT_COLOR = Brushes.White; 
        public const  double BASE_ATTACK_STAT = 100.0;
        public const  int CONNECTION_LIMIT = 4; 
        public static bool AreDoublesEqual(double d1, double d2)
        {
            return Math.Abs(d1 -  d2) < TOLERANCE;
        }
        public static bool IsDoubleZero(double d1)
        {
            return Math.Abs(d1) < TOLERANCE;
        }
    }
    
}
