using System.Drawing;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.UI
{
    public struct Batch
    {
        public int VertexCount;
        public int IndexCount;

        public bool UseIndices;
        public bool UseClipping;
        
        public SharpDX.Direct3D.PrimitiveTopology DrawMode;

        public SharpDX.Direct3D11.ShaderResourceView Texture;
        public SharpDX.Direct3D11.SamplerState Sampler;

        public RectangleF ClipRectangle;

        public ShaderSet TargetShader;
        public Buffer ShaderBuffer;
    }
}
