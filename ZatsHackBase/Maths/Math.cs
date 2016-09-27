using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    public static class Math
    {
        #region CONSTANTS
        private const double DEG2RAD = (System.Math.PI / 180.0);
        private const double RAD2DEG = (180.0 / System.Math.PI);
        #endregion

        #region METHODS
        public static float DegreesToRadians(float deg)
        {
            return (float)(DEG2RAD * deg);
        }
        public static float DegreesToRadians(double deg)
        {
            return (float)(DEG2RAD * deg);
        }
        public static float RadiansToDegrees(float rad)
        {
            return (float)(RAD2DEG * rad);
        }
        public static float RadiansToDegrees(double rad)
        {
            return (float)(RAD2DEG * rad);
        }

        public static Vector2 WorldToScreen(Matrix viewMatrix, Vector2 screenSize, Vector3 point3D)
        {
            Vector2 returnVector = Vector2.Zero;
            float w = viewMatrix[3, 0] * point3D.X + viewMatrix[3, 1] * point3D.Y + viewMatrix[3, 2] * point3D.Z + viewMatrix[3, 3];
            if (w >= 0.01f)
            {
                float inverseX = 1f / w;
                returnVector.X =
                    (screenSize.X / 2f) +
                    (0.5f * (
                    (viewMatrix[0, 0] * point3D.X + viewMatrix[0, 1] * point3D.Y + viewMatrix[0, 2] * point3D.Z + viewMatrix[0, 3])
                    * inverseX)
                    * screenSize.X + 0.5f);
                returnVector.Y =
                    (screenSize.Y / 2f) -
                    (0.5f * (
                    (viewMatrix[1, 0] * point3D.X + viewMatrix[1, 1] * point3D.Y + viewMatrix[1, 2] * point3D.Z + viewMatrix[1, 3])
                    * inverseX)
                    * screenSize.Y + 0.5f);
            }
            return returnVector;
        }
        #endregion
    }
}
