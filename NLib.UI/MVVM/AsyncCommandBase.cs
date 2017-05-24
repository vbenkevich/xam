using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NLib.UI
{
    public abstract class AsyncCommandBase : IExtendedCommand, IAsyncCommand
    {
        private readonly bool isSerial;
        private bool isExecuting;

        protected AsyncCommandBase(bool isSerial)
        {
            this.isSerial = isSerial;
        }

        public event EventHandler CanExecuteChanged;

        public async Task Execute(object parameter)
        {
            if (isSerial)
            {
                isExecuting = true;
                RaiseCanExecuteChanged();
            }

            try
            {
                await ExecuteImpl(parameter);
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

        async void ICommand.Execute(object parameter)
        {
            await Execute(parameter);
        }
            
        public bool CanExecute(object parameter)
        {
            return !(isSerial && isExecuting) && CanExecuteImpl(parameter);
        }

        protected abstract Task ExecuteImpl(object parameter);

        protected abstract bool CanExecuteImpl(object parameter);
    }
}
