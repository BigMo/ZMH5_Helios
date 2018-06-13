using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace ZatsHackBase.Drawing.Detail
{
    public struct Glyph
    {
        public char Code;
        public Vector2[] UV;
        public SizeF Size;
    }
}
