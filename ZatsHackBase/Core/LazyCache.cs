using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    public class LazyCache<T>
    {
        #region VARIABLES
        private T value;
        private bool loaded;
        private Func<T> func;
        #endregion

        #region PROPERTIES
        public T Value
        {
            get
            {
                if (!loaded)
                {
                    value = func();
                    loaded = true;
                }
                return value;
            }
            set
            {
                loaded = true;
                this.value = value;
            }
        }
        #endregion

        #region CONSTRUCTORS
        public LazyCache(Func<T> _fn)
        {
            func = _fn;
            loaded = false;
        }
        #endregion

        #region METHODS
        public void Reset()
        {
            loaded = false;
        }
        public override string ToString()
        {
            if (loaded)
                return string.Format("{0}", value != null ? value.ToString() : "null");
            else
                return string.Format("Lazy<{0}>", typeof(T).Name);
        }
        #endregion

        #region OPERATORS
        public static implicit operator T(LazyCache<T> lc)
        {
            return lc.Value;
        }
        #endregion
    }
}
