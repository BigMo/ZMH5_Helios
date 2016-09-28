using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.UI.Drawing;
using ZatsHackBase.UI.Drawing.Buffers;
using D3D11 = SharpDX.Direct3D11;
using Math = ZatsHackBase.Maths.Math;

namespace ZatsHackBase.UI
{
    public class GeometryBuffer : IDisposable
    {
        #region Constructors

        public GeometryBuffer(Renderer renderer)
        {
            _Renderer = renderer;
            
            _VertexBuffer = new D3D11.Buffer(_Renderer.Device,
                new D3D11.BufferDescription(Vertex.Size * 8192, D3D11.ResourceUsage.Dynamic, D3D11.BindFlags.VertexBuffer,
                    D3D11.CpuAccessFlags.Write, D3D11.ResourceOptionFlags.None, 0));

            _IndexBuffer = new D3D11.Buffer(_Renderer.Device,
                new D3D11.BufferDescription(sizeof(short) * 4096, D3D11.ResourceUsage.Dynamic, D3D11.BindFlags.IndexBuffer,
                    D3D11.CpuAccessFlags.Write, D3D11.ResourceOptionFlags.None, 0));

            _ClipBuffer = new ClipBuffer(renderer);
            _TransformationBuffer = new TransformationBuffer(renderer);
        }

        ~GeometryBuffer()
        {
            Dispose();
        }

        #endregion

        #region Variables

        private readonly Renderer           _Renderer;
        private SharpDX.Direct3D11.Buffer   _VertexBuffer;
        private SharpDX.Direct3D11.Buffer   _IndexBuffer;

        //private RawMatrix _ViewMatrix = new RawMatrix();
        //private RawMatrix _ProjMatrix = new RawMatrix();
        //private RawMatrix _WorldMatrix = new RawMatrix();

        private bool _Synchronised = false;

        private List<Vertex> _Vertices = new List<Vertex>();
        private List<short>  _Indices = new List<short>();  
        private List<Batch>  _Batches = new List<Batch>(); 

        private Batch        _Dummy = new Batch();

        private ClipBuffer _ClipBuffer;
        private TransformationBuffer _TransformationBuffer;

        #endregion

        #region Properties

        public int Vertices => _Vertices.Count;
        public int Indices => _Indices.Count;
        public int VertexDataSize => _Vertices.Count * Vertex.Size;
        public int IndexDataSize => _Indices.Count * sizeof(short);

        public RectangleF ClipRegion
        {
            get { return _Dummy.ClipRectangle; }
            set { _Dummy.ClipRectangle = value; }
        }

        #endregion

        #region Methods

        public void Reset()
        {
            _Vertices.Clear();
            _Indices.Clear();
            _Batches.Clear();

            _Synchronised = true;
        }

        public void Dispose()
        {
            _Vertices.Clear();
            _Indices.Clear();
            _Batches.Clear();

            _VertexBuffer.Dispose();
            _IndexBuffer.Dispose();
        }

        public Vertex FixVertex(Vertex vertex)
        {
            vertex.Origin.X = (((vertex.Origin.X / _Renderer.ViewportSize.Width) * 2) - 1);
            vertex.Origin.Y = -(((vertex.Origin.Y / _Renderer.ViewportSize.Height) * 2) - 1);

            return vertex;
        }

        public void AppendVertex(Vertex vertex)
        {
            _Vertices.Add(FixVertex(vertex));
            _Dummy.VertexCount++;
            _Synchronised = false;
        }

        public void AppendVertices(params Vertex[] vertices)
        {
            if (vertices.Length == 0)
                return;

            _Vertices.AddRange(vertices.Select(x => FixVertex(x)));
            _Dummy.VertexCount += vertices.Length;
            _Synchronised = false;
        }

        public void AppendIndex(short index)
        {
            _Indices.Add(index);
            _Dummy.IndexCount++;
            _Synchronised = false;
        }

        public void AppendIndices(params short[] indices)
        {
            if (indices.Length == 0)
                return;

            _Indices.AddRange(indices);
            _Dummy.IndexCount += indices.Length;
            _Synchronised = false;
        }

        private void Synchronize()
        {

           // if (1 > 0) todo check 4 size (mismatch?)
            {

                DataStream vertexBuffer, indexBuffer;


                _Renderer.DeviceContext.MapSubresource(_VertexBuffer, D3D11.MapMode.WriteDiscard, D3D11.MapFlags.None, out vertexBuffer);

                _Vertices.ForEach(vertex => { vertexBuffer.Write(vertex); });

                _Renderer.DeviceContext.UnmapSubresource(_VertexBuffer, 0);


                _Renderer.DeviceContext.MapSubresource(_IndexBuffer, D3D11.MapMode.WriteDiscard, D3D11.MapFlags.None, out indexBuffer);

                _Indices.ForEach(index => { indexBuffer.Write(index); });

                _Renderer.DeviceContext.UnmapSubresource(_IndexBuffer, 0);

            }

            _Synchronised = true;

        }

        public void Trim()
        {
            _Batches.Add(_Dummy);
            _Dummy.IndexCount = _Dummy.VertexCount = 0;
            _Dummy.UseIndices = true;
            _Dummy.Texture = null;
            _Dummy.Sampler = null;
        }

        public void Draw()
        {
            if (_Synchronised == false)
                Synchronize();

            _Renderer.DeviceContext.InputAssembler.SetVertexBuffers(0,
                new D3D11.VertexBufferBinding(_VertexBuffer, Vertex.Size, 0));

            _Renderer.DeviceContext.InputAssembler.SetIndexBuffer(_IndexBuffer, Format.R16_UInt, 0);

            int vertex_offset = 0;
            int index_offset = 0;

            ShaderSet last_shader = null;

            RectangleF clip = new RectangleF(0f, 0f, _Renderer.ViewportSize.Width, _Renderer.ViewportSize.Height);

            _TransformationBuffer.Apply();
            _ClipBuffer.Apply();

            foreach (var batch in _Batches)
            {

                if (batch.TargetShader != null && batch.TargetShader != last_shader)
                {
                    batch.TargetShader.Apply();
                    last_shader = batch.TargetShader;
                }

                if (batch.UseClipping == true && batch.ClipRectangle != clip)
                {
                    _ClipBuffer.ClipRegion = batch.ClipRectangle;
                    _ClipBuffer.Synchronise();
                }

                batch.ShaderBuffer?.Apply();
                
                _Renderer.DeviceContext.PixelShader.SetSampler(0, batch.Sampler);
                _Renderer.DeviceContext.PixelShader.SetShaderResource(0, batch.Texture);
                _Renderer.DeviceContext.InputAssembler.PrimitiveTopology = batch.DrawMode;
               
                if (batch.UseIndices == true)
                {
                    _Renderer.DeviceContext.DrawIndexed(batch.IndexCount, index_offset, vertex_offset);
                    index_offset += batch.IndexCount;
                }
                else
                {
                    _Renderer.DeviceContext.Draw(batch.VertexCount, vertex_offset);
                }

                vertex_offset += batch.VertexCount;
            }

        }

        public void SetupTexture(D3D11.ShaderResourceView texture, D3D11.SamplerState state)
        {
            _Dummy.Texture = texture;
            _Dummy.Sampler = state;
        }

        public void SetPrimitiveType(PrimitiveTopology topology)
        {
            _Dummy.DrawMode = topology;
        }

        public void DisableUseOfIndices()
        {
            _Dummy.UseIndices = false;
        }

        public void SetShader(ShaderSet shader)
        {
            _Dummy.TargetShader = shader;
        }

        #endregion
    }
}
