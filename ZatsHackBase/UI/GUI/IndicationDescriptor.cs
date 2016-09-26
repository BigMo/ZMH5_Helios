using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;

namespace ZatsHackBase.GUI
{
    public class IndicationDescriptor
    {

        public IndicationDescriptor(
            RawColor4 default_,
            RawColor4 focused,
            RawColor4 entered,
            RawColor4 pressed,
            RawColor4 disabled
            )
        {
            Default = default_;
            Focused = focused;
            Entered = entered;
            Pressed = pressed;
            Disabled = disabled;
        }

        public RawColor4 Get(ControlState state)
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

        public RawColor4 Default;
        public RawColor4 Focused;
        public RawColor4 Entered;
        public RawColor4 Pressed;
        public RawColor4 Disabled;
    }
}
