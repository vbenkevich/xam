using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NLib.UI
{
    public static class CommandHelper
    {
        public static async Task ExecuteAsync(this ICommand command, object parameter = null)
        {
            if (command?.CanExecute(parameter) != true)
            {
                return;
            }

            var asyncCommand = command as IAsyncCommand;
            if (asyncCommand != null)
            {
                await asyncCommand.Execute(parameter);
            }
            else
            {
                command.Execute(parameter);
            }
        }

        public static void TryRaiseCanExecuteChanged(this ICommand command)
        {
            (command as IExtendedCommand)?.RaiseCanExecuteChanged();
        }
    }
}
