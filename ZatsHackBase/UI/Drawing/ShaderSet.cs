using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace ZatsHackBase.UI.Drawing
{
    public class ShaderSet
    {
        #region Constructors

        public ShaderSet(Renderer renderer, string code, string vertex_entry, string pixel_entry,
            SharpDX.Direct3D11.InputElement[] layout)
        {
            _Renderer = renderer;

            var vertexShaderOutput = ShaderBytecode.Compile(code, vertex_entry, "vs_4_0", ShaderFlags.Debug);
            var pixelShaderOutput = ShaderBytecode.Compile(code, pixel_entry, "ps_4_0", ShaderFlags.Debug);

            _VertexShader = new VertexShader(_Renderer.Device, vertexShaderOutput);
            _PixelShader = new PixelShader(_Renderer.Device, pixelShaderOutput);

            var shaderSignature = ShaderSignature.GetInputSignature(vertexShaderOutput);

            _InputLayout = new InputLayout(_Renderer.Device, shaderSignature, layout);
        }

        #endregion

        #region Variables

        private readonly Renderer _Renderer;
        private readonly SharpDX.Direct3D11.VertexShader _VertexShader;
        private readonly SharpDX.Direct3D11.PixelShader _PixelShader;
        private readonly SharpDX.Direct3D11.InputLayout _InputLayout;

        #endregion

        #region Methods

        public void Apply()
        {

            _Renderer.DeviceContext.VertexShader.Set(_VertexShader);
            _Renderer.DeviceContext.PixelShader.Set(_PixelShader);
            _Renderer.DeviceContext.InputAssembler.InputLayout = _InputLayout;

        }

        #endregion
    }
}
