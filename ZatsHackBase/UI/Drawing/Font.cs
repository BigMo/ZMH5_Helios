using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.UI
{
    class Font
    {
        
        #region Constructor
        public Font(Renderer renderer, string family, int height, bool bold, bool italy)
        {
            _Renderer = renderer;

            int texture_size = 0;

            int max_texture_width = 512;

            int texture_width = 0;
            int texture_height = 0;

            Bitmap bm = null;

            using (var font = new System.Drawing.Font(new FontFamily(family), height, FontStyle.Bold))
            {
                _Glyphs = new Dictionary<char, Glyph>();

                // static cfg - char padding
                int wide_pad = 1;
                int high_pad = 1;

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
                            new Vector2((x+size.Width) / texture_width, (y+size.Height) / texture_height),  
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

            _Renderer.DeviceContext.MapSubresource(_Resource.Resource, 0, MapMode.WriteDiscard, MapFlags.None, out stream);

            int pitch = texture_width*(sizeof (float)*4);

            int data_ptr = texture_height * pitch;
            int x_ = 0, y_ = 0;
            while (data_ptr > 0)
            {

                stream.Seek(data_ptr, SeekOrigin.Begin);
                x_ = 0;
                for (int i = 0; i < texture_width;++i)
                {

                    var color = bm.GetPixel(x_, y_);

                    stream.Write( new RawColor4(
                        (float)(color.R) / 255f,
                        (float)(color.G) / 255f,
                        (float)(color.B) / 255f,
                        (float)(color.A) / 255f
                        ));

                    x_++;
                }

                data_ptr -= pitch;
                y_++;
            }

            _Renderer.DeviceContext.UnmapSubresource(_Resource.Resource, 0);

            bm.Dispose();
            
        }
        #endregion

        #region Variables

        private Renderer _Renderer;

        private Dictionary<char, Glyph> _Glyphs;
        private SharpDX.Direct3D11.Texture2D _Texture;
        private SharpDX.Direct3D11.ShaderResourceView _Resource;

        #endregion

        #region Properties

        #endregion

    }
}
