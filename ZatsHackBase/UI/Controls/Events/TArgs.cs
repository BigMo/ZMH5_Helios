using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace ZatsHackBase.UI.Controls.Events
{
    public class TArgs<T>
    {
        public T Value { get; private set; }
        public TArgs(T value)
        {
            Value = value;
        }
    }

    public class FloatArgs : TArgs<float>
    {
        public FloatArgs(float value) : base(value)
        {
        }
    }
    public class Vector2Args : TArgs<Vector2>
    {
        public Vector2Args(Vector2 value) : base(value)
        {
        }
    }
    public class ControlArgs : TArgs<Control>
    {
        public ControlArgs(Control value) : base(value)
        {
        }
    }
}
