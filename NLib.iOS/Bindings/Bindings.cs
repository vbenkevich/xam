using System.Reflection;
using System.ComponentModel;
using UIKit;
using NLib.UI;
using NLib.UI.Bindings;

namespace NLib.iOS.Bindings
{
    public static class Bindings
    {
        public static void SetContext<TContext>(this UIViewController controller, TContext context) where TContext : class, INotifyPropertyChanged
        {
            BindingStorage.SetContext<TContext>(controller, context);
        }

        public static void SetContext<TContext>(this UIView view, TContext context) where TContext : class, INotifyPropertyChanged
        {
            BindingStorage.SetContext<TContext>(view, context);
        }

        public static IBindingBuilder<UILabel, string> Bind(this UIView contextOwner, UILabel label)
        {
            return BindingStorage.Bind<UILabel>(contextOwner, label)
                                 .ViewProperty<string>(SetText);
        }

        public static IBindingBuilder<UILabel, string> Bind(this UIViewController contextOwner, UILabel label)
        {
            return BindingStorage.Bind<UILabel>(contextOwner, label)
                                 .ViewProperty<string>(SetText);
        }

        public static IBindingBuilder<UITextField, string> Bind(this UIViewController contextOwner, UITextField textField)
        {
            return BindingStorage.Bind<UITextField>(contextOwner, textField)
                                 .ViewProperty<string>(SetText, f => f.Text)
                                 .ViewEvent(nameof(UITextField.AllEditingEvents));
        }

        public static IBindingBuilder<UITextField, string> Bind(this UIView contextOwner, UITextField textField)
        {
            return BindingStorage.Bind<UITextField>(contextOwner, textField)
                                 .ViewProperty<string>(SetText, f => f.Text)
                                 .ViewEvent(nameof(UITextField.AllEditingEvents));
        }

        public static IBindingBuilder<UITextField, IInpuntViewModel<string>> BindToInput(this UIView contextOwner, UITextField textField)
        {
            SetTextFieldInnerBindings(textField);

            return BindingStorage.Bind<UITextField>(contextOwner, textField)
                                 .ViewProperty<IInpuntViewModel<string>>((view, viewModel) => view.SetContext(viewModel));
        }

        public static IBindingBuilder<UITextField, IInpuntViewModel<string>> BindToInput(this UIViewController contextOwner, UITextField textField)
        {
            SetTextFieldInnerBindings(textField);

            return BindingStorage.Bind<UITextField>(contextOwner, textField)
                                 .ViewProperty<IInpuntViewModel<string>>((view, viewModel) => view.SetContext(viewModel));
        }

        private static void SetTextFieldInnerBindings(UITextField textField)
        {
            var valueChangedBinding = Binding<UITextField, IInpuntViewModel<string>, string>.TwoWay(
                nameof(IInpuntViewModel<string>.View),
                inputViewModel => inputViewModel.View,
                (field, text) => field.Text = text,
                (inputViewModel, text) => inputViewModel.EditValueAndUpdateModel(text),
                field => field.Text,
                typeof(UITextField).GetRuntimeEvent(nameof(UITextField.EditingChanged))
            );

            var endEditingBinding = Binding<UITextField, IInpuntViewModel<string>, string>.OneWayToSource(
                (inputViewModel, text) => inputViewModel.EditValueAndUpdateModel(text),
                field => field.Text,
                typeof(UITextField).GetRuntimeEvent(nameof(UITextField.EditingDidEnd))
            );

            BindingStorage.SetBinding(textField, textField, valueChangedBinding);
            BindingStorage.SetBinding(textField, textField, endEditingBinding);
        }

        private static void SetText(UITextField field, string text)
        {
            if (!Equals(field.Text, text))
            {
                field.Text = text;
            }
        }

        private static void SetText(UILabel label, string text)
        {
            if (!Equals(label.Text, text))
            {
                label.Text = text;
            }
        }
    }
}
