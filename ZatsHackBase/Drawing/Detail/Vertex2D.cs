using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace ZatsHackBase.Drawing.Detail
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct Vertex2D
    {
        public Vertex2D(float x, float y, RawColor4 color)
        {
            Origin.X = x;
            Origin.Y = y;
            Color = color;
            UV = new Vector2(0f, 0f);
        }
        public Vertex2D(float x, float y, RawColor4 color, float u, float v)
        {
            Origin.X = x;
            Origin.Y = y;
            Color = color;
            UV = new Vector2(u, v);
        }

        [FieldOffset(0)] public Vector2 Origin;
        [FieldOffset(8)] public RawColor4 Color;
        [FieldOffset(24)] public Vector2 UV;

        public static D3D11.InputElement[] layout = new D3D11.InputElement[]
        {
            new D3D11.InputElement("POSITION", 0, DXGI.Format.R32G32_Float, 0, 0),
            new D3D11.InputElement("COLOR", 0, DXGI.Format.R32G32B32A32_Float, 8, 0),
            new D3D11.InputElement("TEXCOORDS", 0, DXGI.Format.R32G32_Float, 24, 0),
        };

        public static int Size = sizeof(float) * 8;
    }
}
