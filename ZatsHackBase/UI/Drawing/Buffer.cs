using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.UI.Drawing
{
    public abstract class Buffer : IDisposable
    {
        #region METHODS

        public abstract void Dispose();
        public abstract void Apply();

        #endregion
    }
}
