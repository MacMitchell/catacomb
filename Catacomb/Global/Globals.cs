using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Catacomb.Global
{
    public static class Globals
    {
        private static Random rand;
        public static Random Rand
        {
            get
            {
                if (rand == null)
                {
                    rand = new Random();
                }
                return rand;
            }
        }

        public const bool DEBUG = false;
        public const double TOLERANCE = 0.00005;


        //Visual globals
        public const double LINE_THICKNESS = 10.0;
        public  static Avalonia.Media.IImmutableBrush LINE_COLOR = Avalonia.Media.Brushes.White;
        public static Avalonia.Media.IImmutableBrush MAZE_BACKGROUND_COLOR = Avalonia.Media.Brushes.Black;
        public static Avalonia.Media.IImmutableBrush FLOOR_COLOR = Avalonia.Media.Brushes.DarkBlue;
        public static Avalonia.Media.IImmutableBrush COMBAT_FONT_COLOR = Avalonia.Media.Brushes.White; 
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
