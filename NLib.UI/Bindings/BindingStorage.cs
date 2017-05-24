using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NLib.UI.Bindings
{
    static class BindingStorage
    {
        static readonly ConditionalWeakTable<object, IBindingCollection> ownerToCollection;
        static readonly ConditionalWeakTable<object, ICollection<IContextUpdatCycleBreaker>> viewToContextUpdater;

        static BindingStorage()
        {
            ownerToCollection = new ConditionalWeakTable<object, IBindingCollection>();
            viewToContextUpdater = new ConditionalWeakTable<object, ICollection<IContextUpdatCycleBreaker>>();
        }

        public static void UpdateContextValue(object view, object args)
        {
            ICollection<IContextUpdatCycleBreaker> updaters;

            if (viewToContextUpdater.TryGetValue(view, out updaters))
            {
                foreach (var updater in updaters)
                {
                    updater.UpdateContextValues(view);
                }
            }
        }

        public static IBindingBuilder<TView> Bind<TView>(this object contextOwner, TView view) where TView : class
        {
            return new BindingBuilder<TView>(contextOwner, view);
        }

        public static void SetContext<TContext>(object contextOwner, TContext context)
            where TContext : class, INotifyPropertyChanged
        {
            GetBindingCollection<TContext>(contextOwner)?.TrySetContext(context);
        }

        public static void SetBinding<TView, TContext>(this object contextOwner, TView view, IBinding<TView, TContext> binding)
            where TView : class
            where TContext : class, INotifyPropertyChanged
        {
            var bindingCollection = GetBindingCollection<TContext>(contextOwner);
            var contextUpdater = bindingCollection?.SetBinding(view, binding);

            if (contextUpdater != null)
            {
                GetUpdaterCollection(view).Add(contextUpdater);
            }
        }

        public static void ClearBindings(this object contextOwner)
        {
            ownerToCollection.Remove(contextOwner);
        }

        private static ICollection<IContextUpdatCycleBreaker> GetUpdaterCollection(object view)
        {
            ICollection<IContextUpdatCycleBreaker> updatersCollection;

            if (!viewToContextUpdater.TryGetValue(view, out updatersCollection))
            {
                updatersCollection = new List<IContextUpdatCycleBreaker>();
                viewToContextUpdater.Add(view, updatersCollection);
            }

            return updatersCollection;
        }

        private static IBindingCollection GetBindingCollection<TContext>(object contextOwner)
            where TContext : class, INotifyPropertyChanged
        {
            IBindingCollection colletion;

            if (!ownerToCollection.TryGetValue(contextOwner, out colletion))
            {
                colletion = new BindingCollection<TContext>(contextOwner);
                ownerToCollection.Add(contextOwner, colletion);
            }

            return colletion;
        }
    }
}
