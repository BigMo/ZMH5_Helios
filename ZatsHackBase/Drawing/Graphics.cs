using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace ZatsHackBase.Drawing
{
    public class Graphics
    {
        #region PROPERTIES
        public GeometryBuffer GeometryBuffer { get; set; }
        public Renderer Renderer { get { return GeometryBuffer.Renderer; } }
        #endregion

        #region CONSTRUCTOR
        public Graphics(GeometryBuffer geobuf)
        {
            GeometryBuffer = geobuf;
        }
        #endregion 

        #region FUNCTIONS

        public Font FindFont(Font f)
        {
            return Renderer.Fonts[f];
        }

        public void DrawLine(Color color, Vector2 from, Vector2 to)
        {
            var col = (RawColor4)color;
            GeometryBuffer.AppendVertices(
                new Detail.Vertex2D(from.X, from.Y, col),
                new Detail.Vertex2D(to.X, to.Y, col)
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
            var col = (RawColor4)color;

            points.ToList().ForEach(el => { GeometryBuffer.AppendVertex(new Detail.Vertex2D { Origin = el, Color = col }); });

            GeometryBuffer.SetupTexture(Renderer.WhiteView);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.LineList);
            GeometryBuffer.Trim();
        }

        public void FillRectangle(Color color, Vector2 location, Vector2 size)
        {
            var col = (RawColor4)color;

            GeometryBuffer.AppendVertices(
                new Detail.Vertex2D(location.X, location.Y, col),
                new Detail.Vertex2D(location.X + size.X, location.Y, col),
                new Detail.Vertex2D(location.X, location.Y + size.Y, col),
                new Detail.Vertex2D(location.X + size.X, location.Y + size.Y, col)
            );

            GeometryBuffer.AppendIndices(

                1,
                2,
                3,

                0,
                1,
                2

            );

            GeometryBuffer.SetupTexture(Renderer.WhiteView);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.TriangleStrip);
            GeometryBuffer.Trim();
        }

        public void DrawRectangle(Color color, Vector2 location, Vector2 size)
        {
            var col = (RawColor4)color;

            GeometryBuffer.AppendVertices(
                new Detail.Vertex2D(location.X, location.Y, col),

                new Detail.Vertex2D(location.X + size.X, location.Y, col),
                new Detail.Vertex2D(location.X, location.Y + size.Y, col),

                new Detail.Vertex2D(location.X + size.X, location.Y + size.Y, col)
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

            GeometryBuffer.SetupTexture(Renderer.WhiteView);
            GeometryBuffer.SetPrimitiveType(PrimitiveTopology.LineList);
            GeometryBuffer.Trim();
        }

        public void DrawString(Color color, Font font, Vector2 location, string text)
        {
            if (font == null)
                return;

            if (font.IsDisposed)
            {
                font = Renderer.Fonts[font];
                if (font == null || font.IsDisposed)
                    return;
            }

            font.DrawString(GeometryBuffer, location, (RawColor4)color, text);
        }

        public void DrawString(Color color, Font font, Vector2 location, string text, TextAlignment halign, TextAlignment valign)
        {
            if (font == null)
                return;

            if (font.IsDisposed)
            {
                font = Renderer.Fonts[font];
                if (font == null || font.IsDisposed)
                    return;
            }

            font.DrawString(GeometryBuffer, location, (RawColor4)color, text, halign, valign);
        }


        #endregion
    }
}
