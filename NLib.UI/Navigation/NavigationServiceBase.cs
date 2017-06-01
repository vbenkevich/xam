using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NLib.UI.Navigation
{
    abstract class NavigationServiceBase : INavigationService
    {
        protected readonly Stack<INavigtionStack> navigation;

        public NavigationServiceBase()
        {
            navigation = new Stack<INavigtionStack>();
        }

        public Task<TViewModel> PushFrameAsync<TViewModel>(bool animate) where TViewModel : ViewModel
        {
            return navigation.Peek().PushAsync<TViewModel>(animate);
        }

        public async Task<TViewModel> PushRootAsync<TViewModel>(bool animate) where TViewModel : ViewModel
        {
            var newStack = await NewNaviagetionStack<TViewModel>();
            navigation.Push(newStack);

            return newStack.TopViewModel as TViewModel;
        }

        public Task<TViewModel> ShowDetailsAsync<TViewModel>(bool animate) where TViewModel : ViewModel
        {
            var currentDetailsPresenter = navigation.Peek() as IDetailsPresenter
                ?? throw new InvalidOperationException("IDetailsPresenter should be top navigation stack");

            return currentDetailsPresenter.ShowDetailsAsync<TViewModel>(animate);
        }

        public void PopFrame(bool animate)
        {
            navigation.Peek().Pop(animate);
        }

        public void PopRoot(bool animate)
        {
            navigation.Pop().Close(animate);
        }

        protected abstract Task<INavigtionStack> NewNaviagetionStack<TViewModel>();
    }
}
