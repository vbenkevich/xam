using System.Windows.Input;

namespace NLib.UI
{
    public interface IExtendedCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
