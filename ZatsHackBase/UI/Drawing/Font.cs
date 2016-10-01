using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;
using InterpolationMode = System.Drawing.Drawing2D.InterpolationMode;

namespace ZatsHackBase.UI
{
    public class Font : IDisposable
    {

        private const float rgb2f = 1f / 255f;

        #region Constructor
        internal Font(Renderer renderer, string family, float height, bool bold, bool italy)
        {
            ID = ++idCounter;
            _Renderer = renderer;
            Height = height;
            Family = family;
            IsDisposed = true;

            //Make this a dummy font in case renderer isn't ready yet
            if (renderer == null || !renderer.Initialized)
                return;

            //Renderer is ready; Init
            Init();
            IsDisposed = false;
        }
        #endregion

        #region Variables

        private Renderer _Renderer;
        
        private Dictionary<char, Glyph> _Glyphs;
        private SharpDX.Direct3D11.Texture2D _Texture;
        private SharpDX.Direct3D11.ShaderResourceView _Resource;
        private SharpDX.Direct3D11.SamplerState _SamplerState;
        private static int idCounter;
        #endregion

        #region Properties

        public string Family { get; private set; }
        public float Height { get; private set; }
        public int ID { get; private set; }
        public bool IsDisposed { get; private set; }
        #endregion

        #region Method
        private void Init()
        {
            int texture_size = 0;
            int max_texture_width = 1024;
            int texture_width = 0;
            int texture_height = 0;

            Bitmap bm = null;

            using (var font = new System.Drawing.Font(new FontFamily(Family), Height, FontStyle.Regular))
            {
                _Glyphs = new Dictionary<char, Glyph>();

                IsDisposed = false;
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

                bm = new Bitmap(texture_width, texture_height);

                var bm_g = Graphics.FromImage(bm);

                cur_width = cur_height = 0;

                float x = 0;
                float y = 0;

                bm_g.Clear(System.Drawing.Color.Transparent);
                bm_g.SmoothingMode = SmoothingMode.HighQuality;
                bm_g.TextRenderingHint = TextRenderingHint.;

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
                        Size = new SizeF(size) { Width = size.Width, Height = size.Height },
                        UV = new[]
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
                Format = SharpDX.DXGI.Format.R32G32B32A32_Float,
                MipLevels = 1,
                OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
            }, (SharpDX.DataBox[])null);

            DataStream stream;

            _Resource = new ShaderResourceView(_Renderer.Device, _Texture);

            var databox = _Renderer.DeviceContext.MapSubresource(_Texture, 0, MapMode.WriteDiscard, MapFlags.None, out stream);

            var pitch = databox.RowPitch;
            var rows = texture_height;
            var stride = sizeof(float) * 4;

            using (var bm2 = new Bitmap(texture_width,texture_height))
            {

                    for (int row = 0; row < rows; ++row)
                    {

                        for (int i = 0, x = 0; i < pitch; i += stride, ++x)
                        {

                            int offset = (pitch * row) + i;

                            stream.Seek(offset, SeekOrigin.Begin);

                            if (x >= texture_width)
                                continue;

                            var color = bm.GetPixel(x, row);

                            if (color.A == 0 || (color.R == 1 && color.B == 1 && color.G == 1))
                            {

                                stream.Write(rgb2f * color.R);
                                stream.Write(rgb2f * color.G);
                                stream.Write(rgb2f * color.B);
                                stream.Write(rgb2f * color.A);

                                //bm2.SetPixel(x,row, color);
                            }
                            else
                            {
                                var alpha = (color.R + color.G + color.B) / 3f * rgb2f;
                                stream.Write(1f);
                                stream.Write(1f);
                                stream.Write(1f);
                                stream.Write(alpha);
                            //bm2.SetPixel(x, row, System.Drawing.Color.FromArgb((int)(255f * alpha), System.Drawing.Color.White));
                        }
                        }

                    
                }
                    //bm2.Save("test2.png", ImageFormat.Png);
            }

            _Renderer.DeviceContext.UnmapSubresource(_Texture, 0);

            bm.Dispose();

