using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.D3DCompiler;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI.Drawing.Buffers
{
    public class ClipBuffer : Buffer
    {

        #region CONSTRUCTORS

        public ClipBuffer(Renderer renderer)
        {

            _Renderer = renderer;

            _Buffer = new SharpDX.Direct3D11.Buffer(_Renderer.Device,
                new SharpDX.Direct3D11.BufferDescription(sizeof(float)*4, SharpDX.Direct3D11.ResourceUsage.Dynamic, SharpDX.Direct3D11.BindFlags.ConstantBuffer,
                    SharpDX.Direct3D11.CpuAccessFlags.Write, SharpDX.Direct3D11.ResourceOptionFlags.None, 0));

        }

        #endregion

        #region VARIABLES

        private Renderer _Renderer;
        private SharpDX.Direct3D11.Buffer _Buffer;

        #endregion

        public RectangleF ClipRegion;

        #region METHODS

        private float FixH(float x)
        {
            return (((x / _Renderer.ViewportSize.Width) * 2) - 1);
        }

        private float FixV(float y)
        {
            return -(((y / _Renderer.ViewportSize.Height) * 2) - 1);
        }

        public void Synchronise()
        {

            DataStream stream;
            _Renderer.DeviceContext.MapSubresource(_Buffer, SharpDX.Direct3D11.MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out stream);

            stream.Write(FixH(ClipRegion.Left));
            stream.Write(FixV(ClipRegion.Top));
            stream.Write(FixH(ClipRegion.Left+ClipRegion.Width));
            stream.Write(FixV(ClipRegion.Top+ClipRegion.Height));

            _Renderer.DeviceContext.UnmapSubresource(_Buffer, 0);

        }

        public override void Apply()
        {
            _Renderer.DeviceContext.PixelShader.SetConstantBuffer(0, _Buffer);
        }

        public override void Dispose()
        {
            _Buffer.Dispose();
        }

        #endregion 
    }
}
