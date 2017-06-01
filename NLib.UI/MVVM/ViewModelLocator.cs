using System;

namespace NLib.UI
{
    public static class ViewModelLocator
    {
        private static IViewModelResolver currentResolver;

        public static void InitializeViewModelResolver(IViewModelResolver resolver = null)
        {
            currentResolver = resolver ?? new DefaultResolver();
        }

        public static TViewModel GetViewModel<TViewModel>(Action<TViewModel> initilizer = null) where TViewModel : ViewModel
        {
            if (currentResolver == null)
                throw new InvalidOperationException("IViewModelResolver should be set (ViewModelLocator.InitializeViewModelResolver())");

            var viewModel = currentResolver.Resolve<TViewModel>() 
                ?? throw new InvalidOperationException("IViewModelResolver.Resolve shouldn't return null");

            initilizer?.Invoke(viewModel);

            return viewModel;
        }

        public class DefaultResolver : IViewModelResolver
        {
            public TViewModel Resolve<TViewModel>()
            {
                return Activator.CreateInstance<TViewModel>();
            }
        }
    }
}
