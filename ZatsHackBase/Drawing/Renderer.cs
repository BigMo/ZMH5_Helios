using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using SharpDX;
using System.Windows.Forms;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.D3DCompiler;
using SharpDX.Mathematics.Interop;

namespace ZatsHackBase.Drawing
{
    public class Renderer : IDisposable
    {
        #region VARIABLES
        private D3D11.Device d3dDevice;
        private D3D11.DeviceContext d3dDeviceContext;
        private DXGI.SwapChain swapChain;
        private D3D11.RenderTargetView renderTargetView;
        private D3D11.BlendState blendState;
        private D3D11.Buffer transfBuffer;
        private D3D11.VertexShader vertexShader;
        private D3D11.PixelShader pixelShader;
        private D3D11.InputLayout inputLayout;
        private D3D11.SamplerState samplerState;
        private List<Scene> scenes = new List<Scene>();
        private List<GeometryBuffer> gbufs = new List<GeometryBuffer>();
        private Camera cam;
        #endregion

        #region PROPERTIES
        public bool Initialized { get { return d3dDevice != null; } }
        public D3D11.Device Device => d3dDevice;
        public D3D11.DeviceContext DeviceContext => d3dDeviceContext;
        public D3D11.Texture2D White;
        public D3D11.ShaderResourceView WhiteView;
        public D3D11.SamplerState Sampler;
        public Size2F ViewportSize { get; set; }
        public Size2F hViewportSize { get; set; }
        public Detail.FontCache Fonts { get; private set; }
        public Camera Camera { get { return cam; } }
        #endregion

        #region METHODS
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
            
            Fonts = new Detail.FontCache(this);

            var layout2d = new D3D11.InputElement[]
            {
                new D3D11.InputElement("POSITION", 0, Format.R32G32_Float, 0, 0),
                new D3D11.InputElement("COLOR", 0, Format.R32G32B32A32_Float, 8, 0),
                new D3D11.InputElement("TEXCOORDS", 0, Format.R32G32_Float, 24, 0),
            };

            var layout3d = new D3D11.InputElement[]
            {
                new D3D11.InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new D3D11.InputElement("COLOR", 0, Format.R32G32B32A32_Float, 12, 0),
            };

            var vertexShaderOutput2d = ShaderBytecode.Compile(Detail.Shaders.vertexShaderCode2d, "main", "vs_4_0", ShaderFlags.Debug);
            var vertexShaderOutput3d = ShaderBytecode.Compile(Detail.Shaders.vertexShaderCode3d, "main", "vs_4_0", ShaderFlags.Debug);
            var pixelShaderOutput = ShaderBytecode.Compile(Detail.Shaders.pixelShaderCode, "main", "ps_4_0", ShaderFlags.Debug);

            Detail.Shaders.VertexShader2D = new D3D11.VertexShader(Device, vertexShaderOutput2d);
            Detail.Shaders.VertexShader3D = new D3D11.VertexShader(Device, vertexShaderOutput3d);
            Detail.Shaders.PixelShader = new D3D11.PixelShader(Device, pixelShaderOutput);
            
            Detail.Shaders.InputLayout2D = new D3D11.InputLayout(Device, ShaderSignature.GetInputSignature(vertexShaderOutput2d), layout2d);
            Detail.Shaders.InputLayout3D = new D3D11.InputLayout(Device, ShaderSignature.GetInputSignature(vertexShaderOutput3d), layout3d);
            
            IntPtr data = System.Runtime.InteropServices.Marshal.AllocHGlobal(4 * 4 * 4);
            var white = BitConverter.GetBytes(1f);
            for (int i = 0; i < 4 * 4; i++)
            {
                for (int j = 0; j < white.Length; j++)
                    System.Runtime.InteropServices.Marshal.WriteByte(data, i * sizeof(float) + j, white[j]);
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
                new D3D11.BufferDescription(sizeof(float) * 4, D3D11.ResourceUsage.Dynamic, D3D11.BindFlags.ConstantBuffer,
                    D3D11.CpuAccessFlags.Write, D3D11.ResourceOptionFlags.None, sizeof(float)));

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

            cam = new Camera(this);
        }

        public void Dispose()
        {

        }

        public Font MakeFont(Font font)
        {
            if (!Initialized) //Return dummy-font
                return new Font(null, font);

            //Return cached font
            var fnt = Fonts.GetFont(font.Family, font.Height);
            if (fnt != null)
                return fnt;

            //... or else create a new one
            return new Font(this, font);
        }

        public Scene MakeScene(int size = 1024 * 1024 * 10/*10mb*/)
        {
            var scene = new Scene(this, size);
            scenes.Add(scene);
            return scene;
        }

        public GeometryBuffer MakeGeometryBuffer(int size = 1024 * 1024 * 2/*2mb*/)
        {
            var gbuf = new GeometryBuffer(this, size);
            gbufs.Add(gbuf);
            return gbuf;
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

            cam.Update();

            DeviceContext.InputAssembler.InputLayout = Detail.Shaders.InputLayout3D;
            DeviceContext.VertexShader.SetShader(Detail.Shaders.VertexShader3D, null, 0);
            DeviceContext.VertexShader.SetConstantBuffer(0, cam.TransformBuffer);
            DeviceContext.PixelShader.SetShaderResource(0, WhiteView);

            foreach (var scene in scenes)
            {
                if(scene.Enabled)
                    scene.Draw();
            }
            
            DeviceContext.InputAssembler.InputLayout = Detail.Shaders.InputLayout2D;
            DeviceContext.VertexShader.SetShader(Detail.Shaders.VertexShader2D, null, 0);
            DeviceContext.VertexShader.SetConstantBuffer(0, transfBuffer);
            DeviceContext.PixelShader.SetShader(Detail.Shaders.PixelShader, null, 0);

            foreach (var gbuf in gbufs)
            {
                if (gbuf.Enabled)
                    gbuf.Draw();
            }

            swapChain.Present(1, PresentFlags.None);
        }
        #endregion

    }
}
