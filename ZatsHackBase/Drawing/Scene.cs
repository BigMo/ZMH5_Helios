using SharpDX;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3D11 = SharpDX.Direct3D11;

namespace ZatsHackBase.Drawing
{
    public class Scene : IDisposable
    {
        /*
         *  Die Scene-klasse erwartet, dass man immer 2 vertices hinzufügt, die immer für linien verantwortlich sind
         * */
        #region VARIABLES
        private readonly Renderer _Renderer;
        private D3D11.Buffer _VertexBuffer;
        
        private bool _Synchronised = false;

        struct Buf<T>
        {
            public T[] Elements;
            public int Count;
            public int MaxCount;

            public Buf(int maxElements = 1000)
            {
                MaxCount = maxElements;
                Count = 0;
                Elements = new T[MaxCount];
            }

            public void Add(T val)
            {
                Elements[Count] = val;
                Count++;
                if (Count > MaxCount)
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

        private Buf<Detail.Vertex3D> _Vertices = new Buf<Detail.Vertex3D>();
        #endregion

        #region PROPERTIES
        public delegate bool FunctionalCondition(ref int vertexOffset, object obj);
        public bool ConditionedRendering { get; set; }
        public FunctionalCondition Condition { get; set; }
        public object ConditionObject { get; set; }
        public bool Enabled { get; set; }
        #endregion

        #region CONSTRUCTORS

        public Scene(Renderer renderer, int size)
        {
            _Renderer = renderer;

            _VertexBuffer = new D3D11.Buffer(_Renderer.Device,
                new D3D11.BufferDescription(size, D3D11.ResourceUsage.Dynamic, D3D11.BindFlags.VertexBuffer,
                    D3D11.CpuAccessFlags.Write, D3D11.ResourceOptionFlags.None, 0));
        }

        ~Scene()
        {
            Dispose();
        }

        #endregion

        #region METHODS
        private void Synchronize()
        {
            DataStream vertexBuffer;
            int i;

            _Renderer.DeviceContext.MapSubresource(_VertexBuffer, D3D11.MapMode.WriteDiscard, D3D11.MapFlags.None, out vertexBuffer);

            for (i = 0; i < _Vertices.Count; i++)
            {
                vertexBuffer.Write(_Vertices.Elements[i]);
            }

            _Renderer.DeviceContext.UnmapSubresource(_VertexBuffer, 0);
        }

        public void Dispose()
        {
            _Vertices.Clear();
            _VertexBuffer.Dispose();
        }

        // gibt den index von der vertex liste zurück, für conditioned rendering
        public int AddLine(Maths.Vector3 from, Maths.Vector3 to, Color color)
        {
            var col = (RawColor4)color;
            int idx = _Vertices.Count;
            _Vertices.Add(new Detail.Vertex3D(from.X, from.Y, from.Z, col));
            _Vertices.Add(new Detail.Vertex3D(to.X, to.Y, to.Z, col));
            return idx;
        }

        // gibt den index von der vertex liste zurück, für conditioned rendering
        public int AddLine(Maths.Vector3 from, Color fromColor, Maths.Vector3 to, Color toColor)
        {
            int idx = _Vertices.Count;
            _Vertices.Add(new Detail.Vertex3D(from.X, from.Y, from.Z, fromColor));
            _Vertices.Add(new Detail.Vertex3D(to.X, to.Y, to.Z, toColor));
            return idx;
        }

        public void Draw()
        {
            if (!_Synchronised)
                Synchronize();
            if(ConditionedRendering)
            {
                int vertexOffset = 0;
                while (Condition(ref vertexOffset, ConditionObject))
                {
                    _Renderer.DeviceContext.Draw(2, vertexOffset);
                }
            }
            else
            {
                for (int i = 0; i < _Vertices.Count; i+=2)
                {
                    _Renderer.DeviceContext.Draw(2, i);
                }
            }
        }

        public void Reset()
        {
            _Vertices.Clear();
            _Synchronised = false;
        }
        #endregion
    }
}
