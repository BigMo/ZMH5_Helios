using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZatsHackBase.Core
{
    public class LazyArray<T>
    {
        #region VARIABLES
        private LazyCache<T>[] elements;
        #endregion

        #region PROPERTIES
        public int Length { get { return elements.Length; } }
        protected Func<int, T> loadFunc;
        #endregion

        #region CONSTRUCTORS
        public LazyArray(int length)
        {
            elements = new LazyCache<T>[length];
        }
        #endregion

        #region METHODS
        public override string ToString()
        {
            return string.Format("LazyArray<{0}>: Length: {1}", typeof(T).Name, Length);
        }
        #endregion

        #region OPERANDS
        public T this[int index]
        {
            get
            {
                if (elements[index] == null)
                    elements[index] = new LazyCache<T>(() => loadFunc(index));
                return elements[index].Value;
            }
            set
            {
                if (elements[index] == null)
                    elements[index] = new LazyCache<T>(() => loadFunc(index));
                elements[index].Value = value;
            }
        }
        #endregion
    }
}
