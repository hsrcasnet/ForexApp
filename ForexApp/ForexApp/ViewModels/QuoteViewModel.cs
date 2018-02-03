using ForexApp.Model;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class QuoteViewModel : BindableObject
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
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
                this.OnPropertyChanged(nameof(this.Symbol));
            }
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }
            set
            {
                this.price = value;
                this.OnPropertyChanged(nameof(this.Price));
            }
        }

        public decimal? LastPrice
        {
            get
            {
                return this.lastPrice;
            }
            set
            {
                this.lastPrice = value;
                this.OnPropertyChanged(nameof(this.LastPrice));
                this.OnPropertyChanged(nameof(this.Difference));
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