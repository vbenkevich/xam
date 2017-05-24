using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;

namespace NLib.UI.Tests
{
    public class PropertyChangedSubscriber : IDisposable
    {
        readonly INotifyPropertyChanged sender;
        readonly List<string> sequence;

        public PropertyChangedSubscriber(INotifyPropertyChanged sender)
        {
            this.sender = sender;
            this.sequence = new List<string>();

            sender.PropertyChanged += OnSenderPropertyChanged;
        }

        public string Last => sequence.Count > 0 ? sequence[sequence.Count - 1] : null;

        public IReadOnlyList<string> Sequence => sequence;

        public void Clear()
        {
            sequence.Clear();
        }

        public void Dispose()
        {
            sender.PropertyChanged -= OnSenderPropertyChanged;
        }

        private void OnSenderPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            sequence.Add(args.PropertyName);
        }
    }
}
