using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace ZatsHackBase.UI.Drawing.Buffers
{
    public class TransformationBuffer : Buffer
    {

        #region CONSTRUCTORS

        public TransformationBuffer(Renderer renderer)
        {

            _Renderer = renderer;

            _Buffer = new SharpDX.Direct3D11.Buffer(_Renderer.Device,
                new SharpDX.Direct3D11.BufferDescription(sizeof(float) * 4, SharpDX.Direct3D11.ResourceUsage.Dynamic, SharpDX.Direct3D11.BindFlags.ConstantBuffer,
                    SharpDX.Direct3D11.CpuAccessFlags.Write, SharpDX.Direct3D11.ResourceOptionFlags.None, sizeof(float)));
            
            DataStream stream;
            _Renderer.DeviceContext.MapSubresource(_Buffer, SharpDX.Direct3D11.MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out stream);

            stream.Write(_Renderer.hViewportSize.Width);
            stream.Write(_Renderer.hViewportSize.Height);
            stream.Write(_Renderer.ViewportSize.Width);
            stream.Write(_Renderer.ViewportSize.Height);

            _Renderer.DeviceContext.UnmapSubresource(_Buffer, 0);
        }

        #endregion

        #region VARIABLES

        private Renderer _Renderer;
        private SharpDX.Direct3D11.Buffer _Buffer;

        #endregion

        #region METHODS
        
        public override void Apply()
        {
            _Renderer.DeviceContext.VertexShader.SetConstantBuffer(0, _Buffer);
        }

        public override void Dispose()
        {
            _Buffer.Dispose();
        }

        #endregion 
    }
}
