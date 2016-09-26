using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    //TODO: Durch Vector2 ersetzen?
    public struct Point
    {
        public Vector2 Vec;

        public float Left
        {
            get { return Vec.X; }
            set { Vec.X = value; }
        }

        public float Top
        {
            get { return Vec.Y; }
            set { Vec.Y = value; }
        }
    }
}
