using System;
using Foundation;
using UIKit;
using NLib.UI;

namespace NLib.iOS.Controllers
{
    public class ViewController<TViewModel> : UIViewController, IViewController<TViewModel> where TViewModel : ViewModel
    {
        readonly ViewModelWrapper<TViewModel> viewModelWrapper;

        public ViewController()
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public ViewController(IntPtr handle) : base(handle)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public ViewController(string nibNile, NSBundle bundle) : base(nibNile, bundle)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public ViewController(NSCoder coder) : base(coder)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public ViewController(NSObjectFlag flag) : base(flag)
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
