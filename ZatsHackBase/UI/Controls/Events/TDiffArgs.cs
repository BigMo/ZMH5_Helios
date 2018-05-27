using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI.Controls.Events
{
    public class TDiffArgs<T>
    {
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public TDiffArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class FloatDiffArgs : TDiffArgs<float>
    {
        public FloatDiffArgs(float oldValue, float newValue) : base(oldValue, newValue)
        {
        }
    }
    public class Vector2DiffArgs : TDiffArgs<Vector2>
    {
        public Vector2DiffArgs(Vector2 oldValue, Vector2 newValue) : base(oldValue, newValue)
        {
        }
    }
    public class ControlDiffArgs : TDiffArgs<Control>
    {
        public ControlDiffArgs(Control oldValue, Control newValue) : base(oldValue, newValue)
        {
        }
    }

}
