using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Misc
{
    public class GenericEventArgs<T> : EventArgs
    {
        #region PROPERTIES
        public T Argument { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public GenericEventArgs(T value)
        {
            Argument = value;
        }
        #endregion
    }
}
