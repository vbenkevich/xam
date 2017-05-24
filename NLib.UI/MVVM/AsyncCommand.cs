using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NLib.UI
{
    public class AsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> onExecuted;
        private readonly Func<bool> canExecute;

        public AsyncCommand(Func<Task> onExecuted, Func<bool> canExecute = null, bool isSerial = true) : base(isSerial)
        {
            if (onExecuted == null)
                throw new ArgumentNullException(nameof(onExecuted));

            this.onExecuted = onExecuted;
            this.canExecute = canExecute;
        }

        protected override Task ExecuteImpl(object parameter)
        {
            return onExecuted.Invoke();
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return canExecute?.Invoke() != false;
        }        
    }
}
