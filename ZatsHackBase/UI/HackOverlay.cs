using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;
using ZatsHackBase.GUI.Controls;
using ZatsHackBase.Input;
using ZatsHackBase.Maths;
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
        public Vector2 Size { get; private set; }
        public Frame BaseContainer { get; private set; }
        public HackInput Input { get; private set; }
        public List<Controls.Control> Controls { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public HackOverlay(EUCProcess proc, HackInput input)
        {
            BackColor = Color.Transparent;
            Input = input;
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
            Size = Vector2.Zero;
            BaseContainer = new Frame();
            BaseContainer.BackColor = Color.Transparent;
            Controls = new List<Controls.Control>();
        }
        #endregion

        #region METHODS
        public void Start()
        {
            FormThread.Start();
        }
        public void Update(Time time, Vector2 cursorPos)
        {
            AdjustForm();
            ProcessInput();

            foreach (var c in Controls)
            {
                c.Update(time, Input, cursorPos);
                c.Draw(Renderer);
            }
        }

        private void ProcessInput()
        {
            #region MOUSE
            if (Input.MouseMoveDist.Length > 0)
                BaseContainer.MouseMove(new MouseEventArgs(MouseButtons.None, 0, (int)Input.MousePos.X, (int)Input.MousePos.Y, 0));

            HandleMouseButton(Input.LeftMouseButton, MouseButtons.Left);
            HandleMouseButton(Input.RightMouseButton, MouseButtons.Right);
            HandleMouseButton(Input.MiddleMouseButton, MouseButtons.Middle);
            HandleMouseButton(Input.XMouseButton1, MouseButtons.XButton1);
            HandleMouseButton(Input.XMouseButton2, MouseButtons.XButton2);
            #endregion

            #region KEYBOARD
            foreach (var key in Input.KeysWentDown)
                BaseContainer.KeyDown(new KeyEventArgs(key));

            foreach (var key in Input.KeysWentUp)
                BaseContainer.KeyUp(new KeyEventArgs(key));
            #endregion
        }
        private void HandleMouseButton(Input.Button button, MouseButtons mbutton)
        {
            if (button.WentUp)
                BaseContainer.MouseUp(new MouseEventArgs(mbutton, 0, (int)Input.MousePos.X, (int)Input.MousePos.Y, 0));
            else if (button.WentDown)
                BaseContainer.MouseDown(new MouseEventArgs(mbutton, 0, (int)Input.MousePos.X, (int)Input.MousePos.Y, 0));
        }
        private void AdjustForm()
        {
            if (!Renderer.Initialized)
                return;


            Form.Invoke((MethodInvoker)delegate
            {
                WinAPI.BringWindowToTop(Form.Handle);
                WinAPI.RECT rect = new WinAPI.RECT();
                WinAPI.POINT pt = new WinAPI.POINT();
                
                if (WinAPI.GetClientRect(Process.Process.MainWindowHandle, out rect) &&
                    WinAPI.ClientToScreen(Process.Process.MainWindowHandle, out pt))
                {
                    bool sizeChanged = 
                        Form.Width != rect.Right - rect.Left ||
                        Form.Height != rect.Bottom - rect.Top;

                    bool posChanged = 
                        Form.Location.X != pt.X ||
                        Form.Location.Y != pt.Y;

                    if (sizeChanged)
                    {
                        Size = new Vector2(rect.Right - rect.Left, rect.Bottom - rect.Top);
                        BaseContainer.Bounds.Size = Size;
                    }
                    if (posChanged || sizeChanged)
                    {
                        WinAPI.SetWindowPos(Form.Handle,
                            IntPtr.Zero,
                            pt.X,
                            pt.Y,
                            rect.Right - rect.Left,
                            rect.Bottom - rect.Top,
                            0);
                    }
                }
            });
        }
        #endregion
    }
}
