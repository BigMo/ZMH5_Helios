using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Maths;
using Point = System.Drawing.Point;

namespace ZatsHackBase.UI.Drawing.FontHelpers
{
    internal struct PrimitiveColor
    {
        public byte a;
        public byte r;
        public byte g;
        public byte b;
    }
    public class GlyphAtlas : IDisposable
    {
        //TODO: fix character placement
        //TODO: http://jrgraphix.net/research/unicode_blocks.php <=> only add character ranges

        private const float rgb2f = 1f / 255f;

        #region CLASSES
        public class CharRange
        {
            public char From { get; private set; }
            public char To { get; private set; }
            public SizeF[] Sizes { get; private set; }
            public Vector2[] Locations { get; private set; }
            public char[] Chars { get; private set; }

            public CharRange(char from, char to)
            {
                From = from;
                To = to;
                Sizes = new SizeF[to - from];
                Locations = new Vector2[to - from];
                Chars = new char[to - from];
                for (int i = 0; i < Chars.Length; i++)
                    Chars[i] = (char)(from + i);
            }
        }
        #endregion

        #region VARIABLES
        private char[] chars;
        private SizeF[] sizes;
        private Vector2[] locations;
        private static StringFormat stringFormat = new StringFormat {Alignment = StringAlignment.Near, FormatFlags = StringFormatFlags.NoWrap, HotkeyPrefix = HotkeyPrefix.None, Trimming = StringTrimming.Word};
        #endregion

        #region PROPERTIES
        public ShaderResourceView Resource { get; private set; }
        public Texture2D Texture { get; private set; }
        public Bitmap Image { get; private set; }
        public Dictionary<char, Glyph> Glyphs { get; private set; }
        public Vector2 Padding { get; private set; }

        public bool IsDisposed { get; private set; }
        public bool Outline { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public GlyphAtlas(CharRange[] ranges, Vector2 padding, bool outlined = false)
        {
            Outline = outlined;
            Padding = padding;
            Glyphs = new Dictionary<char, Glyph>();
            chars = ranges.SelectMany(x => x.Chars).ToArray();
            locations = new Vector2[chars.Length];
            sizes = new SizeF[chars.Length];
            
            Resource = null;
            Texture = null;
            Image = null;
            IsDisposed = true;
        }
        #endregion

        #region METHODS
        public void InitDebug(string family, float height)
        {
            using (System.Drawing.Font fnt = new System.Drawing.Font(family, height))
            {
                PrepareTextureAtlas(fnt);
                DrawTextureAtlas(fnt);
            }
        }
        public void Init(Renderer renderer, System.Drawing.Font font, int maxTextureWidth = 1024)
        {
            PrepareTextureAtlas(font, maxTextureWidth);
            DrawTextureAtlas(font);
            CreateTexture(renderer);

            IsDisposed = false;
        }
        private void PrepareTextureAtlas(System.Drawing.Font font, int maxTextureWidth = 1024)
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                int curWidth = 0, curHeight = 0;
                int texWidth = 0, texHeight = 0;
                int grChar = 0;
                SizeF msSize = new SizeF(100f, 100f);
                for (int i = 0; i < chars.Length; i++)
                {
                    var size = g.MeasureString(chars[i].ToString(), font, msSize, StringFormat.GenericDefault);

                    if (size.Width < 0)
                        size.Width = -size.Width;

                    if (size.Height < 0)
                        size.Height = -size.Height;

                    sizes[i] = size;

                    //TODO: Experimentell
                    size.Width = (int)(size.Width * 1.3f);// Padding.X;
                    size.Height = (int)(size.Height + Padding.Y);

                    curHeight = System.Math.Max(curHeight, (int)size.Height);
                    grChar = System.Math.Max(grChar, (int)size.Height);

                    var newWidth = curWidth + size.Width;
                    if (newWidth > maxTextureWidth)
                    {
                        texWidth = System.Math.Max(curWidth, texWidth);
                        texHeight += curHeight;
                        curHeight = 0;
                        curWidth = 0;
                        newWidth = size.Width;
                    }

                    locations[i] = new Vector2(curWidth, texHeight);
                    curWidth = (int)newWidth;

                }
                texHeight += curHeight;

                Image = new Bitmap(texWidth, texHeight, PixelFormat.Format32bppArgb);
            }
        }
        private void DrawTextureAtlas(System.Drawing.Font font)
        {
            using (var g = Graphics.FromImage(Image))
            {
                g.Clear(System.Drawing.Color.Transparent);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                var brString = new SolidBrush(System.Drawing.Color.White);
                var brOutline = new SolidBrush(System.Drawing.Color.Black);

                for (int i = 0; i < chars.Length; i++)
                {
                    if (Glyphs.ContainsKey(chars[i]))
                        continue;

                    var x = locations[i].X;
                    var y = locations[i].Y;
                    var size = sizes[i];

                    var c = chars[i].ToString();

                    if (Outline)
                    {
                        g.DrawString(c, font, brOutline, x - 1, y, StringFormat.GenericTypographic);
                        g.DrawString(c, font, brOutline, x + 1, y, StringFormat.GenericTypographic);
                        g.DrawString(c, font, brOutline, x, y - 1, StringFormat.GenericTypographic);
                        g.DrawString(c, font, brOutline, x, y + 1, StringFormat.GenericTypographic);
                    }
                    g.DrawString(c, font, brString, x, y, StringFormat.GenericTypographic);
                    
                    var glyph = new Glyph
                    {
                        Size = size,
                        UV = new[]
                        {
                            new Vector2(x / Image.Width, y / Image.Height),
                            new Vector2(
                                (x + size.Width) / Image.Width,
                                (y + size.Height) / Image.Height
                            ),
                        },
                        Code = chars[i]
                    };

                    Glyphs.Add(chars[i], glyph);
                }

                brString.Dispose();
                brOutline.Dispose();

            }

            Image.Save(string.Format("{0} {1}.png", font.FontFamily.Name, (int)font.Size), ImageFormat.Png);
        }
        private void CreateTexture(Renderer renderer)
        {
            Texture = new Texture2D(renderer.Device, new Texture2DDescription
            {
                Width = Image.Width,
                Height = Image.Height,
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

            Resource = new ShaderResourceView(renderer.Device, Texture);

            var databox = renderer.DeviceContext.MapSubresource(Texture, 0, MapMode.WriteDiscard, MapFlags.None, out stream);

            unsafe
            {
                BitmapData data = Image.LockBits(new System.Drawing.Rectangle(0, 0, Image.Width, Image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                byte* scan0 = (byte*)data.Scan0.ToPointer();

                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0, i = 0; x < data.Width; x++, i += databox.RowPitch)
                    {
                        PrimitiveColor* dat = (PrimitiveColor*)(scan0 + y * data.Stride + x * 4);

                        stream.Seek((databox.RowPitch * y) + (x * sizeof(int) * sizeof(float)), SeekOrigin.Begin);

                        stream.Write(rgb2f * dat->a);
                        stream.Write(rgb2f * dat->r);
                        stream.Write(rgb2f * dat->g);
                        stream.Write(rgb2f * dat->b);
                    }
                }
                Image.UnlockBits(data);
            }
            renderer.DeviceContext.UnmapSubresource(Texture, 0);
        }
        public void Dispose()
        {
            if (!IsDisposed)
            {
                Resource?.Dispose();
                Texture?.Dispose();
                Glyphs?.Clear();
                Image?.Dispose();
                
                Resource = null;
                Texture = null;
                Image = null;

                IsDisposed = true;
            }
        }
        #endregion
    }
}
