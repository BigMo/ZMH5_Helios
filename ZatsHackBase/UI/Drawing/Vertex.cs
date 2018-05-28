using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vertex
    {
        public Vertex(float x, float y, RawColor4 color)
        {
            Origin.X = x;
            Origin.Y = y;
            Color = color;
            UV = new Vector2(0f,0f);
        }
        public Vertex(float x, float y, RawColor4 color, float u, float v)
        {
            Origin.X = x;
            Origin.Y = y;
            Color = color;
            UV = new Vector2(u,v);
        }
        
        [FieldOffset(0)] public Vector2 Origin;
        [FieldOffset(8)] public RawColor4 Color;
        [FieldOffset(24)] public Vector2 UV;

        public static int Size = sizeof(float)*8;
    }
}
