using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3D11 = SharpDX.Direct3D11;

namespace ZatsHackBase.Drawing.Detail
{
    internal class Shaders
    {
        public static string pixelShaderCode =
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

        public static string vertexShaderCode2d =
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

        public static string vertexShaderCode3d =
                @"
                cbuffer ShaderParams : register(b0) 
                {
                    float4x4 g_viewMatrix;
                    float4x4 g_projMatrix;
                }

                struct vsinput
                {
                    float3 pos  : POSITION;
                    float4 col  : COLOR;
                };

                struct pselement
                {
                    float4 pos  : SV_POSITION;
                    float4 col  : COLOR;
                    float2 uv   : TEXCOORDS;
                };

                pselement main(vsinput vertex)
                {
                    pselement output;

                    float4 pos = float4(vertex.pos.xyz, 1.0f);
                    pos = mul(pos, g_viewMatrix);
                    pos = mul(pos, g_projMatrix);

                    output.pos      = pos;
                    output.col      = vertex.col;
                    output.uv       = float2(0.f,0.f);
                    return output;
                }
                ";

        public static D3D11.VertexShader VertexShader3D;
        public static D3D11.VertexShader VertexShader2D;
        public static D3D11.PixelShader PixelShader;
        public static D3D11.InputLayout InputLayout2D;
        public static D3D11.InputLayout InputLayout3D;
    }
}
