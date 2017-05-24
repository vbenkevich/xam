using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NLib.UI.Bindings
{
    public interface IBindingBuilder<TView> where TView : class
    {
        IBindingBuilder<TView, TValue> ViewProperty<TValue>(Action<TView, TValue> setValue, Func<TView, TValue> getValue = null);

        IBindingBuilder<TView, TValue> ViewProperty<TValue>(Expression<Func<TView, TValue>> expression);
    }

    public interface IBindingBuilder<TView, TValue> where TView : class
    {
        IBindingBuilder<TView, TValue> ViewEvent(string eventName);

        void To<TContext>(Expression<Func<TContext, TValue>> expression) where TContext : class, INotifyPropertyChanged;

        IBindingBuilder<TView, TValue, TContextValue> Converter<TContextValue>(IValueConverter<TContextValue, TValue> converter);
    }

    public interface IBindingBuilder<TView, TValue, TContextValue> where TView : class
    {
        void To<TContext>(Expression<Func<TContext, TContextValue>> expression) where TContext : class, INotifyPropertyChanged;
    }
}