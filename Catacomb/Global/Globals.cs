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

        public const bool DEBUG = true;
        //ROOM DEBUGS
        public const bool SHOW_CONNECTION_POINTS = DEBUG && true;
        public const bool SHOW_ROOM_META_TEXT = DEBUG && false;

        //ENTITIES DEBUG
        public const bool SHOW_COLLISION_TESTERS = DEBUG && true;

        
        public const double TOLERANCE = 0.00005;


        //Visual globals
        public const double LINE_THICKNESS = 10.0;
        public  static Avalonia.Media.IImmutableBrush LINE_COLOR = Avalonia.Media.Brushes.White;
        public static Avalonia.Media.IImmutableBrush MAZE_BACKGROUND_COLOR = Avalonia.Media.Brushes.Black;
        public static Avalonia.Media.IImmutableBrush FLOOR_COLOR = Avalonia.Media.Brushes.DarkBlue;
        public static Avalonia.Media.IImmutableBrush COMBAT_FONT_COLOR = Avalonia.Media.Brushes.White;

        public static double DOOR_WIDTH = 100;
        public static double CONNCETION_LENGTH = 300;

        public const  double BASE_ATTACK_STAT = 100.0;
        public const  int CONNECTION_LIMIT = 4;

        //ROOM
        public const int TOP = 0;
        public const int RIGHT = 1;
        public const int BOTTOM = 2;
        public const int LEFT = 3;
        
        
        public static bool AreDoublesEqual(double d1, double d2)
        {
            return Math.Abs(d1 -  d2) < TOLERANCE;
        }
        public static bool IsDoubleZero(double d1)
        {
            return Math.Abs(d1) < TOLERANCE;
        }
        public static double GetRandomNumber(double minimum, double maximum)
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }
    }
    
}
