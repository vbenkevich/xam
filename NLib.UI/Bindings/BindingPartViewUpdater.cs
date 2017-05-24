using System;

namespace NLib.UI.Bindings
{
    class BindingPartViewUpdater<TView, TContext, TValue>
    {
        readonly Func<TContext, TValue> getContextValue;
        readonly Action<TView, TValue> setViewValue;

        public BindingPartViewUpdater(Func<TContext, TValue> getContextValue, Action<TView, TValue> setViewValue)
        {
            this.setViewValue = setViewValue;
            this.getContextValue = getContextValue;
        }

        public void UpdateViewValue(TView view, TContext context)
        {
            setViewValue?.Invoke(view, getContextValue(context));
        }
    }
}