            _SamplerState = new SamplerState(_Renderer.Device, new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                BorderColor = new RawColor4(1f, 0f, 1f, 1f),
                ComparisonFunction = Comparison.Never,
                MaximumAnisotropy = 16,
                MipLodBias = 0,
                MinimumLod = 0,
                MaximumLod = 16
            });
        }

        public void DrawString(GeometryBuffer geometry_buffer, Vector2 location, RawColor4 color, string text)
        {
            if (IsDisposed)
                return;

            float wide_pos = location.X;
            float high_pos = location.Y;
            float highest_char = 0f;

            short vertex_offset = 0;

            var space = Height * 0.35f;

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
                    high_pos += highest_char == 0f ? Height : highest_char;
                    highest_char = 0f;
                    continue;
                }

                var glyph = _Glyphs[c];

                if (glyph.Size.Height > highest_char)
                    highest_char = glyph.Size.Height;

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

        public Vector2 MeasureString(string text)
        {
            float height = 0f;
            int lines = 0;
            var space = Height * 0.35f;


            float width = 0f;
            float cur_width = 0f;
            float highest_char = 0f;

            foreach (var c in text)
            {
                if (c == ' ')
                {
                    cur_width += space;
                    continue;
                }
                if (c == '\n')
                {
                    height += highest_char == 0f ? Height : highest_char;
                    if (cur_width > width)
                        width = cur_width;
                    cur_width = 0f;
                    highest_char = 0f;
                    continue;
                }

                var glyph = _Glyphs[c];

                width += glyph.Size.Width;

                if (glyph.Size.Height > highest_char)
                    highest_char = glyph.Size.Height;
            }

            if (lines == 0)
                height = Height;

            return new Vector2(width,height);
        }

        public void MeasureString(string text, out List<float> line_widths, out float height)
        {
            line_widths = new List<float>();
            height = 0f;

            var space = Height * 0.35f;

            float width = 0f;
            float highest_char = 0f;
            if (IsDisposed)
                return;

            foreach (var c in text)
            {
                if (c == ' ')
                {
                    width += space;
                    continue;
                }

                if (c == '\n')
                {
                    line_widths.Add(width);
                    height += highest_char == 0f ? Height : highest_char;
                    width = 0f;
                }

                var glyph = _Glyphs[c];

                width += glyph.Size.Width;

                if (glyph.Size.Height > highest_char)
                    highest_char = glyph.Size.Height;
            }

            if (line_widths.Count == 0 && text.Length != 0)
            {
                line_widths.Add(width);
                height += highest_char;
            }
        }

        public void DrawString(GeometryBuffer geometry_buffer, Vector2 location, RawColor4 color, string text,
            TextAlignment halign = TextAlignment.Near, TextAlignment valign = TextAlignment.Near)
        {
            if (IsDisposed)
                return;

            List<float> line_widths;
            float height;

            MeasureString(text, out line_widths, out height);

            if (line_widths.Count == 0 && text.Length == 0)
                return;

            float wide = 0f;
            float space = Height*0.35f;
            float highest_char = 0f;

            short vertex_offset = 0;

            switch (halign)
            {
                case TextAlignment.Center:
                    wide = location.X - line_widths[0] / 2;
                    break;
                case TextAlignment.Far:
                    wide = location.X - line_widths[0];
                    break;
                default:
                    wide = location.X;
                    break;
            }

            switch (valign)
            {
                case TextAlignment.Center:
                    break;
                    location.Y -= height / 2;
                case TextAlignment.Far:
                    location.Y -= height;
                    break;
            }

            foreach (var c in text)
            {
                if (c == ' ')
                {
                    wide += space;
                    continue;
                }

                if (c == '\n')
                {
                    height += highest_char == 0f ? Height : highest_char;

                    switch (halign)
                    {
                        case TextAlignment.Center:
                            wide = location.X - line_widths[0] / 2;
                            break;
                        case TextAlignment.Far:
                            wide = location.X - line_widths[0];
                            break;
                        default:
                            wide = location.X;
                            break;
                    }
                }

                var glyph = _Glyphs[c];

                geometry_buffer.AppendVertices(
                    new Vertex(wide, location.Y, color, glyph.UV[0].X, glyph.UV[0].Y),

                    new Vertex(wide + glyph.Size.Width, location.Y, color, glyph.UV[1].X, glyph.UV[0].Y),

                    new Vertex(wide, location.Y + glyph.Size.Height, color, glyph.UV[0].X, glyph.UV[1].Y),

                    new Vertex(wide + glyph.Size.Width, location.Y + glyph.Size.Height, color, glyph.UV[1].X,
                        glyph.UV[1].Y)
                    );

                geometry_buffer.AppendIndices(
                    vertex_offset, (short)(vertex_offset + 1), (short)(vertex_offset + 2),
                    (short)(vertex_offset + 1), (short)(vertex_offset + 2), (short)(vertex_offset + 3)
                    );

                vertex_offset += 4;

                wide += glyph.Size.Width;

                if (glyph.Size.Height > highest_char)
                    highest_char = glyph.Size.Height;

            }

            geometry_buffer.SetPrimitiveType(PrimitiveTopology.TriangleStrip);
            geometry_buffer.SetupTexture(_Resource, _SamplerState);
            geometry_buffer.Trim();
        }

        #endregion

        public void Dispose()
        {
            IsDisposed = true;
            _Resource?.Dispose();
            _Texture?.Dispose();
            _Glyphs?.Clear();
        }
    }
}
