using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.UI.Controls.Layouts
{
    public interface ILayout
    {
        void ApplyLayout(Control parent);
    }
}
