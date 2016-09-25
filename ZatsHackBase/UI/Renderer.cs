using D3D11 = SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;
using SharpDX.D3DCompiler;
using ZatsHackBase.Maths;
using SharpDX;

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
        private D3D11.VertexShader vertexShader;
        private D3D11.PixelShader pixelShader;
        private ShaderSignature inputSignature;
        private D3D11.InputLayout inputLayout;
        private D3D11.InputElement[] inputElements = new D3D11.InputElement[] { new D3D11.InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };

        // Triangle vertices
        private Vector3[] vertices = new Vector3[] { new Vector3(-1f, 1f, 0.0f), new Vector3(1f, 1f, 0.0f), new Vector3(0.0f, -1f, 0.0f) };
        private D3D11.Buffer triangleVertexBuffer;
        #endregion

        #region PROPERTIES
        public bool Initialized { get { return d3dDevice != null; } }
        #endregion

        #region CONSTRUCTORS
        public Renderer()
        { }
        #endregion

        #region METHODS
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
            InitializeShaders();
            InitializeTriangle();
        }

        private void InitializeShaders()
        {
            // Compile the vertex shader code)
            using (var vertexShaderByteCode = ShaderBytecode.Compile(
                "float4 main(float4 position : POSITION) : SV_POSITION { return position; }", 
                "main", 
                "vs_4_0", 
                ShaderFlags.Debug))
            {
                // Read input signature from shader code
                inputSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

                vertexShader = new D3D11.VertexShader(d3dDevice, vertexShaderByteCode);
            }

            // Compile the pixel shader code
            using (var pixelShaderByteCode = ShaderBytecode.Compile(
                "float4 main(float4 position : SV_POSITION) : SV_TARGET { return float4(1.0, 0.0, 0.0, 1.0); }",
                "main", 
                "ps_4_0", 
                ShaderFlags.Debug))
            {
                pixelShader = new D3D11.PixelShader(d3dDevice, pixelShaderByteCode);
            }

            // Set as current vertex and pixel shaders
            d3dDeviceContext.VertexShader.Set(vertexShader);
            d3dDeviceContext.PixelShader.Set(pixelShader);

            d3dDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            // Create the input layout from the input signature and the input elements
            inputLayout = new D3D11.InputLayout(d3dDevice, inputSignature, inputElements);

            // Set input layout to use
            d3dDeviceContext.InputAssembler.InputLayout = inputLayout;
        }

        private void InitializeTriangle()
        {
            // Create a vertex buffer, and use our array with vertices as data
            triangleVertexBuffer = D3D11.Buffer.Create<Vector3>(d3dDevice, D3D11.BindFlags.VertexBuffer, vertices);
        }

        public void Clear(RawColor4 color)
        {
            if (!Initialized)
                return;

            d3dDeviceContext.OutputMerger.SetRenderTargets(renderTargetView);
            d3dDeviceContext.OutputMerger.SetBlendState(blendState, new RawColor4(0f, 0f, 0f, 0f), 0xFFFFFFFF);
            d3dDeviceContext.ClearRenderTargetView(renderTargetView, color);

            // Set vertex buffer
            d3dDeviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(triangleVertexBuffer, Utilities.SizeOf<Vector3>(), 0));

            // Draw the triangle
            d3dDeviceContext.Draw(vertices.Count(), 0);
        }

        public void Present()
        {
            if (!Initialized)
                return;

            swapChain.Present(1, PresentFlags.None);
        }

        public void Dispose()
        {
            inputLayout.Dispose();
            inputSignature.Dispose();
            triangleVertexBuffer.Dispose();
            vertexShader.Dispose();
            pixelShader.Dispose();
            renderTargetView.Dispose();
            swapChain.Dispose();
            d3dDevice.Dispose();
            d3dDeviceContext.Dispose();
        }
        #endregion
    }
}
