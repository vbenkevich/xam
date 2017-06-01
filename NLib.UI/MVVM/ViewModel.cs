using System;
using System.Threading.Tasks;

namespace NLib.UI
{
    public class ViewModel : ObservableObject
    {
        private static readonly Task completedTask = Task.FromResult(true);

        public ViewState State { get; internal set; }

        public virtual Task ReloadDataAsync()
        {
            return completedTask;
        }

        public virtual void ViewWillAppearing()
        {
        }

        public virtual void ViewWillDisappear()
        {
        }
    }
}
