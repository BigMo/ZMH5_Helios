using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Drawing
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
        public Color(System.Drawing.Color color) : this(color.A / 255f, color.R / 255f, color.G / 255f, color.B / 255f)
        {

        }
        public Color(float a, float r, float g, float b)
        {
            /*float max = Math.Max(Math.Max(Math.Max(Math.Max(1, g), b), r), a);
            float min = Math.Min(Math.Min(Math.Min(Math.Min(0, g), b), r), a);
            float range = Math.Abs(min - max);
            A = (a + min) / range;
            R = (r + min) / range;
            G = (g + min) / range;
            B = (b + min) / range;*/
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public Color(float r, float g, float b) : this(1f, r, g, b) { }
        public static Color FromKnownColor(Color col, float a)
        {
            return new Color(a, col.R, col.G, col.B);
        }
        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb(
                (byte)(A * 255f),
                (byte)(R * 255f),
                (byte)(G * 255f),
                (byte)(B * 255f));
        }
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
        public static bool operator ==(Color color, Color col2)
        {
            return color.A == col2.A && color.R == col2.R && color.G == col2.G && color.B == col2.B;
        }
        public static bool operator !=(Color color, Color col2)
        {
            return !(color == col2);
        }
        #endregion
    }
}
