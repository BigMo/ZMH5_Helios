using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZatsHackBase.UI
{
    class DrawForm : Form
    {
        public DrawForm()
        {
            this.DoubleBuffered = true;

            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Blue;
            this.TransparencyKey = BackColor;
            this.TopMost = true;
            this.TopLevel = true;

            int initialStyle = WinAPI.GetWindowLong(this.Handle, (int)WinAPI.GetWindowLongFlags.GWL_EXSTYLE);
            WinAPI.SetWindowLong(this.Handle, (int)WinAPI.GetWindowLongFlags.GWL_EXSTYLE, initialStyle | (int)WinAPI.ExtendedWindowStyles.WS_EX_LAYERED | (int)WinAPI.ExtendedWindowStyles.WS_EX_TRANSPARENT);
            WinAPI.SetWindowPos(this.Handle, (IntPtr)WinAPI.SetWindpwPosHWNDFlags.TopMost, 0, 0, 0, 0, (uint)WinAPI.SetWindowPosFlags.NOMOVE | (uint)WinAPI.SetWindowPosFlags.NOSIZE);
            WinAPI.SetLayeredWindowAttributes(this.Handle, 0, 255, (uint)WinAPI.LayeredWindowAttributesFlags.LWA_ALPHA);
        }

        #region METHODS
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            WinAPI.MARGINS margins = new WinAPI.MARGINS();
            margins.topHeight = 0;
            margins.bottomHeight = 0;
            margins.leftWidth = this.Left;
            margins.rightWidth = this.Right;
            this.Invoke((MethodInvoker)(() => { WinAPI.DwmExtendFrameIntoClientArea(this.Handle, ref margins); }));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            WinAPI.MARGINS margins = new WinAPI.MARGINS();
            margins.topHeight = 0;
            margins.bottomHeight = 0;
            margins.leftWidth = this.Left;
            margins.rightWidth = this.Right;
            this.Invoke((MethodInvoker)(() => { WinAPI.DwmExtendFrameIntoClientArea(this.Handle, ref margins); }));
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{

        //}

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{

        //}
        #endregion
    }
}
