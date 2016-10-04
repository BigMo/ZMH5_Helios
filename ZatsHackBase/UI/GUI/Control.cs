using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Mathematics.Interop;
using ZatsHackBase.Maths;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.GUI
{
    public class Control
    {

        public delegate void RenderEventHandler(object sender, RenderEventArgs e);
        public delegate void LocationChangeEventHandler(object sender, LocationChangeEventArgs e);

        public Control(ContainerControl parent)
        {
            Parent = parent;
            
        }

        public EventHandler<RenderEventArgs> OnRender;
        public virtual void Render(RenderEventArgs e)
        {
            OnRender.Invoke(this, e);
        }

        public EventHandler<MouseEventArgs> OnMouseDown;
        public virtual void MouseDown(MouseEventArgs e)
        {
            OnMouseDown.Invoke(this, e);
        }

        public EventHandler<MouseEventArgs> OnMouseUp;
        public virtual void MouseUp(MouseEventArgs e)
        {
            OnMouseUp.Invoke(this, e);
        }

        public EventHandler<MouseEventArgs> OnMouseMove;
        public virtual void MouseMove(MouseEventArgs e)
        {
            OnMouseMove.Invoke(this, e);
        }

        public EventHandler<MouseEventArgs> OnMouseScroll;
        public virtual void MouseScroll(MouseEventArgs e)
        {
            OnMouseScroll.Invoke(this, e);
        }

        public EventHandler<KeyEventArgs> OnKeyDown;
        public virtual void KeyDown(KeyEventArgs e)
        {
            OnKeyDown.Invoke(this, e);
        }

        public EventHandler<KeyEventArgs> OnKeyUp;
        public virtual void KeyUp(KeyEventArgs e)
        {
            OnKeyUp.Invoke(this, e);
        }

        public EventHandler<LocationChangeEventArgs> OnParentAbsoluteLocationChanged;
        public virtual void ParentAbsoluteLocationChanged(LocationChangeEventArgs e)
        {

            AbsoluteBounds.Location = e.AbsoluteLocation.Vec;
            AbsoluteBounds.Left += Bounds.Left; ;
            AbsoluteBounds.Top += Bounds.Top;

            if (OverrideLayout == true)
            {
                AbsoluteBounds.Left += LayoutChange.Left;
                AbsoluteBounds.Top += LayoutChange.Top;
                AbsoluteBounds.Width = Bounds.Width;
                AbsoluteBounds.Height = Bounds.Height;
            }
            else
            {
                AbsoluteBounds.Width = LayoutSize.Width;
                AbsoluteBounds.Height = LayoutSize.Height;
            }

            OnParentAbsoluteLocationChanged.Invoke(this, e);
        }

        #region Variables/Properties
        // Parent Control
        protected ContainerControl _Parent;
        public ContainerControl Parent
        {
            get { return _Parent; }
            set
            {
                _Parent = value;

                var e = new LocationChangeEventArgs
                {
                    AbsoluteLocation = { Vec = _Parent.AbsoluteBounds.Location }
                };

                ParentAbsoluteLocationChanged(e);
            }
        }

        // Layout features
        public DockStyle Dock { get; set; }
        public AnchorStyles Anchor { get; set; }

        // Name & Caption
        public string Name { get; set; }
        public string Caption { get; set; }

        // Focus
        public bool GainFocus;
        public bool LoseFocus;
        public bool IsFocused;

        // "Positioning"
        public Rectangle Bounds;
        public Rectangle AbsoluteBounds;
        public Margins Margins;
        public Padding Padding;

        // Back- & Fore- color
        public Color BackColor;
        public Color ForeColor;

        // Layout description
        public bool OverrideLayout { get; set; }
        public Point LayoutChange { get; set; }
        public Size LayoutSize { get; set; }
        #endregion
    }
}
