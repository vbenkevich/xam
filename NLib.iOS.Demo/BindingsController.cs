using System;
using UIKit;
using NLib.iOS.Bindings;
using NLib.UI.Bindings;

namespace NLib.iOS.Demo
{
    public partial class BindingsController : UIViewController
    {
        public BindingsController(IntPtr handle) : base(handle)
        {
        }

        private SecurityViewModel viewModel;
        public SecurityViewModel ViewModel
        {
            get { return viewModel; }
            set
            {
                if (viewModel == value) return;
                viewModel = value;
                this.SetContext(viewModel);
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel = new SecurityViewModel();

            WeekRefCounter.Clear();
            refCounterLabel.Text = $"memory leaks: {WeekRefCounter.Count}";

            WeekRefCounter.Add(this);
            WeekRefCounter.Add(symbolLabel);
            WeekRefCounter.Add(sharesField);
            WeekRefCounter.Add(priceField);
            WeekRefCounter.Add(totalLabel);
            WeekRefCounter.Add(ViewModel);

            this.Bind(symbolLabel)
                .To<SecurityViewModel>(vm => vm.Symbol);

            this.Bind(totalLabel)
                .Converter(new MoneyToString())
                .To<SecurityViewModel>(vm => vm.TotalPrice);

            this.BindToInput(sharesField).To<SecurityViewModel>(vm => vm.Shares);
            
            this.Bind(priceField)
                .Converter(new PriceToString(Currency.USD))
                .To<SecurityViewModel>(vm => vm.LastPrice);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            ViewModel.Symbol = "AAPL";
        }

        class MoneyToString : IValueConverter<Money, string>
        {
            public string Convert(Money contextValue)
            {
                return contextValue == null ? "-" : contextValue.Value.ToString("C2");
            }

            public Money ConvertBack(string viewValue)
            {
                throw new NotImplementedException();
            }
        }

        class PriceToString : IValueConverter<Price, string>
        {
            private readonly Currency currency;

            public PriceToString(Currency currency)
            {
                this.currency = currency;
            }

            public string Convert(Price contextValue)
            {
                return contextValue.Value.ToString();
            }

            public Price ConvertBack(string viewValue)
            {
                var price = new Price
                {
                    Currency = currency,
                    Value = 0
                };

                double priceValue;

                if (double.TryParse(viewValue, out priceValue))
                {
                    price.Value = priceValue;
                }

                return price;
            }
        }
    }
}
