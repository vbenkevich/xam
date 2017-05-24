using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NLib.UI.Bindings
{
    class BindingCollection<TContext> : IBindingCollection
        where TContext : class, INotifyPropertyChanged
    {
        readonly WeakReference contextOwnerReference;
        readonly Dictionary<string, ICollection<IBindingWrapper>> bindingsMap; // todo maybe, it should be ICollection.
        readonly List<IBindingWrapper> allBindings;

        TContext currentContex;

        public BindingCollection(object contextOwner)
        {
            contextOwnerReference = new WeakReference(contextOwner);
            bindingsMap = new Dictionary<string, ICollection<IBindingWrapper>>();
            allBindings = new List<IBindingWrapper>();
        }

        public bool IsAlive { get { return contextOwnerReference.IsAlive; } }

        public bool TrySetContext(object context)
        {
            return TrySetContext(context as TContext);
        }

        public IContextUpdatCycleBreaker SetBinding<TView>(TView view, IBinding binding) where TView : class
        {
            var typedBinding = binding as IBinding<TView, TContext>;

            if (typedBinding == null)
            {
                throw new InvalidOperationException($"Invalid binding type. Expected: {typeof(IBinding<TView, TContext>)}");
            }

            var wrapper = new BindingWrapper<TView>(view, typedBinding);

            AddBinding(wrapper);

            UpdateViewValue(wrapper);

            return typedBinding.CanUpdateContext
                 ? new ContextUpdatCycleBreaker<TView>(typedBinding, this)
                 : null;
        }

        public void Dispose()
        {
            TrySetContext(null);

            foreach (var binding in allBindings)
            {
                binding.DetachFromView();
            }
        }

        private void AddBinding(IBindingWrapper wrapper)
        {
            allBindings.Add(wrapper);

            if (wrapper.IgnoreContextChanges)
            {
                return;
            }

            if (!bindingsMap.TryGetValue(wrapper.PropertyName, out ICollection<IBindingWrapper> toPropertyBindings))
            {
                toPropertyBindings = new List<IBindingWrapper>();
                bindingsMap[wrapper.PropertyName] = toPropertyBindings;
            }

            toPropertyBindings.Add(wrapper);
        }

        private bool TrySetContext(TContext context)
        {
            if (currentContex != null)
            {
                currentContex.PropertyChanged -= OnContextPropertyChanged;
            }

            if (context != null)
            {
                context.PropertyChanged += OnContextPropertyChanged;
            }

            currentContex = context;

            foreach (var propertyName in bindingsMap.Keys)
            {
                UpdateViewValues(propertyName);
            }

            return true;
        }

        private void UpdateViewValues(string propertyName)
        {
            ICollection<IBindingWrapper> wrappers;

            if (bindingsMap.TryGetValue(propertyName, out wrappers))
            {
                foreach (var wrapper in wrappers)
                {
                    UpdateViewValue(wrapper);
                }
            }
        }

        private bool UpdateViewValue(IBindingWrapper wrapper)
        {
            var context = currentContex;

            if (!IsAlive)
            {
                Dispose();
                return false;
            }
            else if (context != null)
            {
                wrapper.UpdateViewValue(context);
                return true;
            }

            return true;
        }

        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            UpdateViewValues(args.PropertyName);
        }

        interface IBindingWrapper
        {
            bool IgnoreContextChanges { get; }

            string PropertyName { get; }

            void UpdateViewValue(TContext context);

            void DetachFromView();
        }

        class ContextUpdatCycleBreaker<TView> : IContextUpdatCycleBreaker where TView : class
        {
            readonly WeakReference<IBinding<TView, TContext>> bindingRef;
            readonly WeakReference<BindingCollection<TContext>> collectionRef;

            public ContextUpdatCycleBreaker(IBinding<TView, TContext> binding, BindingCollection<TContext> collection)
            {
                this.collectionRef = new WeakReference<BindingCollection<TContext>>(collection);
                this.bindingRef = new WeakReference<IBinding<TView, TContext>>(binding);
            }

            public void UpdateContextValues(object view)
            {
                IBinding<TView, TContext> binding;
                BindingCollection<TContext> collection;

                if (bindingRef.TryGetTarget(out binding) && collectionRef.TryGetTarget(out collection))
                {
                    binding?.UpdateContextValue((TView)view, collection.currentContex);
                }
            }
        }

        class BindingWrapper<TView> : IBindingWrapper where TView : class
        {
            readonly IBinding<TView, TContext> binding;
            readonly EventHandler handler;
            readonly TView view;

            public BindingWrapper(TView view, IBinding<TView, TContext> binding)
            {
                this.view = view;
                this.binding = binding;

                handler = new EventHandler(BindingStorage.UpdateContextValue);
                binding.AttachToView(view, handler);
            }

            public bool IgnoreContextChanges => !binding.CanUpdateView;

            public string PropertyName => binding.PropertyName;

            public void DetachFromView()
            {
                binding.DetachFromView(view, handler);
            }

            public void UpdateViewValue(TContext context)
            {
                binding.UpdateViewValue(view, context);
            }
        }
    }
}
