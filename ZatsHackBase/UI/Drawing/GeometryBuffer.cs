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
                new D3D11.BufferDescription(1024 * 1024 * 10, D3D11.ResourceUsage.Dynamic, D3D11.BindFlags.VertexBuffer,
                    D3D11.CpuAccessFlags.Write, D3D11.ResourceOptionFlags.None, 0));

            _IndexBuffer = new D3D11.Buffer(_Renderer.Device,
                new D3D11.BufferDescription(1024 * 1024 * 10, D3D11.ResourceUsage.Dynamic, D3D11.BindFlags.IndexBuffer,
                    D3D11.CpuAccessFlags.Write, D3D11.ResourceOptionFlags.None, 0));
        }

        ~GeometryBuffer()
        {
            Dispose();
        }

        #endregion

        #region Variables

        private readonly Renderer           _Renderer;
        private D3D11.Buffer   _VertexBuffer;
        private D3D11.Buffer _IndexBuffer;

        //private RawMatrix _ViewMatrix = new RawMatrix();
        //private RawMatrix _ProjMatrix = new RawMatrix();
        //private RawMatrix _WorldMatrix = new RawMatrix();

        private bool _Synchronised = false;

        struct Buf<T>
        {
            public T[] Elements;
            public uint Count;
            public uint MaxCount;

            public Buf(uint maxElements = 1000)
            {
                MaxCount = maxElements;
                Count = 0;
                Elements = new T[MaxCount];
            }

            public void Add(T val)
            {
                Elements[Count] = val;
                Count++;
                if(Count > MaxCount)
                {
                    MaxCount += 200;
                    T[] newElements = new T[MaxCount];
                    for (int i = 0; i < Count; i++)
                        newElements[i] = Elements[i];
                    Elements = newElements;
                }
            }

            public void AddRange(IEnumerable<T> d)
            {

            }

            public void Clear()
            {
                Count = 0;
            }
        }

        private Buf<Vertex> _Vertices = new Buf<Vertex>();
        private Buf<short>  _Indices = new Buf<short>();  
        private Buf<Batch>  _Batches = new Buf<Batch>(); 
        
        private Batch        _Dummy = new Batch();
        
        #endregion

        #region Properties

        public uint Vertices => _Vertices.Count;
        public uint Indices => _Indices.Count;
        public long VertexDataSize => _Vertices.Count * Vertex.Size;
        public uint IndexDataSize => _Indices.Count * sizeof(short);
        public long CopiedMemory => VertexDataSize + IndexDataSize;

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
            _Vertices.Add(vertex);
            _Dummy.VertexCount++;
            _Synchronised = false;
        }

        public void AppendVertices(params Vertex[] vertices)
        {
            if (vertices.Length == 0)
                return;

            _Vertices.AddRange(vertices.Select(x => x));
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
                int i;

                _Renderer.DeviceContext.MapSubresource(_VertexBuffer, D3D11.MapMode.WriteDiscard, D3D11.MapFlags.None, out vertexBuffer);
                
                for (i = 0; i < _Vertices.Count; i++)
                {
                    vertexBuffer.Write(_Vertices.Elements[i]);
                }

                _Renderer.DeviceContext.UnmapSubresource(_VertexBuffer, 0);
                

                _Renderer.DeviceContext.MapSubresource(_IndexBuffer, D3D11.MapMode.WriteDiscard, D3D11.MapFlags.None, out indexBuffer);
                
                for (i = 0; i < _Vertices.Count; i++)
                {
                    indexBuffer.Write(_Indices.Elements[i]);
                }

                _Renderer.DeviceContext.UnmapSubresource(_IndexBuffer, 0);

            }

            _Synchronised = true;

        }

        public void Trim()
        {
            _Batches.Add(_Dummy);
            _Dummy.IndexCount = _Dummy.VertexCount = 0;
            _Dummy.Texture = null;
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
            
            D3D11.ShaderResourceView last_res = null;

            RectangleF clip = new RectangleF(0f, 0f, _Renderer.ViewportSize.Width, _Renderer.ViewportSize.Height);
            
            foreach (var batch in _Batches.Elements)
            {
                if (batch.Texture != last_res)
                {
                    _Renderer.DeviceContext.PixelShader.SetShaderResource(0, batch.Texture);
                    last_res = batch.Texture;
                }
                
                _Renderer.DeviceContext.InputAssembler.PrimitiveTopology = batch.DrawMode;
               
                if (batch.IndexCount > 0)
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

        public void SetupTexture(D3D11.ShaderResourceView texture)
        {
            _Dummy.Texture = texture;
        }

        public void SetPrimitiveType(PrimitiveTopology topology)
        {
            _Dummy.DrawMode = topology;
        }
        
        #endregion
    }
}
