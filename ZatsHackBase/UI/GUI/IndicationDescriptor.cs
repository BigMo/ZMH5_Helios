using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Drawing;

namespace ZatsHackBase.GUI
{
    public class IndicationDescriptor
    {

        public IndicationDescriptor(
            Color default_,
            Color focused,
            Color entered,
            Color pressed,
            Color disabled
            )
        {
            Default = default_;
            Focused = focused;
            Entered = entered;
            Pressed = pressed;
            Disabled = disabled;
        }

        public Color Get(ControlState state)
        {
            switch (state)
            {
                case ControlState.Default:
                    return Default;
                case ControlState.Focused:
                    return Focused;
                case ControlState.Entered:
                    return Entered;
                case ControlState.Pressed:
                    return Pressed;
                case ControlState.Disabled:
                    return Disabled;
                default:
                    return Default;
            }
        }

        public Color Default;
        public Color Focused;
        public Color Entered;
        public Color Pressed;
        public Color Disabled;
    }
}
