using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Core;
using ZatsHackBase.UI.Drawing;

namespace ZatsHackBase.UI
{
    public class HackOverlay
    {
        #region PROPERTIES
        public Renderer Renderer { get; private set; }
        public EUCProcess Process { get; private set; }
        public Form Form { get; private set; }
        public Color BackColor { get; set; }
        public Thread FormThread { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public HackOverlay(EUCProcess proc)
        {
            BackColor = Color.Transparent;
            Process = proc;
            Renderer = new Renderer();
            FormThread = new Thread(() =>
            {
                Form = new DrawForm();
                Form.Shown += (o,e) => Renderer.Init(Form);
                Form.Resize += (o, e) =>
                {
                    Renderer.Dispose();
                    Renderer.Init(Form);
                };
                Application.Run(Form);
            });
            FormThread.IsBackground = true;
        }
        #endregion

        #region METHODS
        public void Start()
        {
            FormThread.Start();
        }
        public void AdjustForm()
        {
            if (!Renderer.Initialized)
                return;

            Form.Invoke((MethodInvoker)delegate
            {
                WinAPI.RECT rect = new WinAPI.RECT();
                WinAPI.POINT pt = new WinAPI.POINT();
                
                if (WinAPI.GetClientRect(Process.Process.MainWindowHandle, out rect) &&
                    WinAPI.ClientToScreen(Process.Process.MainWindowHandle, out pt))
                {
                    if(Form.Location.X != pt.X || 
                    Form.Location.Y != pt.Y || 
                    Form.Width != rect.Right - rect.Left || 
                    Form.Height != rect.Bottom - rect.Top)
                    WinAPI.SetWindowPos(Form.Handle,
                        IntPtr.Zero, 
                        pt.X, 
                        pt.Y,
                        rect.Right - rect.Left, 
                        rect.Bottom - rect.Top,
                        0);
                }
            });
        }
        #endregion
    }
}
