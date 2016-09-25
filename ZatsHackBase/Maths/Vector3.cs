using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    public struct Vector3
    {
        #region VARIABLES
        public float X;
        public float Y;
        public float Z;
        #endregion

        #region PROPERTIES
        public static Vector3 Unit { get; private set; } = new Vector3(1f, 1f, 1f);
        public static Vector3 UnitX { get; private set; } = new Vector3(1f, 0f, 0f);
        public static Vector3 UnitY { get; private set; } = new Vector3(0f, 1f, 0f);
        public static Vector3 UnitZ { get; private set; } = new Vector3(0f, 0f, 1f);
        public static Vector3 Zero { get; private set; } = new Vector3(0f, 0f, 0f);

        public float Length
        {
            get
            {
                return (float)System.Math.Abs(X * X + Y * Y + Z * Z);
            }
        }
        #endregion

        #region CONSTRUCTOR
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public Vector3(Vector3 vec) : this(vec.X, vec.Y, vec.Z) { }
        public Vector3(float[] values) : this(values[0], values[1], values[2]) { }
        #endregion

        #region METHODS
        public float Dot(Vector3 other)
        {
            return (X * other.X) + (Y * other.Y) + (Z * other.Z);
        }
        #endregion

        #region OPERATORS
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vector3 operator *(Vector3 v1, float scalar)
        {
            return new Vector3(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(Vector3 v1, Vector3 v2)
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
                    case 2:
                        return this.Z;
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
                    case 2:
                        this.Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public void Normalize()
        {
            float len = Length;
            if (len != 0f)
            {
                X /= len;
                Y /= len;
                Z /= len;
            }
        }
        #endregion
    }
}
