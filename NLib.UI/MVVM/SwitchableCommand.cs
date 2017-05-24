using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NLib.UI
{
    public class SwitchableCommand : IAsyncCommand
    {
        private ICommand currentCommand;

        public SwitchableCommand(ICommand defaultCommand = null)
        {
            SetCommand(defaultCommand);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return currentCommand?.CanExecute(parameter) == true;
        }

        public void Execute(object parameter)
        {
            currentCommand.Execute(parameter);
        }

        public void SetCommand(ICommand command)
        {
            if (currentCommand != null)
            {
                currentCommand.CanExecuteChanged -= OnCurrentCommandCanExecuteCanhged;
            }

            currentCommand = command;

            if (currentCommand != null)
            {
                currentCommand.CanExecuteChanged += OnCurrentCommandCanExecuteCanhged;
            }

            OnCurrentCommandCanExecuteCanhged(this, EventArgs.Empty);
        }

        Task IAsyncCommand.Execute(object parameter)
        {
            return currentCommand.ExecuteAsync(parameter);
        }

        private void OnCurrentCommandCanExecuteCanhged(object sender, EventArgs args)
        {
            CanExecuteChanged?.Invoke(this, args);
        }
    }
}
