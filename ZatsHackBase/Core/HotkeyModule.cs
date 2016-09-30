using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Core.Timing;

namespace ZatsHackBase.Core
{
    public class HotkeyModule : HackModule
    {
        #region VARIABLES
        private bool isDown, wasDown;
        private bool active;
        private bool waitForUp;
        #endregion

        #region PROPERTIES
        public System.Windows.Forms.Keys Hotkey { get; set; }
        public KeyMode Mode { get; set; }
        public bool ActiveByHotkey
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    ActiveChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public EventHandler<EventArgs> ActiveChanged { get; set; }
        #endregion

        #region CONSTRUCTORS
        public HotkeyModule(ModulePriority prio) : base(prio)
        {
            Hotkey = System.Windows.Forms.Keys.None;
            Mode = KeyMode.Hold;
            ActiveByHotkey = false;
        }
        #endregion

        #region METHODS
        public void Reset()
        {
            waitForUp = isDown;
            isDown = wasDown = false;
            ActiveByHotkey = false;
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            isDown = ZatsHackBase.WinAPI.GetAsyncKeyState(Hotkey) != 0;
            if (waitForUp)
            {
                waitForUp = isDown;
                return;
            }
            bool wentUp = wasDown && !isDown;
            bool wentDown = !wasDown && isDown;

            if (Mode == KeyMode.Hold)
                if (wentUp)
                    ActiveByHotkey = false;
                else if (wentDown)
                    ActiveByHotkey = true;
            if (Mode == KeyMode.Toggle)
                if (wentUp)
                    ActiveByHotkey = !ActiveByHotkey;


            wasDown = isDown;
            #endregion
        }
    }
}