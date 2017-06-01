﻿using System;
using UIKit;
using Foundation;
using NLib.UI;

namespace NLib.iOS.Controllers
{
    public class TableViewController<TViewModel> : UITableViewController, IViewController<TViewModel> where TViewModel : ViewModel
    {
        readonly ViewModelWrapper<TViewModel> viewModelWrapper;

        public TableViewController()
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TableViewController(IntPtr handle) : base(handle)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TableViewController(string nibNile, NSBundle bundle) : base(nibNile, bundle)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TableViewController(NSCoder coder) : base(coder)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TableViewController(NSObjectFlag flag) : base(flag)
        {
            viewModelWrapper = new ViewModelWrapper<TViewModel>(this);
        }

        public TViewModel ViewModel
        {
            get => viewModelWrapper.ViewModel;
            set => viewModelWrapper.ViewModel = value;
        }

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
