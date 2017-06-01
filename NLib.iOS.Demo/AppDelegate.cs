using System;
using Foundation;
using UIKit;

namespace NLib.iOS.Demo
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate, IUISplitViewControllerDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            NLib.UI.ViewModelLocator.SetCurrentResolver(new SimpleResolver());

            // Override point for customization after application launch.
            var splitViewController = (UISplitViewController)Window.RootViewController;
            var navigationController = (UINavigationController)splitViewController.ViewControllers[1];
            navigationController.TopViewController.NavigationItem.LeftBarButtonItem = splitViewController.DisplayModeButtonItem;
            splitViewController.WeakDelegate = this;

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
        }

        public override void DidEnterBackground(UIApplication application)
        {
        }

        public override void WillEnterForeground(UIApplication application)
        {
        }

        public override void OnActivated(UIApplication application)
        {
        }

        public override void WillTerminate(UIApplication application)
        {
        }

        [Export("splitViewController:collapseSecondaryViewController:ontoPrimaryViewController:")]
        public bool CollapseSecondViewController(UISplitViewController splitViewController, UIViewController secondaryViewController, UIViewController primaryViewController)
        {
            if (secondaryViewController.GetType() == typeof(UINavigationController) &&
                ((UINavigationController)secondaryViewController).TopViewController.GetType() == typeof(DetailViewController) &&
                ((DetailViewController)((UINavigationController)secondaryViewController).TopViewController).DetailItem == null)
            {
                return true;
            }
            return false;
        }
    }

    class SimpleResolver : NLib.UI.IViewModelResolver
    {
        public TViewModel Resolve<TViewModel>()
        {
            return Activator.CreateInstance<TViewModel>();
        }
    }
}

