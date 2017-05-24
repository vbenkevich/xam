using System;
using System.Reflection;

namespace NLib.UI.Bindings
{
    public class BindingPartContextUpdater<TView, TContext, TValue>
    {
        readonly EventInfo eventInfo;
        readonly Action<TContext, TValue> setContextValue;
        readonly Func<TView, TValue> getViewValue;

        public BindingPartContextUpdater(Action<TContext, TValue> setContextValue, Func<TView, TValue> getViewValue, EventInfo eventInfo)
        {
            this.getViewValue = getViewValue;
            this.setContextValue = setContextValue;
            this.eventInfo = eventInfo;
        }

        public void UpdateContextValue(TView view, TContext context)
        {
            setContextValue(context, getViewValue(view));
        }

        public void AttachToView(TView view, EventHandler viewUpdateHandler)
        {
            eventInfo.AddEventHandler(view, viewUpdateHandler);
        }

        public void DetachFromView(TView view, EventHandler viewUpdateHandler)
        {
            eventInfo.RemoveEventHandler(view, viewUpdateHandler);
        }
    }
}
