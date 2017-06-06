using System;
using System.Threading.Tasks;
using NLib.UI.Navigation;
using UIKit;

namespace NLib.iOS.Navigation
{
    class NavigationService : NavigationServiceBase
    {
        public static readonly NavigationService Instance = new NavigationService();

        public NavigationService()
        {
        }

        protected override Task<INavigtionStack> NewNaviagetionStack<TViewModel>()
        {
            if (NavigationMap.Instance.TryGetSegueId<TViewModel>(UIApplication.SharedApplication.KeyWindow.vi out string segueId))
            {
                
            }

            var newStack = new NLibNavigationController();
            newStack.TopViewController = 


            return newStack;
        }
    }
}
