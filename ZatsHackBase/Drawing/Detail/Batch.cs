using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Drawing.Detail
{
    public struct Batch
    {
        public int VertexCount;
        public int IndexCount;

        public bool UseClipping;

        public SharpDX.Direct3D.PrimitiveTopology DrawMode;

        public SharpDX.Direct3D11.ShaderResourceView Texture;
    }
}
