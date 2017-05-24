// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace NLib.iOS.Demo
{
	[Register ("BindingsController")]
	partial class BindingsController
	{
		[Outlet]
		UIKit.UITextField priceField { get; set; }

		[Outlet]
		UIKit.UILabel refCounterLabel { get; set; }

		[Outlet]
		UIKit.UITextField sharesField { get; set; }

		[Outlet]
		UIKit.UILabel symbolLabel { get; set; }

		[Outlet]
		UIKit.UILabel totalLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (priceField != null) {
				priceField.Dispose ();
				priceField = null;
			}

			if (sharesField != null) {
				sharesField.Dispose ();
				sharesField = null;
			}

			if (symbolLabel != null) {
				symbolLabel.Dispose ();
				symbolLabel = null;
			}

			if (totalLabel != null) {
				totalLabel.Dispose ();
				totalLabel = null;
			}

			if (refCounterLabel != null) {
				refCounterLabel.Dispose ();
				refCounterLabel = null;
			}
		}
	}
}
