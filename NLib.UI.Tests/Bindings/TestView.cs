using System;
using System.Linq.Expressions;

namespace NLib.UI.Tests.Bindings
{
    public class TestView<T>
    {
        private T myValue;

        public T Value 
        { 
            get { return myValue; }
            set {
                myValue = value;
                ValueChanged?.Invoke(this, new EventArgs());
            } 
        }

        public event EventHandler ValueChanged;
    }
}
