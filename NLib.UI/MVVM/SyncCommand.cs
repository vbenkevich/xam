using System;

namespace NLib.UI
{
    public class SyncCommand : SyncCommandBase
    {
        private readonly Action onExecuted;
        private readonly Func<bool> canExecute;

        public SyncCommand(Action onExecuted, Func<bool> canExecute = null, bool isSerial = true) : base(isSerial)
        {
            if (onExecuted == null)
                throw new ArgumentNullException(nameof(onExecuted));

            this.onExecuted = onExecuted;
            this.canExecute = canExecute;
        }

        protected override void ExecuteImpl(object parameter)
        {
            onExecuted.Invoke();
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return canExecute?.Invoke() != false;
        }
    }
}
