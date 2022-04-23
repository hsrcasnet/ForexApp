using System;
using System.Windows.Input;
using ForexApp.Model;
using Prism.Mvvm;
using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class QuoteViewModel : BindableBase
    {
        private readonly Action<QuoteViewModel> deleteAction;
        private string symbol;
        private decimal price;
        private decimal? lastPrice;

        public QuoteViewModel(QuoteDto quoteDto, Action<QuoteViewModel> deleteAction)
        {
            this.deleteAction = deleteAction;
            this.Symbol = quoteDto.Symbol;
            this.Price = quoteDto.Price;
        }

        public string Symbol
        {
            get => this.symbol;
            private set => this.SetProperty(ref this.symbol, value);
        }

        public decimal Price
        {
            get => this.price;
            private set => this.SetProperty(ref this.price, value);
        }

        public decimal? LastPrice
        {
            get => this.lastPrice;
            private set
            {
                if (this.SetProperty(ref this.lastPrice, value))
                {
                    this.RaisePropertyChanged(nameof(this.Difference));
                }
            }
        }

        public string Difference
        {
            get
            {
                if (this.LastPrice == null)
                {
                    return null;
                }

                var difference = (this.Price - this.LastPrice) / this.LastPrice * 100;
                return $"{difference:0.0}%";
            }
        }

        public void Update(QuoteDto quoteDto)
        {
            this.Symbol = quoteDto.Symbol;
            this.LastPrice = this.Price;
            this.Price = quoteDto.Price;
        }

        public ICommand DeleteCommand => new Command(this.OnDelete);

        private void OnDelete()
        {
            this.deleteAction(this);
        }
    }
}