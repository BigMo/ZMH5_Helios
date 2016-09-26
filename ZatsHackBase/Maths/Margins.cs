using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    public struct Margins
    {
        #region PROPERTIES
        public static Margins Unit { get; private set; } = new Margins(1, 1, 1, 1);
        public static Margins Zero { get; private set; } = new Margins(0, 0, 0, 0);
        #endregion

        #region VARIABLES
        // distance from the left of the client
        public float Left { get; set; }

        // distance from the top of the client
        public float Top { get; set; }

        // distance from the right of the client
        public float Right { get; set; }

        // distance from the bottom of the client
        public float Bottom { get; set; }
        #endregion
        
        #region CONSTRUCTORS
        public Margins(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        #endregion
    }
}
