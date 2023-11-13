using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Catacomb.Global
{
    public static class Globals
    {

        public const bool DEBUG = true;
        public const double TOLERANCE = 0.00005;


        //Visual globals
        public const double LINE_THICKNESS = 5.0;
        public  static SolidColorBrush LINE_COLOR = Brushes.White;
        public static SolidColorBrush BACKGROUND_COLOR = Brushes.Black;
        public const  int CONNECTION_LIMIT = 4; 
        public static bool AreDoublesEqual(double d1, double d2)
        {
            return Math.Abs(d1 -  d2) < TOLERANCE;
    }
    }
    
}
