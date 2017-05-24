using System;
using UIKit;
using NLib.UI.MVVM;
using System.Threading.Tasks;
using NLib.UI.Bindings;

namespace NLib.iOS.Controllers
{
    public class ViewController<TViewModel> : UIViewController, IViewController<TViewModel>
        where TViewModel : ViewModel
    {
        ViewModelHolder holder;

        public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            holder.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            holder.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            holder.ViewWillDisappear(animated);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) holder?.Dispose();
            holder = null;

            base.Dispose(disposing);
        }

        class ViewModelHolder : IDisposable
        {
            public enum UpdateOrder
            {
                BeforeAppear,
                AfterAppear,
                AfterReload
            }

            readonly UIViewController owner;

            public ViewModelHolder(UIViewController owner)
            {
                this.owner = owner;
            }

            public UpdateOrder SetContextOrder { get; set; } = UpdateOrder.AfterAppear;

            public TViewModel ViewModel { get; set; } = null;

            public void ViewWillAppear(bool animated)
            {
                ViewModel = ViewModel ?? (TViewModel)Activator.CreateInstance(typeof(TViewModel)); //todo
                ViewModel.ViewWillAppearing();

                if (SetContextOrder == UpdateOrder.BeforeAppear)
                {
                    BindingStorage.SetContext(owner, ViewModel);
                }
            }

            public async void ViewDidAppear(bool animated)
            {
                await Task.Yield();

                if (SetContextOrder == UpdateOrder.AfterAppear)
                {
                    BindingStorage.SetContext(owner, ViewModel);
                }

                await ViewModel.ReloadDataAsync();

                if (SetContextOrder == UpdateOrder.AfterReload)
                {
                    BindingStorage.SetContext(owner, ViewModel);
                }
            }

            public void ViewWillDisappear(bool animated)
            {
                ViewModel?.ViewWillDisappear();
            }

            public void Dispose()
            {
                owner.ClearBindings();
                ViewModel = null;
            }
        }
    }
}
