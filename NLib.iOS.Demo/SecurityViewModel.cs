using NLib.UI;
using NLib.UI.Bindings;

namespace NLib.iOS.Demo
{
    public class SecurityViewModel : ViewModel
    {
        public SecurityViewModel()
        {
            Shares = new SharesInput();
            Shares.DefaultModel = 11;
            Shares.HandleModelChanged = (newValue, oldValue) => TotalPrice = LastPrice * newValue;
        }

        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set { SetProperty(ref symbol, value); }
        }

        public SharesInput Shares { get; }

        private Price lastPrice;
        public Price LastPrice
        {
            get { return lastPrice; }
            set
            {
                if (SetProperty(ref lastPrice, value))
                {
                    TotalPrice = value * Shares.Model;
                }
            }
        }

        private Money totalPrice;
        public Money TotalPrice
        {
            get { return totalPrice; }
            private set
            {
                SetProperty(ref totalPrice, value);
            }
        }

        /// <summary>
        /// Incapsulate UI input logic (validation, delayed updates and other)
        /// </summary>
        public class SharesInput : InputViewModel<int, string>
        {
            public SharesInput() : base(Converters.IntStringWithExeption, new Validator())
            {
            }

            class Validator : IValidator
            {
                public bool IsValid(int model, out string message)
                {
                    message = "must be positive";
                    return model > 0;
                }
            }
        }
    }

    public struct Price
    {
        public double Value { get; set; }

        public Currency Currency { get; set; }

        public static Money operator *(Price price, int shares)
        {
            return new Money
            {
                Value = price.Value * shares,
                Currency = price.Currency
            };
        }
    }

    public class Money
    {
        public double Value { get; set; } = 0;

        public Currency Currency { get; set; }
    }

    public enum Currency
    {
        USD, EUR
    }
}
