using System;
using System.Threading.Tasks;

namespace NLib.UI.MVVM
{
    public class ViewModel : ObservableObject
    {
        private static readonly Task completedTask = Task.FromResult(true);

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
