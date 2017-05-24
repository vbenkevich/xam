using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NLib.UI
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty<T>(ref T storage, T value, out T oldValue, [CallerMemberName] string propertyName = null)
        {
            oldValue = storage;

            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty(ref double storage, double value, [CallerMemberName] string propertyName = null)
        {
            if (Math.Abs(storage - value) < double.Epsilon) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty(ref double storage, double value, out double oldValue, [CallerMemberName] string propertyName = null)
        {
            oldValue = storage;

            if (Math.Abs(storage - value) < double.Epsilon) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty(ref float storage, float value, [CallerMemberName] string propertyName = null)
        {
            if (Math.Abs(storage - value) < float.Epsilon) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty(ref float storage, float value, out float oldValue, [CallerMemberName] string propertyName = null)
        {
            oldValue = storage;

            if (Math.Abs(storage - value) < float.Epsilon) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
