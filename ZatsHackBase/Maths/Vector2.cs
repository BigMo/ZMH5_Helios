using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    public struct Vector2
    {
        #region VARIABLES
        public float X, Y;
        #endregion

        #region PROPERTIES
        public static Vector2 Unit { get; private set; } = new Vector2(1f, 1f);
        public static Vector2 UnitX { get; private set; } = new Vector2(1f, 0f);
        public static Vector2 UnitY { get; private set; } = new Vector2(0f, 1f);
        public static Vector2 Zero { get; private set; } = new Vector2(0f, 0f);

        public float Length
        {
            get { return (float)System.Math.Sqrt(X * X + Y * Y); }
        }
        public float Angle
        {
            get
            {
                return Math.RadiansToDegrees(System.Math.Atan2(X, Y));
            }
            set
            {
                float len = Length;
                if (len != 0f)
                {
                    float ang = Angle;
                    if (Angle != value)
                    {
                        float rad = Math.DegreesToRadians(value);
                        X = (float)(System.Math.Sin(rad) * len);
                        Y = (float)(System.Math.Cos(rad) * len);
                    }
                }
            }
        }
        #endregion

        #region CONSTRUCTORS
        public Vector2(float value) : this(value, value) { }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        #endregion

        #region METHODS
        public void Normalize()
        {
            float len = Length;
            if (len != 0f)
            {
                X /= len;
                Y /= len;
            }
        }
        public float DistanceTo(Vector2 other)
        {
            return (this - other).Length;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
                return false;

            return ((Vector2)obj).GetHashCode() == this.GetHashCode();
        }
        public override int GetHashCode()
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(X), 0) ^ BitConverter.ToInt32(BitConverter.GetBytes(Y), 0);
        }
        public float Dot(Vector2 vec)
        {
            return (X * vec.X) + (Y * vec.Y);
        }
        #endregion

        #region OPERATORS
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public static Vector2 operator *(Vector2 v1, float scalar)
        {
            return new Vector2(v1.X * scalar, v1.Y * scalar);
        }
        public static float operator *(Vector2 v1, Vector2 v2)
        {
            return v1.Dot(v2);
        }
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }
        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return this.X;
                    case 1:
                        return this.Y;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        this.X = value;
                        break;
                    case 1:
                        this.Y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion
    }
}
