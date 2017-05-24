using System;

namespace NLib.UI.Bindings
{
    interface IBindingCollection : IDisposable
    {
        bool IsAlive { get; }

        bool TrySetContext(object context);

        IContextUpdatCycleBreaker SetBinding<TView>(TView view, IBinding binding) where TView : class;
    }
}
