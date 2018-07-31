using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Drawing;

namespace ZatsHackBase.UI.GUI
{
    public class IndicationDescriptor
    {

        public IndicationDescriptor(
            Color active,// = Color.White,
            Color inactive,// = Color.White,
            Color highlight,// = Color.White,
            Color pressed// = Color.White
            )
        {
            Active = active;
            Inactive = inactive;
            Highlight = highlight;
            Pressed = pressed;
        }

        public Color Get(ControlState state)
        {
            switch (state)
            {
                case ControlState.Active:
                    return Active;
                case ControlState.Inactive:
                    return Inactive;
                case ControlState.Highlight:
                    return Highlight;
                case ControlState.Pressed:
                    return Pressed;
                default:
                    return Active;
            }
        }

        public Color Active;
        public Color Inactive;
        public Color Highlight;
        public Color Pressed;
    }
}
