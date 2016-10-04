using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Input
{
    public struct Button
    {
        #region VARIABLES
        #endregion

        #region PROPERTIES
        public bool IsDown { get; private set; }
        public bool WasDown { get; private set; }
        public bool IsUp { get { return !IsDown; } }
        public bool WasUp { get { return !WasDown; } }

        public bool WentUp { get { return WasDown && IsUp; } }
        public bool WentDown { get { return WasUp && IsDown; } }
        #endregion

        #region METHODS
        public void UpdateState(bool downState)
        {
            WasDown = IsDown;
            IsDown = downState;
        }
        #endregion
    }
}
