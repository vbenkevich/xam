using System;
using System.Threading.Tasks;

namespace NLib.UI
{
    public class AsyncCommand<T> : AsyncCommandBase
    {
        private readonly Func<T, Task> onExecuted;
        private readonly Func<T, bool> canExecute;

        public AsyncCommand(Func<T, Task> onExecuted, Func<T, bool> canExecute = null, bool isSerial = true) : base(isSerial)
        {
            if (onExecuted == null)
                throw new ArgumentNullException(nameof(onExecuted));

            this.onExecuted = onExecuted;
            this.canExecute = canExecute;
        }

        protected override Task ExecuteImpl(object parameter)
        {
            return onExecuted((T)parameter);
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return parameter is T && canExecute?.Invoke((T)parameter) != false;
        }
    }
}
