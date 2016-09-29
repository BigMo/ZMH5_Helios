﻿using System;
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
        #endregion

        #region PROPERTIES
        public System.Windows.Forms.Keys Hotkey { get; set; }
        public eKeyMode Mode { get; set; }
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
            Mode = eKeyMode.Hold;
            ActiveByHotkey = false;
        }
        #endregion

        #region METHODS
        public void Reset()
        {
            isDown = wasDown = false;
            ActiveByHotkey = false;
        }
        protected override void OnUpdate(TickEventArgs args)
        {
            base.OnUpdate(args);

            isDown = ZatsHackBase.WinAPI.GetAsyncKeyState(Hotkey) != 0;

            bool wentUp = wasDown && !isDown;
            bool wentDown = !wasDown && isDown;

            if (Mode == eKeyMode.Hold)
                if (wentUp)
                    ActiveByHotkey = false;
                else if (wentDown)
                    ActiveByHotkey = true;
            if (Mode == eKeyMode.Toggle)
                if (wentUp)
                    ActiveByHotkey = !ActiveByHotkey;


            wasDown = isDown;
            #endregion
        }
    }
}