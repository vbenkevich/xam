using System;
using System.ComponentModel;

namespace NLib.UI.Bindings
{
    interface IBinding
    {
        string PropertyName { get; }

        bool CanUpdateContext { get; }

        bool CanUpdateView { get; }
    }

    interface IBinding<TView, TContext> : IBinding
        where TView : class
        where TContext : class, INotifyPropertyChanged
    {
        void UpdateViewValue(TView view, TContext context);

        void UpdateContextValue(TView view, TContext context);

        void AttachToView(TView view, EventHandler viewUpdateHandler);

        void DetachFromView(TView view, EventHandler viewUpdateHandler);
    }
}
