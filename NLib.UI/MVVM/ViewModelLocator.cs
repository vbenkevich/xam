using System;

namespace NLib.UI
{
    public static class ViewModelLocator
    {
        private static IViewModelResolver currentResolver;

        public static void SetCurrentResolver(IViewModelResolver resolver)
        {
            currentResolver = resolver;
        }

        public static TViewModel GetViewModel<TViewModel>(Action<TViewModel> initilizer = null) where TViewModel : ViewModel
        {
            if (currentResolver == null)
                throw new InvalidOperationException("IViewModelResolver should be set (ViewModelLocator.SetCurrentResolver())");

            var viewModel = currentResolver.Resolve<TViewModel>() 
                ?? throw new InvalidOperationException("IViewModelResolver.Resolve shouldn't return null");

            return viewModel;
        }
    }
}
