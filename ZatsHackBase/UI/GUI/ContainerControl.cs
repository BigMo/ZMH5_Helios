using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Maths;

namespace ZatsHackBase.GUI
{
    public class ContainerControl : Control
    {
        public ContainerControl(ContainerControl parent = null) : base(parent)
        {
        }

        public override void MouseDown(MouseEventArgs e)
        {

            if (FocusControl == null || FocusControl.LoseFocus == true)
            {
                foreach (var el in Controls)
                {

                    FocusControl = el;
                    break;

                }
            }

            FocusControl.MouseDown(e);

            base.MouseDown(e);
        }

        public override void MouseUp(MouseEventArgs e)
        {
            FocusControl?.MouseUp(e);
            base.MouseUp(e);
        }

        public override void MouseMove(MouseEventArgs e)
        {
            foreach(var el in Controls)
            {
                el.MouseMove(e);
            }
            base.MouseMove(e);
        }

        public override void MouseScroll(MouseEventArgs e)
        {
            FocusControl?.MouseScroll(e);
            base.MouseScroll(e);
        }

        public override void KeyDown(KeyEventArgs e)
        {
            FocusControl?.KeyDown(e);
            base.KeyDown(e);
        }

        public override void KeyUp(KeyEventArgs e)
        {
            FocusControl?.KeyUp(e);
            base.KeyUp(e);
        }

        public override void ParentAbsoluteLocationChanged(LocationChangeEventArgs e)
        {
            base.ParentAbsoluteLocationChanged(e);
            RelocateChildren();
        }

        protected void RelocateChildren()
        {
            LayoutEngine.Run(Scroll.Left, Scroll.Top);
        }

        public List<Control> FindControlsByCaption(string caption)
        {
            return Controls.Where(e => e.Caption == caption).ToList();
        }

        public Control FindControlByName(string name)
        {
            return Controls.Find(e => e.Name == name);
        }

        #region Variables/Properties

        public List<Control> Controls;
        public Control FocusControl;
        public Point Scroll;
        protected LayoutEngine LayoutEngine;

        #endregion
    }
}
