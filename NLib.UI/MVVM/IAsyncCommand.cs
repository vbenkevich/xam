using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NLib.UI
{
    public interface IAsyncCommand : ICommand
    {
        new Task Execute(object parameter);
    }
}
