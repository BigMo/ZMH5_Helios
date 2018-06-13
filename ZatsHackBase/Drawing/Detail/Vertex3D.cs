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
    internal struct Vertex3D
    {
        public Vertex3D(float x, float y, float z, RawColor4 color)
        {
            Origin.X = x;
            Origin.Y = y;
            Origin.Z = z;
            Color = color;
        }

        [FieldOffset(0)] public Vector3 Origin;
        [FieldOffset(12)] public RawColor4 Color;
        
        public static D3D11.InputElement[] layout = new D3D11.InputElement[]
        {
            new D3D11.InputElement("POSITION", 0, DXGI.Format.R32G32B32_Float, 0, 0),
            new D3D11.InputElement("COLOR", 0, DXGI.Format.R32G32B32A32_Float, 12, 0),
        };

        public static int Size = sizeof(float) * 7;
    }
}
