using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Maths
{
    //https://stackoverflow.com/questions/1966587/given-3-pts-how-do-i-calculate-the-normal-vector
    public struct Box
    {
        public Vector3 Center { get; private set; }
        public Vector3 BBMin { get; private set; }
        public Vector3 BBMax { get; private set; }
        public QuadFace Top, Bottom, Left, Right, Front, Back;

        //public Box(Vector3 center, Vector3 bbMin, Vector3 bbMax)
        //{
        //    Center = center;
        //    BBMin = bbMin;
        //    BBMax = bbMax;

        //    Top = new QuadFace()
        //}
    }
}
