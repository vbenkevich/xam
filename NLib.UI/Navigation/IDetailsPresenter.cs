using System;
using System.Threading.Tasks;

namespace NLib.UI.Navigation
{
    public interface IDetailsPresenter : INavigtionStack
    {
        Task<TViewModel> ShowDetailsAsync<TViewModel>(bool animate) where TViewModel : ViewModel;
    }
}
