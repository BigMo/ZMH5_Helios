using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    //TODO: Durch Vector2 ersetzen?
    public struct Size
    {
        public Vector2 Vec;

        public float Width
        {
            get { return Vec.X; }
            set { Vec.X = value; }
        }

        public float Height
        {
            get { return Vec.Y; }
            set { Vec.Y = value; }
        }
    }
}
