using System;

namespace NLib.UI
{
    public interface IViewModelResolver
    {
        TViewModel Resolve<TViewModel>();
    }
}
