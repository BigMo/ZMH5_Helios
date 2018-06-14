using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI.GUI
{
    public class LocationChangeEventArgs : EventArgs
    {
        public Control Target;
        public Point AbsoluteLocation;
    }
}
