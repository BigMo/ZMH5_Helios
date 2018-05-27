using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.UI.Controls
{
    public interface IValueObserver
    {
        ValueObservable Observable { get; set; }
        string PropertyName { get; set; }
        void UpdateValue();
    }
}
