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
            throw new NotImplementedException();
        }
    }
}
