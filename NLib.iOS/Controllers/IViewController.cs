using System;

namespace NLib.iOS.Controllers
{
    public interface IViewController<TViewModel>
    {
        void ViewWillAppear(bool animated);

        void ViewDidAppear(bool animated);

        void ViewWillDisappear(bool animated);

        void ViewDidDisappear(bool animated);
    }
}
