using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    public struct Triangle
    {
        public Vector3 A, B, C, Normal;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;

            //Calculate normal
            var ba = b - a;
            var ca = c - a;
            var dir = ba.Cross(ca);
            dir.Normalize();
            Normal = dir;
        }
    }
}
