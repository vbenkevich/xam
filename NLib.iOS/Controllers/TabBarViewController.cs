using System;
using UIKit;
using Foundation;
using NLib.UI;

namespace NLib.iOS.Controllers
{
    public class TabBarViewController<TViewModel> : UITabBarController, IViewController<TViewModel> where TViewModel : ViewModel
    {
        readonly ViewModelWrapper<TViewModel> viewModelWrapper;

        public TabBarViewController()
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TabBarViewController(IntPtr handle) : base(handle)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TabBarViewController(string nibNile, NSBundle bundle) : base(nibNile, bundle)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TabBarViewController(NSCoder coder) : base(coder)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TabBarViewController(NSObjectFlag flag) : base(flag)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TViewModel ViewModel
        {
            get => viewModelWrapper.ViewModel;
            set => viewModelWrapper.ViewModel = value;
        }

        ViewModel IViewController.ViewModel { get => ViewModel; }

        protected UpdateOrder AttachViewModelOrder
        {
            get => viewModelWrapper.AttachViewModelOrder;
            set => viewModelWrapper.AttachViewModelOrder = value;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            viewModelWrapper.ViewWillAppear();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            viewModelWrapper.ViewDidAppear();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            viewModelWrapper.ViewWillDisappear();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                viewModelWrapper?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

