using D3D11 = SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;
using SharpDX.D3DCompiler;
using ZatsHackBase.Maths;
using SharpDX;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.UI
{
    //TODO: Implement
    public class Renderer : IDisposable
    {
        #region VARIABLES
        private D3D11.Device d3dDevice;
        private D3D11.DeviceContext d3dDeviceContext;
        private SwapChain swapChain;
        private D3D11.RenderTargetView renderTargetView;
        private D3D11.BlendState blendState;

        // Shaders
        private ShaderSet fontShader;
        private ShaderSet primitiveShader;
        #endregion

        #region PROPERTIES
        public bool Initialized { get { return d3dDevice != null; } }
        public D3D11.Device Device => d3dDevice;
        public D3D11.DeviceContext DeviceContext => d3dDeviceContext;
        public GeometryBuffer GeometryBuffer { get; set; }
        public Size2F ViewportSize { get; set; }
        public Size2F hViewportSize { get; set; }
        #endregion

        #region CONSTRUCTORS
        public Renderer()
        {
        }
        #endregion

        #region METHODS
        #region RENDERER
        public void Init(Form form)
        {
            ModeDescription backBufferDesc = new ModeDescription(form.Width, form.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm);
            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(4, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = form.Handle,
                IsWindowed = true
            };

            D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out d3dDevice, out swapChain);
            d3dDeviceContext = d3dDevice.ImmediateContext;
            
            d3dDeviceContext.Rasterizer.SetViewport(0, 0, form.Width, form.Height);
            using (D3D11.Texture2D backBuffer = swapChain.GetBackBuffer<D3D11.Texture2D>(0))
            {
                renderTargetView = new D3D11.RenderTargetView(d3dDevice, backBuffer);
            }
            blendState = new D3D11.BlendState(d3dDevice, D3D11.BlendStateDescription.Default());
            GeometryBuffer = new GeometryBuffer(this);
            InitializeShaders();

            ViewportSize = new Size2F(form.Width, form.Height);
            hViewportSize = new Size2F(form.Width/2f, form.Height/2f);
        }

        private void InitializeShaders()
        {

            var shader =
                @"
                struct Vertex
                {
                    float4 Origin : POSITION;
                    float4 Color  : COLOR;
                    float2 UV     : TEXCOORDS;
                };

                struct Pixel
                {
                    float4 Origin : SV_POSITION;
                    float4 Color  : COLOR;
                    float2 UV     : TEXCOORDS;
                };

                Pixel vertex_entry ( Vertex vertex )
                {
                    vertex.Origin.z = 0.0f;
                    vertex.Origin.w = 1.0f;

                    Pixel output;
                    
                    output.Origin   = vertex.Origin;
                    output.Color    = vertex.Color;
                    output.UV       = vertex.UV;
                    
                    return output;
                }

                Texture2D    g_texture : register( t0 );           
                SamplerState g_linearSampler : register( s0 );

                float4 pixel_entry ( Pixel pixel ) : SV_TARGET
                {
                    float4 midvalue = g_texture.Sample(g_linearSampler, pixel.UV ) + pixel.Color;
                    return midvalue / 2;
                }

                ";

            primitiveShader = new ShaderSet(this,
                shader, "vertex_entry", "pixel_entry", new[]
                {
                    new D3D11.InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                    new D3D11.InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0),
                    new D3D11.InputElement("TEXCOORDS", 0, Format.R32G32_Float, 32, 0),
                });

            primitiveShader.Apply();
        }
        
        public void Clear(Color color)
        {
            if (!Initialized)
                return;

            d3dDeviceContext.OutputMerger.SetRenderTargets(renderTargetView);
            d3dDeviceContext.OutputMerger.SetBlendState(blendState, new Color(0f, 0f, 0f, 0f), 0xFFFFFFFF);
            d3dDeviceContext.ClearRenderTargetView(renderTargetView, color);
            
        }

        public void Present()
        {
            if (!Initialized)
                return;

            GeometryBuffer.Draw();
            GeometryBuffer.Reset();

            swapChain.Present(1, PresentFlags.None);
        }

        public void Dispose()
        {
            if (!Initialized)
                return;

            renderTargetView.Dispose();
            swapChain.Dispose();
            d3dDevice.Dispose();
            d3dDeviceContext.Dispose();
        }
        #endregion

        #region RENDER FEATURES
        public void DrawLine(Color color, Vector2 from, Vector2 to)
        {
            if (!Initialized)
                return;

            var col = (RawColor4)color;
            GeometryBuffer.AppendVertices(
                new Vertex(from.X,from.Y,col),
                new Vertex(to.X,to.Y,col)
            );

            //GeometryBuffer.AppendIndices(new short[]
            //{
            //    0,
            //    1
            //});

            GeometryBuffer.SetShader(primitiveShader);
            GeometryBuffer.DisableUseOfIndices();
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.LineList);
            GeometryBuffer.Trim();
        }

        public void DrawLines(Color color, Vector2[] points)
        {
            if (!Initialized)
                return;

            var col = (RawColor4)color;

            points.ToList().ForEach(el => { GeometryBuffer.AppendVertex(new Vertex { Origin = el, Color = col }); });

            GeometryBuffer.SetShader(primitiveShader);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.LineList);
            GeometryBuffer.Trim();
        }

        public void FillRectangle(Color color, Vector2 location, Vector2 size)
        {
            if (!Initialized)
                return;

            var col = (RawColor4)color;

            GeometryBuffer.AppendVertices(
                new Vertex(location.X, location.Y, col),
                new Vertex(location.X + size.X, location.Y, col),
                new Vertex(location.X, location.Y + size.Y, col),
                new Vertex(location.X + size.X, location.Y + size.Y, col)
            );

            GeometryBuffer.AppendIndices(
                0,
                1,
                2,

                1,
                2,
                3
            );

            GeometryBuffer.SetShader(primitiveShader);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.TriangleList);
            GeometryBuffer.Trim();
        }

        public void DrawRectangle(Color color, Vector2 location, Vector2 size)
        {
            if (!Initialized)
                return;

            var col = (RawColor4)color;
            GeometryBuffer.AppendVertices(
                new Vertex(location.X, location.Y, col),
                new Vertex(location.X + size.X, location.Y, col),
                new Vertex(location.X, location.Y + size.Y, col),
                new Vertex(location.X + size.X, location.Y + size.Y, col)
            );

            GeometryBuffer.AppendIndices(
                // top left -> top right
                0,
                1,

                // top right -> bottom right
                1,
                3,

                // bottom right -> bottom left
                3,
                2,

                // bottom left -> top left
                2,
                0
            );

            GeometryBuffer.SetShader(primitiveShader);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.LineList);
            GeometryBuffer.Trim();
        }

        #endregion

        #region FONT
        


        #endregion
        #endregion
    }
}
