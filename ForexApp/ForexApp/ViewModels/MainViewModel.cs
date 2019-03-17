using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ForexApp.Model;
using ForexApp.Services;
using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class MainViewModel : BindableObject
    {
        private readonly IForexService forexService;
        private string title;
        private string newQuoteSymbol;
        private bool isBusy;
        private bool isRefreshing;

        public MainViewModel(IForexService forexService)
        {
            this.forexService = forexService;

            this.Title = "Welcome to ForexApp";
            this.Quotes = new ObservableCollection<QuoteViewModel>();
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.OnPropertyChanged(nameof(this.Title));
            }
        }

        public ICommand RefreshButtonCommand => new Command(
                                                    async () =>
                                                        {
                                                            this.IsBusy = true;
                                                            await this.LoadData();
                                                            this.IsBusy = false;
                                                        });

        public ICommand RefreshListCommand => new Command(
                                                  async () =>
                                                      {
                                                          this.IsRefreshing = true;
                                                          await this.LoadData();
                                                          this.IsRefreshing = false;
                                                      });

        private async Task LoadData()
        {
            if (!this.Quotes.Any())
            {
                var pairs = new[] { "EURCHF", "CHFEUR", "USDCHF", "CHFUSD" };
                await this.LoadAndUpdateQuotes(pairs);
            }
            else
            {
                var pairs = this.Quotes.Select(q => q.Symbol).ToArray();
                await this.LoadAndUpdateQuotes(pairs);
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                this.isBusy = value;
                this.OnPropertyChanged(nameof(this.IsBusy));
            }
        }

        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }
            set
            {
                this.isRefreshing = value;
                this.OnPropertyChanged(nameof(this.IsRefreshing));
            }
        }

        private async Task LoadAndUpdateQuotes(string[] pairs)
        {
            ICollection<QuoteDto> quotes = (await this.forexService.GetQuotes(pairs)).ToList();

            this.UpdateQuotes(quotes);
        }

        private void UpdateQuotes(ICollection<QuoteDto> quotes)
        {
            foreach (var quoteDto in quotes)
            {
                this.AddOrUpdateQuote(quoteDto);
            }

            var returnedSymbols = quotes.Select(dto => dto.Symbol);
            var unusedViewModels = this.Quotes.Where(vm => !returnedSymbols.Contains(vm.Symbol)).ToList();
            foreach (var quoteViewModel in unusedViewModels)
            {
                this.Quotes.Remove(quoteViewModel);
            }

            this.Quotes = new ObservableCollection<QuoteViewModel>(this.Quotes.OrderBy(q => q.Symbol));
        }

        private void AddOrUpdateQuote(QuoteDto quoteDto)
        {
            var vm = this.Quotes.SingleOrDefault(q => q.Symbol == quoteDto.Symbol);
            if (vm == null)
            {
                this.Quotes.Add(new QuoteViewModel(quoteDto));
            }
            else
            {
                vm.Update(quoteDto);
            }
        }

        public ICommand AddSymbolCommand => new Command(async () => await this.AddSymbol(), () => this.IsNewQuoteSymbolEnabled);

        private async Task AddSymbol()
        {
            var symbol = this.NewQuoteSymbol;

            var pairs = new[] { symbol };
            ICollection<QuoteDto> quoteDtos = (await this.forexService.GetQuotes(pairs)).ToList();
            var quoteDto = quoteDtos.Single();
            this.AddOrUpdateQuote(quoteDto);

            this.NewQuoteSymbol = null;
        }

        public string NewQuoteSymbol
        {
            get
            {
                return this.newQuoteSymbol;
            }
            set
            {
                this.newQuoteSymbol = value;
                this.OnPropertyChanged(nameof(this.NewQuoteSymbol));
                this.OnPropertyChanged(nameof(this.IsNewQuoteSymbolEnabled));
            }
        }

        public bool IsNewQuoteSymbolEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.NewQuoteSymbol);
            }
        }

        public ObservableCollection<QuoteViewModel> Quotes { get; set; }
    }
}