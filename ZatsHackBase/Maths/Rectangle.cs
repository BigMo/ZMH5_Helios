using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;

namespace ZatsHackBase.Maths
{
    public struct Rectangle
    {
        #region VARIABLES
        public Vector2 Location;
        public Vector2 Size;
        #endregion

        #region PROPERTIES
        public float Left
        {
            get
            {
                return Location.X;

            }
            set
            {
                Location.X = value;
            }
        }

        public float Top
        {
            get { return Location.Y; }
            set { Location.Y = value; }
        }
        
        public float Width
        {
            get
            {
                return Size.X;

            }
            set
            {
                Size.X = value;
            }
        }

        public float Height
        {
            get
            {
                return Size.Y;

            }
            set
            {
                Size.Y = value;
            }
        }
        #endregion

        #region CONSTRUCTORS
        public Rectangle(float x, float y, float w, float h)
        {
            Location = new Vector2(x, y);
            Size = new Vector2(w, h);
        }
        #endregion

        #region METHODS
        public bool Contains(float x, float y)
        {
            return x <= Left && y >= Top && x <= Left + Width && y <= Top + Height;
        }
        #endregion
    }
}
