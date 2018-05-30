using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
        static string pixelShaderCode =
            @"
                Texture2D    g_texture : register(t0);           
                SamplerState g_sampler : register(s0);

                struct pselement
                {
                    float4 pos  : SV_POSITION;
                    float4 col  : COLOR;
                    float2 uv   : TEXCOORDS;
                };

                float4 main(pselement input) : SV_TARGET
                {
                    return input.col * g_texture.Sample ( g_sampler, input.uv );
                }
            ";

        static string vertexShaderCode =
                @"
                cbuffer ShaderParams : register(b0) 
                {
                    float2 g_screenSize;
                }

                struct vsinput
                {
                    float2 pos  : POSITION;
                    float4 col  : COLOR;
                    float2 uv   : TEXCOORDS;
                };

                struct pselement
                {
                    float4 pos  : SV_POSITION;
                    float4 col  : COLOR;
                    float2 uv   : TEXCOORDS;
                };

                float4 fix_pos(float2 orig_origin)
                {
                    return float4(
                        -1.0f + (orig_origin.x / g_screenSize.x),
                        1.0f - (orig_origin.y / g_screenSize.y),
                        0.0f, 1.0f);
                }

                pselement main(vsinput vertex)
                {
                    pselement output;
                    output.pos      = fix_pos(vertex.pos);
                    output.col      = vertex.col;
                    output.uv       = vertex.uv;
                    return output;
                }
                ";

        #region VARIABLES
        private D3D11.Device d3dDevice;
        private D3D11.DeviceContext d3dDeviceContext;
        private SwapChain swapChain;
        private D3D11.RenderTargetView renderTargetView;
        private D3D11.BlendState blendState;
        private D3D11.Buffer transfBuffer;
        private D3D11.VertexShader vertexShader;
        private D3D11.PixelShader pixelShader;
        private D3D11.InputLayout inputLayout;
        private D3D11.SamplerState samplerState;
        #endregion

        #region PROPERTIES
        public bool Initialized { get { return d3dDevice != null; } }
        public D3D11.Device Device => d3dDevice;
        public D3D11.DeviceContext DeviceContext => d3dDeviceContext;
        public D3D11.Texture2D White;
        public D3D11.ShaderResourceView WhiteView;
        public D3D11.SamplerState Sampler;
        public GeometryBuffer GeometryBuffer { get; set; }
        public Size2F ViewportSize { get; set; }
        public Size2F hViewportSize { get; set; }
        public FontCache Fonts { get; private set; }
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
            ViewportSize = new Size2F(form.Width, form.Height);
            hViewportSize = new Size2F(form.Width / 2f, form.Height / 2f);

            ModeDescription backBufferDesc = new ModeDescription(form.Width, form.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm);
            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = form.Handle,
                IsWindowed = true,
            };

            D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out d3dDevice, out swapChain);
            d3dDeviceContext = d3dDevice.ImmediateContext;
            
            d3dDeviceContext.Rasterizer.SetViewport(0, 0, form.Width, form.Height);
            using (D3D11.Texture2D backBuffer = swapChain.GetBackBuffer<D3D11.Texture2D>(0))
            {
                renderTargetView = new D3D11.RenderTargetView(d3dDevice, backBuffer);
            }

            D3D11.BlendStateDescription blendStateDesc = D3D11.BlendStateDescription.Default();
            blendStateDesc.AlphaToCoverageEnable = false;
            blendStateDesc.RenderTarget[0].IsBlendEnabled = true;
            blendStateDesc.RenderTarget[0].SourceBlend = D3D11.BlendOption.SourceAlpha;
            blendStateDesc.RenderTarget[0].DestinationBlend = D3D11.BlendOption.One; //
            blendStateDesc.RenderTarget[0].BlendOperation = D3D11.BlendOperation.Maximum;
            blendStateDesc.RenderTarget[0].SourceAlphaBlend = D3D11.BlendOption.SourceAlpha; //Zero
            blendStateDesc.RenderTarget[0].DestinationAlphaBlend = D3D11.BlendOption.DestinationAlpha;
            blendStateDesc.RenderTarget[0].AlphaBlendOperation = D3D11.BlendOperation.Maximum;
            blendStateDesc.RenderTarget[0].RenderTargetWriteMask = D3D11.ColorWriteMaskFlags.All;
            blendState = new D3D11.BlendState(d3dDevice, blendStateDesc);

            GeometryBuffer = new GeometryBuffer(this);
            
            Fonts = new FontCache(this);

            var layout = new D3D11.InputElement[]
            {
                new D3D11.InputElement("POSITION", 0, Format.R32G32_Float, 0, 0),
                new D3D11.InputElement("COLOR", 0, Format.R32G32B32A32_Float, 8, 0),
                new D3D11.InputElement("TEXCOORDS", 0, Format.R32G32_Float, 24, 0),
            };

            var vertexShaderOutput = ShaderBytecode.Compile(vertexShaderCode, "main", "vs_4_0", ShaderFlags.Debug);
            var pixelShaderOutput = ShaderBytecode.Compile(pixelShaderCode, "main", "ps_4_0", ShaderFlags.Debug);

            vertexShader = new D3D11.VertexShader(Device, vertexShaderOutput);
            pixelShader = new D3D11.PixelShader(Device, pixelShaderOutput);

            var shaderSignature = ShaderSignature.GetInputSignature(vertexShaderOutput);

            inputLayout = new D3D11.InputLayout(Device, shaderSignature, layout);
            
            IntPtr data = System.Runtime.InteropServices.Marshal.AllocHGlobal(4 * 4 * 4);
            var white = BitConverter.GetBytes(1f);
            for (int i = 0; i < 4 * 4; i++)
            {
                for (int j = 0; j < white.Length; j++) 
                    System.Runtime.InteropServices.Marshal.WriteByte(data, i*sizeof(float)+j, white[j]);
            }
            
            White = new D3D11.Texture2D(Device, new D3D11.Texture2DDescription
            {
                Width = 4,
                Height = 4,
                ArraySize = 1,
                BindFlags = D3D11.BindFlags.ShaderResource,
                Usage = D3D11.ResourceUsage.Dynamic,
                CpuAccessFlags = D3D11.CpuAccessFlags.Write,
                Format = Format.R32G32B32A32_Float,
                MipLevels = 1,
                OptionFlags = D3D11.ResourceOptionFlags.None,
                SampleDescription = new DXGI.SampleDescription(1, 0),
            }, new DataBox[] { new DataBox(data, 4 * 2, 4) });

            System.Runtime.InteropServices.Marshal.FreeHGlobal(data);
            
            WhiteView = new D3D11.ShaderResourceView(Device, White);
            
            samplerState = new D3D11.SamplerState(Device, new D3D11.SamplerStateDescription()
            {
                Filter = D3D11.Filter.MinMagMipLinear,
                AddressU = D3D11.TextureAddressMode.Clamp,
                AddressV = D3D11.TextureAddressMode.Clamp,
                AddressW = D3D11.TextureAddressMode.Clamp,
                BorderColor = new RawColor4(1f, 0f, 1f, 1f),
                ComparisonFunction = D3D11.Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = 0,
                MaximumLod = 16
            });
            
            transfBuffer = new SharpDX.Direct3D11.Buffer(Device,
                new SharpDX.Direct3D11.BufferDescription(sizeof(float) * 4, SharpDX.Direct3D11.ResourceUsage.Dynamic, SharpDX.Direct3D11.BindFlags.ConstantBuffer,
                    SharpDX.Direct3D11.CpuAccessFlags.Write, SharpDX.Direct3D11.ResourceOptionFlags.None, sizeof(float)));

            DataStream stream;
            DeviceContext.MapSubresource(transfBuffer, D3D11.MapMode.WriteDiscard, D3D11.MapFlags.None, out stream);

            stream.Write(hViewportSize.Width);
            stream.Write(hViewportSize.Height);

            DeviceContext.UnmapSubresource(transfBuffer, 0);

            DeviceContext.VertexShader.SetShader(vertexShader, null, 0);
            DeviceContext.VertexShader.SetConstantBuffer(0, transfBuffer);
            DeviceContext.PixelShader.SetShader(pixelShader, null, 0);
            DeviceContext.PixelShader.SetSampler(0, samplerState);
            DeviceContext.InputAssembler.InputLayout = inputLayout;
            DeviceContext.OutputMerger.BlendState = blendState;
        }
        
        public void Clear(Color color)
        {
            if (!Initialized)
                return;

            d3dDeviceContext.OutputMerger.SetRenderTargets(renderTargetView);
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
            vertexShader.Dispose();
            pixelShader.Dispose();
            inputLayout.Dispose();
            renderTargetView.Dispose();
            swapChain.Dispose();
            d3dDevice.Dispose();
            d3dDeviceContext.Dispose();
            Fonts.Dispose();
        }
        #endregion

        #region RENDER FEATURES
        public Font CreateFont(string family, float height, char additonalRangeFrom, char additionalRangeTo, bool outline = false)
        {
            if (!Initialized) //Return dummy-font
                return new Font(null, family, height, false, false, outline, additonalRangeFrom, additionalRangeTo);

            //Return cached font
            var fnt = Fonts.GetFont(family, height);
            if (fnt != null)
                return fnt;

            //... or else create a new one
            return new Font(this, family, height, false, false, outline, additonalRangeFrom, additionalRangeTo);
        }

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
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.LineList);

            GeometryBuffer.Trim();
        }

        public void DrawLines(Color color, Vector2[] points)
        {
            if (!Initialized)
                return;

            var col = (RawColor4)color;

            points.ToList().ForEach(el => { GeometryBuffer.AppendVertex(new Vertex { Origin = el, Color = col }); });

            GeometryBuffer.SetupTexture(WhiteView);
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

                1,
                2,
                3,

                0,
                1,
                2

            );

            GeometryBuffer.SetupTexture(WhiteView);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.TriangleStrip);
            GeometryBuffer.Trim();
        }

        public void DrawRectangle(Color color, Vector2 location, Vector2 size)
        {
            if (!Initialized)
                return;

            var col = (RawColor4)color;

            GeometryBuffer.AppendVertices(
                new Vertex(location.X,          location.Y, col),

                new Vertex(location.X + size.X, location.Y, col),
                new Vertex(location.X,          location.Y + size.Y, col),

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

            GeometryBuffer.SetupTexture(WhiteView);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.LineList);
            GeometryBuffer.Trim();
        }

        public void DrawString(Color color, Font font, Vector2 location, string text)
        {
            if (!Initialized || font == null)
                return;

            if (font.IsDisposed)
            {
                font = Fonts[font];
                if (font == null || font.IsDisposed)
                    return;
            }
            
            font.DrawString(GeometryBuffer, location, (RawColor4)color, text);
        }

        public void DrawString(Color color, Font font, Vector2 location, string text, TextAlignment halign, TextAlignment valign)
        {
            if (!Initialized || font == null)
                return;

            if (font.IsDisposed)
            {
                font = Fonts[font];
                if (font == null || font.IsDisposed)
                    return;
            }
            
            font.DrawString(GeometryBuffer, location, (RawColor4)color, text, halign, valign);
        }

        public void Debug(Font font)
        {
            if (!Initialized || font == null)
                return;
            
            if (font.IsDisposed)
            {
                font = Fonts[font];
                if (font == null || font.IsDisposed)
                    return;
            }
            
            font.DrawString(GeometryBuffer, new Vector2(10f,10f), new RawColor4(1f,0f,1f,1f), 
                String.Format("Number of Vertices: {0}\nNumber of Indices {1}\nMemory pushend onto GPU: {2}",
                GeometryBuffer.Vertices, GeometryBuffer.Indices, GeometryBuffer.CopiedMemory));
        }
        #endregion
        #endregion
    }
}
