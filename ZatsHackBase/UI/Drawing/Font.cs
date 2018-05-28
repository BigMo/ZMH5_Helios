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
using System.Linq;
using ZatsHackBase.UI.Drawing.FontHelpers;

namespace ZatsHackBase.UI
{
    public class Font : IDisposable
    {

        #region Constructor
        public static Font CreateDummy(string family, float height, bool outlined = false, bool bold = false, bool italic = false, char additionRangeFrom = (char)0, char additionalRangeTo = (char)0)
        {
            return new Font(null, family, height, outlined, bold, italic, additionRangeFrom, additionalRangeTo);
        }
        internal Font(Renderer renderer, Font other) : this(renderer, other.Family, other.Height, other.Outlined, other.Bold, other.Italic, other.AdditionalRangeFrom, other.AdditionalRangeTo) { }
        internal Font(Renderer renderer, string family, float height, bool outlined, bool bold, bool italic, char additionRangeFrom = (char)0, char additionalRangeTo = (char)0)
        {
            AdditionalRangeFrom = additionRangeFrom;
            AdditionalRangeTo = additionalRangeTo;
            Outlined = outlined;
            Bold = bold;
            Italic = italic;

            var ranges = new List<GlyphAtlas.CharRange>(new GlyphAtlas.CharRange[]{
                    new GlyphAtlas.CharRange((char)32, (char)1000), //Basic
                    new GlyphAtlas.CharRange((char)0x0400, (char)0x04ff), //Cyrillic
                    new GlyphAtlas.CharRange((char)0x0500, (char)0x052f), //Cyrillic Supplementary
                    new GlyphAtlas.CharRange((char)0x02b0, (char)0x02ff), //Block Elements
                    new GlyphAtlas.CharRange((char)0x2580, (char)0x259f), //Block Elements
                    new GlyphAtlas.CharRange((char)0x25A0, (char)0x25ff) //Geometric Shapes
                });
            if (additionRangeFrom != 0 && additionalRangeTo != 0)
                ranges.Add(new GlyphAtlas.CharRange(additionRangeFrom, additionalRangeTo));

            ID = ++idCounter;
            _Renderer = renderer;
            Height = height;
            Family = family;
            IsDisposed = true;
            atlas = new GlyphAtlas(
                ranges.ToArray(), 
                new Vector2(height * 0.75f, height * (1f / 3f)), outlined);

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

        private GlyphAtlas atlas;
        private static int idCounter;
        #endregion

        #region Properties

        public string Family { get; private set; }
        public float Height { get; private set; }
        public int ID { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsInitialized { get { return _Renderer != null; } }
        public char AdditionalRangeFrom { get; private set; }
        public char AdditionalRangeTo { get; private set; }
        public bool Outlined { get; private set; }
        public bool Bold { get; private set; }
        public bool Italic { get; private set; }
        #endregion

        #region Method
        private void Init()
        {
            var font = new System.Drawing.Font(new FontFamily(Family), Height, FontStyle.Regular,GraphicsUnit.Pixel);

            atlas.Init(_Renderer, font);

            font.Dispose();
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

                Glyph glyph;
                if (!atlas.Glyphs.ContainsKey(c))
                    glyph = atlas.Glyphs['?'];
                else
                    glyph = atlas.Glyphs[c];

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
            geometry_buffer.SetupTexture(atlas.Resource);
            geometry_buffer.Trim();

        }

        public Vector2 MeasureString(string text)
        {
            if (IsDisposed || string.IsNullOrEmpty(text))
                return Vector2.Zero;

            List<float> line_widths = new List<float>();
            float height = 0f;

            MeasureString(text, out line_widths, out height);

            return new Vector2(line_widths.Max(x => x), height);
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

                Glyph glyph;
                if (atlas.Glyphs.ContainsKey(c))
                    glyph = atlas.Glyphs[c];
                else
                    glyph = atlas.Glyphs['?'];

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
            float space = Height * 0.35f;
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

                Glyph glyph;
                if(atlas.Glyphs.ContainsKey(c))
                    glyph = atlas.Glyphs[c];
                else
                    glyph = atlas.Glyphs['?'];

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
            geometry_buffer.SetupTexture(atlas.Resource);
            geometry_buffer.Trim();
        }

        public void Debug(GeometryBuffer buf)
        {
            var color = new RawColor4(1f,1f,1f,1f);
            buf.AppendVertices(
                new Vertex(0f, 0f, color, 0f, 0f),
                new Vertex(1024, 0f, color, 1f, 0f),
                new Vertex(0f, 512, color, 0f, 1f),
                new Vertex(1024, 512, color, 1f, 1f));
            buf.AppendIndices(0, 1, 2, 1, 2, 3);
            buf.SetupTexture(atlas.Resource);
            buf.Trim();
        }

        #endregion

        public void Dispose()
        {
            IsDisposed = true;

            if (!atlas.IsDisposed)
                atlas.Dispose();
        }
    }
}
