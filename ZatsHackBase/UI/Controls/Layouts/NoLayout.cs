using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.UI.Controls.Layouts
{
    public sealed class NoLayout : ILayout
    {
        public static NoLayout Instance { get; } = new NoLayout();
        public void ApplyLayout(Control parent) { }
    }
}
