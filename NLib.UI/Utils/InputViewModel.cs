using System;
using System.ComponentModel;
using NLib.UI.Bindings;

namespace NLib.UI
{
    /// <summary>
    /// Inteface for inputVm (used only for bindings)
    /// </summary>
    public interface IInpuntViewModel<TView> : INotifyPropertyChanged
    {
        TView View { get; }

        void EditValueAndUpdateModel(TView view);
    }

    /// <summary>
    /// Simple inputViewModel
    /// </summary>
    public class InputViewModel<T> : InputViewModel<T, T>
    {
        private static readonly EmptyConverter emptyConverter = new EmptyConverter();

        public InputViewModel(IValidator validator = null, IReverter reverter = null) : base(emptyConverter, validator, reverter)
        {
        }

        public InputViewModel(IValueConverter<T, T> converter, IValidator validator = null, IReverter reverter = null) : base(converter, validator, reverter)
        {
        }

        public InputViewModel(IConverter converter, IValidator validator = null, IReverter reverter = null) : base(converter, validator, reverter)
        {
        }

        class EmptyConverter : IConverter
        {
            public T ConvertToView(T model)
            {
                return model;
            }

            public bool TryConvertToModel(T view, out T model, out string message)
            {
                message = string.Empty;
                model = view;
                return true;
            }
        }
    }

    public delegate void ValueChangedDelegate<T>(T newValue, T oldValue);

    /// <summary>
    /// Incapsulate UI input logic (validation, delayed updates and other)
    /// </summary>
    public class InputViewModel<TModel, TView> : ObservableObject, IInpuntViewModel<TView>
    {
        private static readonly SuccessValidator successValidator = new SuccessValidator();
        private static readonly PrevValueReverter prevValueReverter = new PrevValueReverter();

        private readonly IConverter converter;
        private readonly IReverter reverter;
        private readonly IValidator validator;

        private TView view;
        private TModel model;
        private TModel defaultModel;
        private bool isValid;
        private string errorMessage;

        private bool viewWasSet;
        private bool modelWasSet;

        public InputViewModel(IValueConverter<TModel, TView> converter, IValidator validator = null, IReverter reverter = null)
            : this(new ConverterAdapter(converter), validator, reverter)
        {
        }

        public InputViewModel(IConverter converter, IValidator validator = null, IReverter reverter = null)
        {
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));

            this.validator = validator ?? successValidator;
            this.reverter = reverter ?? prevValueReverter;

            IsValid = true;
            ValidateDuringEditing = false;
            ErrorMessage = string.Empty;
        }

        public interface IConverter
        {
            TView ConvertToView(TModel model);

            bool TryConvertToModel(TView view, out TModel model, out string message);
        }

        public interface IValidator
        {
            bool IsValid(TModel model, out string message);
        }

        public interface IReverter
        {
            TModel Revert(TModel defaultValue, TModel previousValue);
        }

        public ValueChangedDelegate<TModel> HandleModelChanged { get; set; }

        /// <summary>
        /// Default domain value.
        /// </summary>
        /// <value>The default model.</value>
        public TModel DefaultModel
        {
            get { return defaultModel; }
            set
            {
                SetProperty(ref defaultModel, value);

                if (!modelWasSet || !validator.IsValid(Model, out string error))
                {
                    Model = defaultModel;
                }
            }
        }

        /// <summary>
        /// Gets domain value
        /// </summary>
        /// <value>value. </value>
        public TModel Model
        {
            get { return model; }
            set
            {
                modelWasSet = true;

                if (SetProperty(ref model, value, out TModel old))
                {
                    View = converter.ConvertToView(model);
                    OnModelChangedInternal(model, old);
                }
            }
        }

        /// <summary>
        /// Gets value for presentation
        /// </summary>
        /// <value>value.</value>
        public TView View
        {
            get { return viewWasSet ? view : converter.ConvertToView(DefaultModel); }
            private set 
            {
                viewWasSet = true;
                SetProperty(ref view, value); 
            }
        }

        /// <summary>
        /// Gets a flag indicating whether last inputed value valid or not
        /// </summary>
        /// <value><c>true</c> if is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get { return isValid; }
            private set { SetProperty(ref isValid, value); }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get { return errorMessage; }
            private set { SetProperty(ref errorMessage, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inputed value should be validete instantly
        /// </summary>
        /// <value><c>true</c> if validate during editing; otherwise, <c>false</c>.</value>
        public bool ValidateDuringEditing { get; set; }

        /// <summary>
        /// Complete editing process
        /// </summary>
        public void EndEditing()
        {
            if (converter.TryConvertToModel(View, out TModel newModel, out string error))
            {
                SetValidModelOnly(newModel);
            }
            else
            {
                Model = reverter.Revert(DefaultModel, Model);

                ToInvalidState(error);
            }
        }

        /// <summary>
        /// Clear isValid and ErrorMessage
        /// </summary>
        public void ToValidState()
        {
            IsValid = true;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// IsValid = false
        /// ErrorMessage = errorMessage
        /// </summary>
        /// <param name="errorMessage">message.</param>
        public void ToInvalidState(string errorMessage)
        {
            IsValid = false;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Update value from view
        /// </summary>
        /// <param name="view">view value.</param>
        public void EditValue(TView view)
        {
            if (ValidateDuringEditing
                && (!converter.TryConvertToModel(view, out TModel testModel, out string error)
                    || !validator.IsValid(testModel, out error)))
            {
                ToInvalidState(error);
            }
            else
            {
                ToValidState();
            }

            View = view;
        }

        /// <summary>
        /// EditValue(view) + EndEditing()
        /// </summary>
        /// <param name="view">view value.</param>
        public void EditValueAndUpdateModel(TView view)
        {
            EditValue(view);
            EndEditing();
        }

        protected virtual void OnModelCnahged(TModel newValue, TModel oldValue)
        {
        }

        private void OnModelChangedInternal(TModel newValue, TModel oldValue)
        {
            HandleModelChanged?.Invoke(newValue, oldValue);
            OnModelCnahged(newValue, oldValue);
        }

        private void SetValidModelOnly(TModel newModel)
        {
            string error;

            if (validator.IsValid(newModel, out error))
            {
                Model = newModel;
                ToValidState();
            }
            else
            {
                Model = reverter.Revert(DefaultModel, Model);
                ToInvalidState(error);
            }
        }

        class ConverterAdapter : IConverter
        {
            readonly IValueConverter<TModel, TView> converter;
            readonly string errorMessage;

            public ConverterAdapter(IValueConverter<TModel, TView> converter, string errorMessage = null)
            {
                this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
                this.errorMessage = errorMessage;
            }

            public TView ConvertToView(TModel model)
            {
                return converter.Convert(model);
            }

            public bool TryConvertToModel(TView view, out TModel model, out string message)
            {
                try
                {
                    model = converter.ConvertBack(view);
                    message = string.Empty;

                    return true;
                }
                catch (Exception ex)
                {
                    model = default(TModel);
                    message = errorMessage;

                    return false;
                }
            }
        }

        class SuccessValidator : IValidator
        {
            public bool IsValid(TModel model, out string message)
            {
                message = string.Empty;
                return true;
            }
        }

        class PrevValueReverter : IReverter
        {
            public TModel Revert(TModel defaultValue, TModel previousValue)
            {
                return previousValue;
            }
        }
    }
}
