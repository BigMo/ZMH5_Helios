using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.UI.Drawing
{
    public struct Color
    {
        #region VARIABLES
        public float R;
        public float G;
        public float B;
        public float A;
        #endregion

        #region PROPERTIES
        public static Color Transparent => new Color(0f, 0f, 0f, 0f);
        public static Color Black => new Color(0f, 0f, 0f);
        public static Color White => new Color(1f, 1f, 1f);
        public static Color Red => new Color(1f, 0f, 0f);
        public static Color Green => new Color(0f, 1f, 0f);
        public static Color Blue => new Color(0f, 0f, 1f);
        public static Color Orange => new Color(1f, 0.5f, 0.12f);
        #endregion

        #region CONSTRUCTORS
        public Color(float a, float r, float g, float b)
        {
            A = Math.Min(1f, Math.Max(0f, a));
            R = Math.Min(1f, Math.Max(0f, r));
            G = Math.Min(1f, Math.Max(0f, g));
            B = Math.Min(1f, Math.Max(0f, b));
        }
        public Color(float r, float g, float b) : this(1f, r, g, b) { }
        #endregion

        #region OPERATORS
        public static implicit operator RawColor4(Color col)
        {
            return new RawColor4(col.R, col.G, col.B, col.A);
        }
        public static Color operator *(Color color, float scalar)
        {
            return new Color(color.A * scalar, color.R * scalar, color.G * scalar, color.B * scalar);
        }
        public static Color operator +(Color color, float scalar)
        {
            return new Color(color.A + scalar, color.R + scalar, color.G + scalar, color.B + scalar);
        }
        public static Color operator -(Color color, float scalar)
        {
            return new Color(color.A - scalar, color.R - scalar, color.G - scalar, color.B - scalar);
        }
        public static Color operator +(Color color, Color col2)
        {
            return new Color(color.A + col2.A, color.R + col2.R, color.G + col2.G, color.B + col2.B);
        }
        public static Color operator -(Color color, Color col2)
        {
            return new Color(color.A - col2.A, color.R - col2.R, color.G - col2.G, color.B - col2.B);
        }
        #endregion
    }
}
