using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _ZMH5__Helios.UI.Controls
{
    public class ValueObservable
    {
        private object value;
        private List<IValueObserver> observers;

        public object Value
        {
            get { return value; }
            set
            {
                if(this.value != value)
                {
                    this.value = value;
                    foreach (var c in observers)
                        c.UpdateValue();
                }
            }
        }

        public ValueObservable()
        {
            observers = new List<IValueObserver>();
        }

        private FieldInfo GetFieldName(string fieldName)
        {
            if (value == null)
                return null;

            return value.GetType().GetField(fieldName);
        }

        public bool GetValue<T>(string fieldName, ref T output)
        {
            var prop = GetFieldName(fieldName);
            if (prop == null)
                return false;

            output = (T)prop.GetValue(value);
            return true;
        }

        public bool SetValue<T>(string fieldName, T value)
        {
            var prop = GetFieldName(fieldName);
            if (prop == null)
                return false;

            prop.SetValue(this.value, value);
            return true;
        }

        public void Subscribe(IValueObserver observer)
        {
            if (observers.Contains(observer) || observer == null)
                return;

            observers.Add(observer);
            observer.Observable = this;
            observer.UpdateValue();
        }

        public void Unsubscribe(IValueObserver observer)
        {
            if (observers.Contains(observer))
                return;

            observers.Remove(observer);
            observer.Observable = null;
        }
    }
}
