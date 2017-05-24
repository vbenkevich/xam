using System;
using System.ComponentModel;

namespace NLib.UI.Tests.Bindings
{
    public class TestContext<T> : ObservableObject
    {
        private T property;
        public T Property
        {
            get { return property; }
            set { SetProperty(ref property, value); }
        }
    }
}
