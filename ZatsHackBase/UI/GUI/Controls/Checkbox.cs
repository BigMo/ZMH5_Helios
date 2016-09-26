using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZatsHackBase.GUI.Controls
{

    public class Checkbox : Control
    {
        public delegate void ButtonClickHandler(Checkbox sender);

        public Checkbox(ContainerControl parent) : base(parent)
        {
        }

        public override void Render(RenderEventArgs e)
        {
            /*
            graphics.FillRectangle ( m_idBkColors.Array [ m_iControlState ], m_rctAbsBounds.Location, m_rctChkBounds.Size );
	        graphics.DrawRectangle ( m_idFgColors.Array [ m_iControlState ], m_rctAbsBounds.Location, m_rctChkBounds.Size );

	        if ( m_bChecked == True )
	        {
	        	int pts [] =
	        	{
	        		m_rctAbsBounds.Left + m_rctChkBounds.Right * 0.9111f,
	        		m_rctAbsBounds.Top + m_rctChkBounds.Bottom * 0.3,

	        		m_rctAbsBounds.Left + m_rctChkBounds.Right * 0.4f,
	        		m_rctAbsBounds.Top + m_rctChkBounds.Bottom * 0.8f,

	        		m_rctAbsBounds.Left + m_rctChkBounds.Right * 0.1999f,
	        		m_rctAbsBounds.Top + m_rctChkBounds.Bottom * 0.5333f,
	        	};

	        	graphics.DrawLine ( m_idFgColors.Array [ m_iControlState ], pts [ 0 ], pts [ 1 ], pts [ 2 ], pts [ 3 ] );
	        	graphics.DrawLine ( m_idFgColors.Array [ m_iControlState ], pts [ 2 ], pts [ 3 ], pts [ 4 ], pts [ 5 ] );
	        }

	        auto pt = m_rctAbsBounds.Location;
	        pt.Left += m_rctChkBounds.Right + 5.f;
	        graphics.DrawString ( m_pFont, m_idFgColors.Default, m_szCaption, pt, m_rctChkBounds.Size, Drawing::VCenter );

	                 *graphics.FillRectangle ( m_idBkColors.Array [ m_iControlState ], m_rctAbsBounds.Location, m_rctChkBounds.Size );
	        graphics.DrawRectangle ( m_idFgColors.Array [ m_iControlState ], m_rctAbsBounds.Location, m_rctChkBounds.Size );

	        if ( m_bChecked == True )
	        {
	        	int pts [] =
	        	{
	        		m_rctAbsBounds.Left + m_rctChkBounds.Right * 0.9111f,
	        		m_rctAbsBounds.Top + m_rctChkBounds.Bottom * 0.3,

	        		m_rctAbsBounds.Left + m_rctChkBounds.Right * 0.4f,
	        		m_rctAbsBounds.Top + m_rctChkBounds.Bottom * 0.8f,

	        		m_rctAbsBounds.Left + m_rctChkBounds.Right * 0.1999f,
	        		m_rctAbsBounds.Top + m_rctChkBounds.Bottom * 0.5333f,
	        	};

	        	graphics.DrawLine ( m_idFgColors.Array [ m_iControlState ], pts [ 0 ], pts [ 1 ], pts [ 2 ], pts [ 3 ] );
	        	graphics.DrawLine ( m_idFgColors.Array [ m_iControlState ], pts [ 2 ], pts [ 3 ], pts [ 4 ], pts [ 5 ] );
	        }

	        auto pt = m_rctAbsBounds.Location;
	        pt.Left += m_rctChkBounds.Right + 5.f;
	        graphics.DrawString ( m_pFont, m_idFgColors.Default, m_szCaption, pt, m_rctChkBounds.Size, Drawing::VCenter );
	         */
            base.Render(e);
        }

        public override void MouseDown(MouseEventArgs e)
        {
            if (ButtonState == ControlState.Entered)
                ButtonState = ControlState.Pressed;
            base.MouseDown(e);
        }

        public event ButtonClickHandler OnCheckedChanged;
        public override void MouseUp(MouseEventArgs e)
        {
            if (ButtonState == ControlState.Pressed)
            {
                Checked = !Checked;
                OnCheckedChanged.Invoke(this);
                ButtonState = AbsoluteBounds.Contains(e.X, e.Y) ? ControlState.Entered : ControlState.Focused;

            }
            base.MouseUp(e);
        }

        public override void MouseMove(MouseEventArgs e)
        {
            if ((ButtonState == ControlState.Default || ButtonState == ControlState.Focused) &&
                AbsoluteBounds.Contains(e.X, e.Y))
            {
                ButtonState = ControlState.Entered;
            }

            GainFocus = ButtonState == ControlState.Entered;
            LoseFocus = ButtonState != ControlState.Pressed && ButtonState != ControlState.Entered;

            base.MouseMove(e);
        }

        public override void KeyDown(KeyEventArgs e)
        {
            if (IsFocused && e.KeyCode == Keys.Enter)
            {
                PrevState = ButtonState;
                ButtonState = ControlState.Pressed;
                PressedFromKey = true;
            }
            base.KeyDown(e);
        }

        public override void KeyUp(KeyEventArgs e)
        {
            if (ButtonState == ControlState.Pressed && PressedFromKey == true && e.KeyCode == Keys.Enter)
            {
                OnCheckedChanged.Invoke(this);
                ButtonState = PrevState;
                PressedFromKey = false;
            }
            base.KeyUp(e);
        }

        public ControlState ButtonState;
        public IndicationDescriptor IndicationDescriptor;
        public bool Checked;

        private bool PressedFromKey;
        private ControlState PrevState;

    }

}
