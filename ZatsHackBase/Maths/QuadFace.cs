using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    public struct QuadFace
    {
        public Vector3 A, B, C, D;
        public Triangle[] Triangles;

        /* (looking at the FRONT)
         * 
         *  A __ D
         *   /\ |
         *  /__\|
         * B     C
         */
        public QuadFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            Triangles = new Triangle[]
            {
                new Triangle(A, B, C),
                new Triangle(A, C, D)
            };
        }
    }
}
