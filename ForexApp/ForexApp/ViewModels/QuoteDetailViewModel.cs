
using ForexApp.Extensions;

using Prism.Navigation;

namespace ForexApp.ViewModels
{
    public class QuoteDetailViewModel : ViewModelBase
    {
        private string symbol;

        public QuoteDetailViewModel()
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            this.Symbol = parameters.GetQuoteDetail();
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            private set
            {
                this.symbol = value;
                this.OnPropertyChanged(nameof(this.Symbol));
            }
        }
    }
}
