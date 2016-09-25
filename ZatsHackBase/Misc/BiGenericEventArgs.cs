using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Misc
{
    public class BiGenericEventArgs<T, U> : EventArgs
    {
        #region PROPERTIES
        public T Argument1 { get; private set; }
        public U Argument2 { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public BiGenericEventArgs(T val1, U val2)
        {
            Argument1 = val1;
            Argument2 = val2;
        }
        #endregion
    }
}
