using System;

namespace NLib.UI
{
    public class SyncCommand<T> : SyncCommandBase
    {
        private readonly Action<T> onExecuted;
        private readonly Func<T, bool> canExecute;

        public SyncCommand(Action<T> onExecuted, Func<T, bool> canExecute = null, bool isSerial = true) : base(isSerial)
        {
            if (onExecuted == null)
                throw new ArgumentNullException(nameof(onExecuted));

            this.onExecuted = onExecuted;
            this.canExecute = canExecute;
        }

        protected override void ExecuteImpl(object parameter)
        {
            onExecuted((T)parameter);
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return parameter is T && canExecute?.Invoke((T)parameter) != false;
        }
    }
}
