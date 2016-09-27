using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix
    {
        #region CONSTANTS
        public const int NUM_ROWS = 4;
        public const int NUM_COLS = 4;
        public const int NUM_ELEMENTS = NUM_ROWS * NUM_COLS;
        #endregion

        #region VARIABLES
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_ELEMENTS)]
        private float[] Elements;
        #endregion

        #region OPERATORS
        public float this[int i]
        {
            get
            {
                if (i < 0 || i >= NUM_ELEMENTS)
                    throw new IndexOutOfRangeException("Invalid matrix-index");

                return Elements[i];
            }
        }
        public float this[int row, int column]
        {
            get
            {
                if (row < 0 || row >= NUM_ROWS || column < 0 || column >= NUM_COLS)
                    throw new IndexOutOfRangeException("Invalid matrix-indices");

                return Elements[row * NUM_COLS + column];
            }
        }
        #endregion
    }
}
