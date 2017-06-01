using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace NLib.UI.Navigation
{
    public interface INavigtionStack
    {
        ViewModel TopViewModel { get; } 

        Task<TViewModel> PushAsync<TViewModel>(bool animate) where TViewModel : ViewModel;

        void Pop(bool animate);

        void Close(bool animate);
    }
}
