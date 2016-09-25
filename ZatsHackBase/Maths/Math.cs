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
        #endregion
    }
}
