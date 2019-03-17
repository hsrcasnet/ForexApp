using ForexApp.Model;
using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class QuoteViewModel : BindableObject
    {
        private decimal price;
        private string symbol;

        public QuoteViewModel(QuoteDto quoteDto)
        {
            this.Update(quoteDto);
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

        public void Update(QuoteDto quoteDto)
        {
            this.Symbol = quoteDto.Symbol;
            this.Price = quoteDto.Price;
        }
    }
}