using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using ZatsHackBase.Maths;
using ZatsHackBase.UI;
using ZatsHackBase.Drawing;

namespace ZatsHackBase.UI.GUI.Controls
{

    public class Button : Control
    {
        public delegate void ButtonClickHandler(Button sender);

        public Button(ContainerControl parent) : base(parent)
        {
        }

        public override void Render(RenderEventArgs e)
        {
            /*
            graphics.FillRectangle ( m_idColors.Array [ m_iControlState ], m_rctAbsBounds );
	        graphics.DrawString ( m_pFont, m_colForeColor, m_szCaption, m_rctAbsBounds, Drawing::Center | Drawing::VCenter );
	        */
            //e.Graphics.FillRectangle(IndicationDescriptor.Get(ButtonState), AbsoluteBounds.Location, AbsoluteBounds.Size);

            var center = AbsoluteBounds.Location;
            center.X += AbsoluteBounds.Width/2;
            center.Y += AbsoluteBounds.Height/2;
            e.Graphics.DrawString(Color.Black, Font, center, Caption, TextAlignment.Center, TextAlignment.Center);


            base.Render(e);
        }

        //public override void MouseDown(MouseEventArgs e)
        //{
        //    if ( ButtonState == ControlState.Entered)
        //        ButtonState = ControlState.Pressed;
        //    base.MouseDown(e);
        //}

        public event ButtonClickHandler OnClick;
        //public override void MouseUp(MouseEventArgs e)
        //{
        //    if (ButtonState == ControlState.Pressed)
        //    {
        //        OnClick.Invoke(this);
        //        ButtonState = AbsoluteBounds.Contains(e.X,e.Y) ? ControlState.Entered : ControlState.Focused;

        //    }
        //    base.MouseUp(e);
        //}

        //public override void MouseMove(MouseEventArgs e)
        //{
        //    if ((ButtonState == ControlState.Default || ButtonState == ControlState.Focused) &&
        //        AbsoluteBounds.Contains(e.X, e.Y)) 
        //    {
        //        ButtonState = ControlState.Entered;
        //    }

        //    GainFocus = ButtonState == ControlState.Entered;
        //    LoseFocus = ButtonState != ControlState.Pressed && ButtonState != ControlState.Entered;

        //    base.MouseMove(e);
        //}

        //public override void KeyDown(KeyEventArgs e)
        //{
        //    if (IsFocused && e.KeyCode == Keys.Enter)
        //    {
        //        PrevState = ButtonState;
        //        ButtonState = ControlState.Pressed;
        //        PressedFromKey = true;
        //    }
        //    base.KeyDown(e);
        //}

        //public override void KeyUp(KeyEventArgs e)
        //{
        //    if (ButtonState == ControlState.Pressed && PressedFromKey == true && e.KeyCode == Keys.Enter)
        //    {
        //        OnClick.Invoke(this);
        //        ButtonState = PrevState;
        //        PressedFromKey = false;
        //    }
        //    base.KeyUp(e);
        //}

        public ControlState ButtonState;

        public IndicationDescriptor IdBackground;
        public IndicationDescriptor IdBorder;
        public IndicationDescriptor IdText;

        private bool PressedFromKey;
        private ControlState PrevState;

        private Font Font;

    }

}
