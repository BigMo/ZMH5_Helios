using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.UI.Controls
{
    public struct Distance
    {
        public readonly float Top, Left, Right, Bottom;
        
        public Distance(float top, float left, float right, float bottom)
        {
            this.Top = top;
            this.Left = left;
            this.Right = right;
            this.Bottom = bottom;
        }

        public Distance(float dist) : this(dist, dist, dist, dist) { }

        public static bool operator ==(Distance v1, Distance v2)
        {
            return v1.Top == v2.Top && v1.Left == v2.Left && v1.Right == v2.Right && v1.Bottom == v2.Bottom;
        }
        public static bool operator !=(Distance v1, Distance v2)
        {
            return !(v1 == v2);
        }
    }
}
