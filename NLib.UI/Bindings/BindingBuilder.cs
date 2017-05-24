using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace NLib.UI.Bindings
{
    class BindingBuilder<TView> : IBindingBuilder<TView> where TView : class
    {
        readonly TView view;
        readonly object contextOwner;

        public BindingBuilder(object contextOwner, TView view)
        {
            this.contextOwner = contextOwner ?? throw new ArgumentNullException(nameof(contextOwner));
            this.view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public IBindingBuilder<TView, TValue> ViewProperty<TValue>(Expression<Func<TView, TValue>> expression)
        {
            var propertyInfo = expression.GetPropertyInfo();

            return ViewProperty(
                CreateSetter<TValue>(propertyInfo),
                CreateGetter<TValue>(propertyInfo));
        }

        public IBindingBuilder<TView, TValue> ViewProperty<TValue>(Action<TView, TValue> setValue, Func<TView, TValue> getValue = null)
        {
            return new BindingBuilder<TView, TValue>(contextOwner, view, setValue, getValue);
        }

        private Action<TView, TValue> CreateSetter<TValue>(PropertyInfo propertyInfo)
        {
            return (view, val) => propertyInfo.SetValue(view, val);
        }

        private Func<TView, TValue> CreateGetter<TValue>(PropertyInfo propertyInfo)
        {
            return (view) => (TValue)propertyInfo.GetValue(view);
        }
    }

    class BindingBuilder<TView, TValue> : IBindingBuilder<TView, TValue> where TView : class
    {
        readonly Action<TView, TValue> setViewValue;
        readonly Func<TView, TValue> getViewValue;
        readonly TView view;
        readonly object contextOwner;
        EventInfo eventInfo;

        public BindingBuilder(object contextOwner, TView view, Action<TView, TValue> setViewValue, Func<TView, TValue> getViewValue)
        {
            this.view = view ?? throw new ArgumentNullException(nameof(view));
            this.contextOwner = contextOwner ?? throw new ArgumentNullException(nameof(contextOwner));
            this.setViewValue = setViewValue ?? throw new ArgumentNullException(nameof(setViewValue));
            this.getViewValue = getViewValue;
            this.eventInfo = null;
        }

        public void To<TContext>(Expression<Func<TContext, TValue>> expression)
            where TContext : class, INotifyPropertyChanged
        {
            var getter = expression.Compile();
            var propertyInfo = expression.GetPropertyInfo();
            var setter = CreateSetter<TContext>(propertyInfo);

            if (eventInfo == null)
            {
                BindingStorage.SetBinding(contextOwner, view, Binding<TView, TContext, TValue>.OneWayToView(propertyInfo.Name, getter, setViewValue));
            }
            else
            {
                BindingStorage.SetBinding(contextOwner, view, Binding<TView, TContext, TValue>.TwoWay(propertyInfo.Name, getter, setViewValue, setter, getViewValue, eventInfo));
            }
        }

        public IBindingBuilder<TView, TValue, TContextValue> Converter<TContextValue>(IValueConverter<TContextValue, TValue> converter)
        {
            return new BindingBuilder<TView, TValue, TContextValue>(contextOwner, view, setViewValue, getViewValue, converter, eventInfo);
        }

        public IBindingBuilder<TView, TValue> ViewEvent(string eventName)
        {
            eventInfo = typeof(TView).GetRuntimeEvent(eventName);
            return this;
        }

        private Action<TContext, TValue> CreateSetter<TContext>(PropertyInfo propertyInfo)
        {
            if (getViewValue == null) return null;
            return (ctx, val) => propertyInfo.SetValue(ctx, val);
        }
    }

    class BindingBuilder<TView, TValue, TContextValue> : IBindingBuilder<TView, TValue, TContextValue> where TView : class
    {
        readonly TView view;
        readonly Action<TView, TValue> setViewValue;
        readonly Func<TView, TValue> getViewValue;
        readonly IValueConverter<TContextValue, TValue> converter;
        readonly EventInfo eventInfo;
        readonly object contextOwner;

        public BindingBuilder(object contextOwner, TView view, Action<TView, TValue> setViewValue, Func<TView, TValue> getViewValue, IValueConverter<TContextValue, TValue> converter, EventInfo eventInfo)
        {
            this.contextOwner = contextOwner ?? throw new ArgumentNullException(nameof(contextOwner));
            this.view = view ?? throw new ArgumentNullException(nameof(view));

            this.setViewValue = setViewValue ?? throw new ArgumentNullException(nameof(setViewValue));
            this.getViewValue = getViewValue;

            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));

            this.eventInfo = eventInfo;
        }

        public void To<TContext>(Expression<Func<TContext, TContextValue>> expression)
            where TContext : class, INotifyPropertyChanged
        {
            var propertyInfo = expression.GetPropertyInfo();
            var getter = expression.GetWithForwardConvert(converter);
            Action<TContext, TValue> setter = null;

            if (getViewValue != null)
            {
                setter = propertyInfo.SetWithBackConvert<TContext, TContextValue, TValue>(converter);
            }

            if (eventInfo == null)
            {
                BindingStorage.SetBinding(contextOwner, view, Binding<TView, TContext, TValue>.OneWayToView(propertyInfo.Name, getter, setViewValue));
            }
            else
            {
                BindingStorage.SetBinding(contextOwner, view, Binding<TView, TContext, TValue>.TwoWay(propertyInfo.Name, getter, setViewValue, setter, getViewValue, eventInfo));
            }
        }
    }

    static class BindingBuilderHelper
    {
        public static Func<TContext, TValue> GetWithForwardConvert<TContext, TContextValue, TValue>(
            this Expression<Func<TContext, TContextValue>> expression, 
            IValueConverter<TContextValue, TValue> converter)
        {
            var getter = expression.Compile();
            return ctx => converter.Convert(getter(ctx));
        }

        public static Action<TContext, TValue> SetWithBackConvert<TContext, TContextValue, TValue>(
            this PropertyInfo propertyInfo,
            IValueConverter<TContextValue, TValue> converter)
        {
            return (ctx, val) => propertyInfo.SetValue(ctx, converter.ConvertBack(val));
        }

        public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");
            }

            var propInfo = memberExpression.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");
            }

            return propInfo;
        }
    }
}
