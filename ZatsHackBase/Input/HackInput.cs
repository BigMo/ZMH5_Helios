using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZatsHackBase.Maths;

namespace ZatsHackBase.Input
{
    public class HackInput
    {
        #region VARIABLES
        private Keys[] allKeys;
        private byte[] kbOld, kbNew;
        #endregion

        #region PROPERTIES
        //Mouse
        public Vector2 MouseMoveDist { get; private set; }
        public Vector2 MousePos { get; private set; }
        public Button LeftMouseButton { get; private set; }
        public Button RightMouseButton { get; private set; }
        public Button MiddleMouseButton { get; private set; }
        public Button XMouseButton1 { get; private set; }
        public Button XMouseButton2 { get; private set; }

        //Keyboard
        public Keys[] KeysUp { get; private set; }
        public Keys[] KeysDown { get; private set; }
        public Keys[] KeysWentUp { get; private set; }
        public Keys[] KeysWentDown { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public HackInput()
        {
            kbOld = new byte[256];
            kbNew = new byte[256];
            allKeys = ((Keys[])Enum.GetValues(typeof(Keys))).Where(x => x >= 0 && (int)x <= 255).ToArray();

            MousePos = new Vector2(Cursor.Position.X, Cursor.Position.Y);
            MouseMoveDist = Vector2.Zero;
            LeftMouseButton = new Button();
            RightMouseButton = new Button();
            MiddleMouseButton = new Button();
            XMouseButton1 = new Button();
            XMouseButton2 = new Button();

            KeysDown = KeysUp = KeysWentDown = KeysWentUp = new Keys[0];
        }
        #endregion

        #region METHODS
        public void Update()
        {
            //Keyboard
            //var keysDown = kbNew.PressedKeys;
            //KeysUp = kbNew.AllKeys.Except(keysDown).Select(x => (Keys)x).ToArray();
            //KeysDown = keysDown.Select(x => (Keys)x).ToArray();
            //KeysWentDown = kbNew.PressedKeys.Where(x => !kbOld.PressedKeys.Contains(x)).Select(x => (Keys)x).ToArray();
            //KeysWentUp = kbOld.PressedKeys.Where(x => !kbNew.PressedKeys.Contains(x)).Select(x => (Keys)x).ToArray();

            Array.Copy(kbNew, kbOld, kbNew.Length);
            WinAPI.GetKeyboardState(kbNew);
            kbNew = kbNew.Select(x => (int)(x & 0x80) != 0 ? (byte)1 : (byte)0).ToArray();

            KeysDown = allKeys.Where(x => kbNew[(int)x] != 0).ToArray();
            KeysUp = allKeys.Except(KeysDown).ToArray();
            KeysWentDown = allKeys.Where(x => kbOld[(int)x] == 0 && kbNew[(int)x] != 0).ToArray();
            KeysWentUp = allKeys.Where(x => kbOld[(int)x] != 0 && kbNew[(int)x] == 0).ToArray();

            //Mouse
            var newPos = new Vector2(Cursor.Position.X, Cursor.Position.Y);
            MouseMoveDist = newPos - MousePos;
            MousePos = newPos;

            LeftMouseButton.UpdateState(kbNew[(int)Keys.LButton] != 0);
            MiddleMouseButton.UpdateState(kbNew[(int)Keys.MButton] != 0);
            RightMouseButton.UpdateState(kbNew[(int)Keys.RButton] != 0);
            XMouseButton1.UpdateState(kbNew[(int)Keys.XButton1] != 0);
            XMouseButton2.UpdateState(kbNew[(int)Keys.XButton2] != 0);
        }
        #endregion
    }
}
