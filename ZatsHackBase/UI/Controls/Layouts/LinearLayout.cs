using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.UI.Controls.Layouts
{
    public class LinearLayout : ILayout
    {
        public static LinearLayout Instance { get; } = new LinearLayout();

        public void ApplyLayout(Control parent)
        {
            if (!parent.IsVisible || !parent.Children.Any())
                return;

            var filtered = parent.Children.Where(x => x.Visible);
            var newY = parent.Padding.Top;
            var newX = parent.Padding.Left;

            foreach (var child in filtered)
            {
                child.Position = new Maths.Vector2(newY + child.Margin.Top, newX + child.Margin.Left);
                newY = child.Position.Y + child.Margin.Bottom;
            }

            var lowest = filtered.Max(x => x.Bottom + x.Margin.Bottom) + parent.Padding.Bottom;
            if (lowest > parent.Bottom)
                parent.Height = lowest;

            var width = filtered.Max(x => x.Right + x.Margin.Right) + parent.Padding.Right;
            if (width > parent.Right)
                parent.Width = width;
        }
    }
}
