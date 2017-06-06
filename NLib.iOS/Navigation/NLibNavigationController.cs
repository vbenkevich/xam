﻿using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using NLib.UI.Navigation;
using NLib.UI;

namespace NLib.iOS.Navigation
{
    [Register("NLibNavigationController")]
    public class NLibNavigationController : UINavigationController, INavigtionStack, IUINavigationControllerDelegate
    {
        private TaskCompletionSource<UIViewController> currentTaskSource;

        #region ctor

        public NLibNavigationController()
        {
        }

        public NLibNavigationController(IntPtr handle) : base(handle)
        {
        }

        public NLibNavigationController(string nibNile, NSBundle bundle) : base(nibNile, bundle)
        {
        }

        public NLibNavigationController(NSCoder coder) : base(coder)
        {
        }

        public NLibNavigationController(NSObjectFlag flag) : base(flag)
        {
        }

        #endregion

        #region INavigtionStack

        public ViewModel TopViewModel => (TopViewController as IViewController).ViewModel;

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);
        }

        public async Task<TViewModel> PushAsync<TViewModel>(bool animate) where TViewModel : ViewModel
        {
            if (currentTaskSource != null)
                throw new InvalidOperationException("navigation is in progress already");

            var taskSource = currentTaskSource = new TaskCompletionSource<UIViewController>();

            if (NavigationMap.Instance.TryGetSegueId<TViewModel>(TopViewController, out string segueId))
            {
                TopViewController.PerformSegue(segueId, this);
            }
            else
            {
                PushViewController(NavigationMap.Instance.CreateViewController<TViewModel>(), animate);
            }

            var controller = await taskSource.Task;
            var vmcontroller = controller as IViewController<TViewModel>;

            return vmcontroller.ViewModel;
        }

        public void Pop(bool animate)
        {
            TopViewController.DismissViewController(animate, null);
        }

        public void Close(bool animate)
        {
            DismissViewController(animate, null);
        }

        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Delegate = this;
        }

        [Export("navigationController:didShowViewController:animated:")]
        public void DidShowViewController(UINavigationController navigationController, UIViewController viewController, bool animated)
        {
            if (currentTaskSource == null) return;

            currentTaskSource.SetResult(viewController);

            currentTaskSource = null;
        }
    }
}
