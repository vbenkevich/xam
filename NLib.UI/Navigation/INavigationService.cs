using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace NLib.UI.Navigation
{
    public interface INavigationService
    {
        /// <summary>
        /// Push new page to current navigation frame
        /// iOS - to top NLibNavigatinController
        /// Android - to top fragment navigation
        /// </summary>
        /// <returns>View model of the new page.</returns>
        /// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
        Task<TViewModel> PushFrameAsync<TViewModel>(bool animate) where TViewModel : ViewModel;

        /// <summary>
        /// Start new navigation frame of type: TViewModel
        /// iOS - push new NLibNavigatinController
        /// Android - start new Activity
        /// <returns>View model of the new page..</returns>
        /// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
        Task<TViewModel> PushRootAsync<TViewModel>(bool animate) where TViewModel : ViewModel;

        /// <summary>
        /// todo display in tab bar or MD
        /// </summary>
        /// <returns>View model of the new page.</returns>
        /// <typeparam name="TViewModel">The 1st type parameter.</typeparam>
        Task<TViewModel> ShowDetailsAsync<TViewModel>(bool animate) where TViewModel : ViewModel;

        /// <summary>
        /// Pop page from top navigation
        /// </summary>
        void PopFrame(bool animate);

        /// <summary>
        /// Pop top navigation
        /// iOS - pop form NLibNavigationController.ParentViewControlller.NavigationController
        /// Android - finish activity
        /// </summary>
        void PopRoot(bool animate);
    }
}
