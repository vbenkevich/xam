using System;

namespace NLib.UI
{
    public abstract class SyncCommandBase : IExtendedCommand
    {
        private readonly bool isSerial;
        private bool isExecuting;

        protected SyncCommandBase(bool isSerial)
        {
            this.isSerial = isSerial;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (isSerial)
            {
                isExecuting = true;
                RaiseCanExecuteChanged();
            }

            try
            {
                ExecuteImpl(parameter);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return !(isSerial && isExecuting) && CanExecuteImpl(parameter);
        }

        protected abstract void ExecuteImpl(object parameter);

        protected abstract bool CanExecuteImpl(object parameter);
    }
}
