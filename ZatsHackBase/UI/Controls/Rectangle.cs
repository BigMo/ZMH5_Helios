using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI.Controls
{
    public struct Rectangle
    {
        #region PROPERTIES
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float Top { get { return this.Y; } }
        public float Bottom { get { return this.Y + this.Height; } }
        public float Left { get { return this.X; } }
        public float Right { get { return this.X + this.Width; } }
        public static Rectangle Empty { get { return new Rectangle(0f, 0f, 0f, 0f); } }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new rectangle using the given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public Rectangle(Vector2 position, Vector2 size) : this(position.X, position.Y, size.X, size.Y) { }
        /// <summary>
        /// Initializes a new rectangle by copying the values of the given rectangle
        /// </summary>
        /// <param name="rect"></param>
        public Rectangle(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }
        #endregion

        #region METHODS
        /// <summary>
        /// Returns whether this rectangle intersects with the given rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool IntersectsWith(Rectangle rect)
        {
            return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
        }
        /// <summary>
        /// Returns the intersection of this rectangle and the given rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rectangle Intersect(Rectangle rect)
        {
            float x = System.Math.Max(this.X, rect.X);
            float width = System.Math.Min(this.X + this.Width, rect.X + rect.Width);
            float y = System.Math.Max(this.Y, rect.Y);
            float height = System.Math.Min(this.Y + this.Height, rect.Y + rect.Height);
            if (width >= x && height >= y)
            {
                return new Rectangle(x, y, y - x, height - y);
            }
            return Rectangle.Empty;
        }
        public bool Contains(Vector2 pos)
        {
            return pos.X >= X && pos.X <= X + Width && pos.Y >= Y && pos.Y <= Y + Height;
        }
        #endregion
    }
}
