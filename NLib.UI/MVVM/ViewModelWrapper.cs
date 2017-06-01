using System;
using System.Threading.Tasks;
using NLib.UI.Bindings;

namespace NLib.UI
{
    public class ViewModelWrapper<TViewModel> : IDisposable
        where TViewModel : ViewModel
    {
        IViewController<TViewModel> owner;
        TViewModel viewModel;
        UpdateOrder setContextOrder;

        public ViewModelWrapper(IViewController<TViewModel> owner, UpdateOrder setContextOrder = UpdateOrder.AfterAppear)
        {
            this.owner = owner;
            this.setContextOrder = setContextOrder;

            State = ViewState.Unknown;
        }

        public ViewState State 
        { 
            get; 
            private set; 
        }

        public TViewModel ViewModel 
        {
            get => viewModel;
            set => SetViewModel(value);
        }

        public UpdateOrder AttachViewModelOrder 
        {
            get => setContextOrder;
            set => SetUpdateOrder(value);
        }

        public void ViewWillAppear()
        {
            viewModel = viewModel ?? ViewModelLocator.GetViewModel<TViewModel>();
            viewModel?.ViewWillAppearing();

            if (setContextOrder == UpdateOrder.BeforeAppear)
            {
                BindingStorage.SetContext(owner, ViewModel);
            }

            State = ViewState.Appearing;
        }

        public async void ViewDidAppear()
        {
            await Task.Yield();

            State = ViewState.Visible;

            if (setContextOrder == UpdateOrder.AfterAppear)
            {
                BindingStorage.SetContext(owner, ViewModel);
            }

            await ViewModel?.ReloadDataAsync();

            if (setContextOrder == UpdateOrder.AfterReload)
            {
                BindingStorage.SetContext(owner, ViewModel);
            }
        }

        public void ViewWillDisappear()
        {
            State = ViewState.Hidden;
            ViewModel?.ViewWillDisappear();
        }

        public void Dispose()
        {
            owner.ClearBindings();
            owner = null;
            ViewModel = null;
        }

        private void SetUpdateOrder(UpdateOrder value)
        {
            if (State == ViewState.Visible)
                throw new InvalidOperationException($"UpdateOrder={value} should be set before view appeared");

            if (State == ViewState.Appearing && value == UpdateOrder.BeforeAppear)
                throw new InvalidOperationException($"UpdateOrder={value} should be set before view beging appearing");

            setContextOrder = value;
        }

        private async void SetViewModel(TViewModel value)
        {
            if (viewModel == value) return;

            if (State == ViewState.Appearing || State == ViewState.Visible)
            {
                value?.ViewWillAppearing();
            }
            if (State == ViewState.Visible)
            {
                await value?.ReloadDataAsync();
            }

            viewModel = value;

            BindingStorage.SetContext(owner, value);
        }
    }
}
