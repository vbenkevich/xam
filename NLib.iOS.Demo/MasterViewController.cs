using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using CoreGraphics;

namespace NLib.iOS.Demo
{
    public partial class MasterViewController : UITableViewController
    {
        protected MasterViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = NSBundle.MainBundle.LocalizedString("Master", "Master");

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                PreferredContentSize = new CGSize(320f, 600f);
                ClearsSelectionOnViewWillAppear = false;
            }
        }
    }
}

