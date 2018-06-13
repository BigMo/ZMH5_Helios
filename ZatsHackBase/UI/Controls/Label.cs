using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Drawing;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI.Controls
{
    public class Label : Control
    {
        private bool recalcSize;

        public Label()
        {
            DrawBackground = DrawBorder = false;
            this.TextChanged += (s, e) => 
                recalcSize = true;
            this.FontChanged += (s, e) => 
                recalcSize = true;
        }

        public override void Draw(Graphics g)
        {
            if (Font != null && Font.IsInitialized && !string.IsNullOrEmpty(Text))
            {
                if (recalcSize && Font != null && Font.IsInitialized)
                {
                    Size = Font.MeasureString(Text);
                    recalcSize = false;
                }

                var pos = AbsolutePosition;

                switch (TextAlignment)
                {
                    case TextAlignment.Center:
                        pos = pos - Size * 0.5f;
                        break;
                    case TextAlignment.Right:
                        pos = pos - Vector2.UnitX * Width;
                        break;
                }

                g.DrawString(ForeColor, Font, pos, Text);
            }

            base.Draw(g);
        }
    }
}
