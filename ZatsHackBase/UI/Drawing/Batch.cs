using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.UI
{
    public struct Batch
    {
        public int VertexCount;
        public int IndexCount;

        public bool UseIndices;
        public bool UseClipping;

        public bool SetupTexture;

        public Rectangle ClipRegion;
        public PrimitiveTopology DrawMode;
        public ShaderSet TargetShader;

        public ShaderResourceView Texture;
        public SamplerState Sampler;
    }
}
