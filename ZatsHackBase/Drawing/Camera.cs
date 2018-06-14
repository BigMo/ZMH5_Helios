using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;

namespace ZatsHackBase.Drawing
{
    public class Camera
    {
        private bool synchronized;

        private Maths.Matrix viewMatrix;
        public Maths.Matrix ViewMatrix { get { return viewMatrix; } set { viewMatrix = value; synchronized = false; } }

        private Maths.Matrix projMatrix;
        public Maths.Matrix ProjectionMatrix { get { return projMatrix; } set { projMatrix = value; synchronized = false; } }

        private Maths.Matrix viewProj;

        internal Renderer Renderer;
        internal D3D11.Buffer TransformBuffer;

        internal Camera(Renderer renderer)
        {
            Renderer = renderer;
            TransformBuffer = new D3D11.Buffer(Renderer.Device,
                new D3D11.BufferDescription(sizeof(float) * Maths.Matrix.NUM_ELEMENTS * 2, D3D11.ResourceUsage.Dynamic, D3D11.BindFlags.ConstantBuffer,
                    D3D11.CpuAccessFlags.Write, D3D11.ResourceOptionFlags.None, sizeof(float)));
            synchronized = true;
        }

        public void Update()
        {
            if(!synchronized)
            {
                DataStream stream;
                int i;

                Renderer.DeviceContext.MapSubresource(TransformBuffer, D3D11.MapMode.WriteDiscard, D3D11.MapFlags.None, out stream);

                stream.Write(viewProj);
                stream.Write(projMatrix);

                Renderer.DeviceContext.UnmapSubresource(TransformBuffer, 0);
            }
        }
    }
}
