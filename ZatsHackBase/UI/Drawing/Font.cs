using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.UI
{
    public class Font
    {
        
        #region Constructor
        public Font(Renderer renderer, string family, int height, bool bold, bool italy)
        {
            _Renderer = renderer;

            _Height = height;

            int texture_size = 0;

            int max_texture_width = 512;

            int texture_width = 0;
            int texture_height = 0;

            Bitmap bm = null;

            using (var font = new System.Drawing.Font(new FontFamily(family), height, FontStyle.Regular))
            {
                _Glyphs = new Dictionary<char, Glyph>();

                // static cfg - char padding
                int wide_pad = 4;
                int high_pad = 4;

                var graphics = Graphics.FromHwnd(IntPtr.Zero);

                int cur_width = 0;

                int cur_height = 0;

                int greatest_char = 0;

                for (int i = 32; i < 1000; ++i)
                {

                    char char_ = Convert.ToChar(i);

                    var size = graphics.MeasureString(char_.ToString(), font, new SizeF(100f, 100f),
                        StringFormat.GenericTypographic);

                    size.Width += wide_pad;
                    size.Height += high_pad;

                    if (size.Height > cur_height)
                        cur_height = (int)size.Height;

                    if (greatest_char < cur_height)
                        greatest_char = cur_height;

                    var n_w = cur_width + size.Width;

                    if (n_w > max_texture_width)
                    {
                        if (cur_width > texture_width)
                            texture_width = cur_width;
                        texture_height += cur_height;
                        cur_height = 0;
                        cur_width = 0;
                        continue;
                    }

                    cur_width = (int)n_w;
                }

                if (texture_width == 0) texture_width = cur_width;
                if (texture_height == 0) texture_height = cur_height;

                bm = new Bitmap(texture_width,texture_height);

                var bm_g = Graphics.FromImage(bm);

                cur_width = cur_height = 0;

                float x = 0;
                float y = 0;

                bm_g.Clear(System.Drawing.Color.Transparent);
                var brush = new SolidBrush(System.Drawing.Color.White);

                for (int i = 32; i < 1000; ++i)
                {

                    char char_ = Convert.ToChar(i);

                    var glyph = new Glyph();

                    var size = graphics.MeasureString(char_.ToString(), font, new SizeF(100f, 100f),
                        StringFormat.GenericTypographic);

                    if (size.Width + 1 + x > max_texture_width)
                    {
                        x = 0;
                        y += cur_height;
                        cur_height = 0;
                    }

                    bm_g.DrawString(char_.ToString(), font, brush, x, y);

                    glyph = new Glyph
                    {
                        Size = new SizeF(size) {Width = size.Width,Height = size.Height},
                        UV = new []
                        {
                            new Vector2(x / texture_width, y / texture_height),
                            new Vector2((x+size.Width+wide_pad) / texture_width, (y+size.Height) / texture_height),  
                        },
                        Code = char_
                    };

                    size.Width += wide_pad;
                    size.Height += high_pad;

                    x += size.Width;

                    if (size.Height > cur_height)
                        cur_height = (int)size.Height;
                    
                    _Glyphs.Add(char_, glyph);
                }

                bm.Save("test.png", ImageFormat.Png);

            }

            _Texture = new SharpDX.Direct3D11.Texture2D(_Renderer.Device, new Texture2DDescription
            {
                Width = texture_width,
                Height = texture_height,
                ArraySize = 1,
                BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                Usage = SharpDX.Direct3D11.ResourceUsage.Dynamic,
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.Write,
                Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                MipLevels = 1,
                OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
            }, (SharpDX.DataBox[])null);

            _Resource = new ShaderResourceView(_Renderer.Device, _Texture);
            
            DataStream stream;

            var databox = _Renderer.DeviceContext.MapSubresource(_Texture, 0, MapMode.WriteDiscard, MapFlags.None, out stream);

            var pitch = databox.RowPitch;
            var rows = texture_height;
            var stride = 4;

            for (int row = 0; row < rows; ++row)
            {

                for (int i = 0, x = 0; i < pitch; i += stride, ++x)
                {

                    int offset = (pitch*row) + i;

                    //if (offset > stream.Length)
                    //    break;

                    stream.Seek(offset, SeekOrigin.Begin);

                    if (x >= texture_width)
                        continue;

                    var color = bm.GetPixel(x, row);

                    if (color.A == 0 && color.R == 0 && color.G == 0 && color.B == 0)
                        continue;

                    //if (color.R < 254 && color.G < 254 && color.B < 254)
                    //    color = System.Drawing.Color.FromArgb(0,0,0,0);

                    if(color.R > 150 && color.G > 150 && color.B > 150)
                        color = System.Drawing.Color.FromArgb(255, 149, 149, 149);

                    stream.Write((byte)color.R);
                    stream.Write((byte)color.G);
                    stream.Write((byte)color.B);
                    stream.Write((byte)color.A);

                }

            }

            _Renderer.DeviceContext.UnmapSubresource(_Resource.Resource, 0);

            bm.Dispose();

            _SamplerState = new SamplerState(_Renderer.Device, new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                BorderColor = new RawColor4(1f,0f,1f,1f),
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = -float.MaxValue,
                MaximumLod = float.MaxValue
            });

        }
        #endregion

        #region Variables

        private Renderer _Renderer;

        private float _Height;
        private Dictionary<char, Glyph> _Glyphs;
        private SharpDX.Direct3D11.Texture2D _Texture;
        private SharpDX.Direct3D11.ShaderResourceView _Resource;
        private SharpDX.Direct3D11.SamplerState _SamplerState;

        #endregion

        #region Properties

        #endregion

        #region Method

        public void Debug(GeometryBuffer geometry_buffer)
        {
            var color = new RawColor4(1f,0f,0f,1f);
            geometry_buffer.AppendVertices(
                new Vertex(0f, 0f, color, 0f, 0f),
                new Vertex(_Texture.Description.Width, 0f, color, 1f, 0f),
                new Vertex(0f, _Texture.Description.Height, color, 0f, 1f),
                new Vertex(_Texture.Description.Width, _Texture.Description.Height, color, 1f, 1f)
                );

            geometry_buffer.AppendIndices(
                0,1,2,
                1,2,3
                );

            geometry_buffer.SetPrimitiveType(PrimitiveTopology.TriangleStrip);
            geometry_buffer.SetupTexture(_Resource, _SamplerState);
            geometry_buffer.Trim();
        }

        public void DrawString(GeometryBuffer geometry_buffer, Vector2 location, RawColor4 color, string text)
        {

            float wide_pos = location.X;
            float high_pos = location.Y;

            short vertex_offset = 0;

            var space = _Height * 0.35f;

            foreach (var c in text)
            {

                if (c == ' ')
                {
                    wide_pos += space;
                    continue;
                }

                if (c == '\n')
                {
                    wide_pos = location.X;
                    high_pos += _Height;
                    continue;
                }
                
                var glyph = _Glyphs[c];

                geometry_buffer.AppendVertices(
                    new Vertex(wide_pos, high_pos, color, glyph.UV[0].X, glyph.UV[0].Y),

                    new Vertex(wide_pos + glyph.Size.Width, high_pos, color, glyph.UV[1].X, glyph.UV[0].Y),

                    new Vertex(wide_pos, high_pos + glyph.Size.Height, color, glyph.UV[0].X, glyph.UV[1].Y),

                    new Vertex(wide_pos + glyph.Size.Width, high_pos + glyph.Size.Height, color, glyph.UV[1].X,
                        glyph.UV[1].Y)
                    );

                geometry_buffer.AppendIndices(
                    vertex_offset, (short)(vertex_offset + 1), (short)(vertex_offset + 2),
                    (short)(vertex_offset + 1), (short)(vertex_offset + 2), (short)(vertex_offset + 3)
                    );

                vertex_offset += 4;

                wide_pos += glyph.Size.Width;
            }

            geometry_buffer.SetPrimitiveType(PrimitiveTopology.TriangleStrip);
            geometry_buffer.SetupTexture(_Resource, _SamplerState);
            geometry_buffer.Trim();
            
        }

        public void DrawString(GeometryBuffer buffer, Vector2 location, RawColor4 color, string text, bool hcenter,
            bool vcenter = false)
        {
            
        }

        #endregion

    }
}
