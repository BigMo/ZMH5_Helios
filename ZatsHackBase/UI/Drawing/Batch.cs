using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI
{
    public struct Batch
    {
        public int VertexCount;
        public int IndexCount;

        public bool UseIndices;
        public bool UseClipping;

        public Rectangle ClipRegion;

        public PrimitiveTopology DrawMode;
    }
}
