using System;
using System.ComponentModel;
using System.Reflection;

namespace NLib.UI.Bindings
{
    class Binding<TView, TContext, TValue> : IBinding<TView, TContext>
        where TView : class
        where TContext : class, INotifyPropertyChanged
    {
        readonly ContextUpdater contextUpdater;
        readonly ViewUpdater viewUpdater;

        Binding(string propertyName, ContextUpdater contextUpdater, ViewUpdater viewUpdater)
        {
            this.PropertyName = propertyName ?? throw new ArgumentNullException(propertyName);
            this.viewUpdater = viewUpdater;
            this.contextUpdater = contextUpdater;
        }

        public bool CanUpdateContext => contextUpdater != null;

        public bool CanUpdateView => viewUpdater != null;

        public string PropertyName { get;  }

        public static IBinding<TView, TContext> OneWayToView(string propertyName, Func<TContext, TValue> getContextValue, Action<TView, TValue> setViewValue)
        {
            var viewUpdater = new ViewUpdater(getContextValue, setViewValue);
            return new Binding<TView, TContext, TValue>(propertyName, null, viewUpdater);
        }

        public static IBinding<TView, TContext> OneWayToSource(Action<TContext, TValue> setContextValue, Func<TView, TValue> getViewValue, EventInfo eventInfo)
        {
            var contextUpdater = new ContextUpdater(setContextValue, getViewValue, eventInfo);
            return new Binding<TView, TContext, TValue>(string.Empty, contextUpdater, null);
        }

        public static IBinding<TView, TContext> TwoWay(
            string propertyName, Func<TContext, TValue> getContextValue, Action<TView, TValue> setViewValue,
            Action<TContext, TValue> setContextValue, Func<TView, TValue> getViewValue, EventInfo eventInfo)
        {
            var viewUpdater = new ViewUpdater(getContextValue, setViewValue);
            var contextUpdater = new ContextUpdater(setContextValue, getViewValue, eventInfo);
            return new Binding<TView, TContext, TValue>(propertyName, contextUpdater, viewUpdater);
        }

        public void AttachToView(TView view, EventHandler viewUpdateHandler)
        {
            contextUpdater?.AttachToView(view, viewUpdateHandler);
        }

        public void DetachFromView(TView view, EventHandler viewUpdateHandler)
        {
            contextUpdater?.DetachFromView(view, viewUpdateHandler);
        }

        public void UpdateContextValue(TView view, TContext context)
        {
            contextUpdater?.UpdateContextValue(view, context);
        }

        public void UpdateViewValue(TView view, TContext context)
        {
            viewUpdater?.UpdateViewValue(view, context);
        }

        class ContextUpdater
        {
            readonly EventInfo eventInfo;
            readonly Action<TContext, TValue> setContextValue;
            readonly Func<TView, TValue> getViewValue;

            public ContextUpdater(Action<TContext, TValue> setContextValue, Func<TView, TValue> getViewValue, EventInfo eventInfo)
            {
                this.getViewValue = getViewValue ?? throw new ArgumentNullException(nameof(getViewValue));
                this.setContextValue = setContextValue ?? throw new ArgumentNullException(nameof(setContextValue));
                this.eventInfo = eventInfo ?? throw new ArgumentNullException(nameof(eventInfo));
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

        class ViewUpdater
        {
            readonly Func<TContext, TValue> getContextValue;
            readonly Action<TView, TValue> setViewValue;

            public ViewUpdater(Func<TContext, TValue> getContextValue, Action<TView, TValue> setViewValue)
            {
                this.setViewValue = setViewValue ?? throw new ArgumentNullException(nameof(setViewValue));
                this.getContextValue = getContextValue ?? throw new ArgumentNullException(nameof(getContextValue));
            }

            public void UpdateViewValue(TView view, TContext context)
            {
                setViewValue?.Invoke(view, getContextValue(context));
            }
        }
    }
}
