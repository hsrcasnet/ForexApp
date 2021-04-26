using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ForexApp.Extensions;
using ForexApp.Model;
using ForexApp.Services;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ForexApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IForexSettings forexSettings;
        private readonly IForexService forexService;
        private readonly INavigationService navigationService;
        private readonly IPageDialogService pageDialogService;
        private string title;
        private string newQuoteSymbol;
        private bool isBusy;
        private bool isRefreshing;
        private ObservableCollection<QuoteViewModel> quotes;
        private ICommand selectQuoteCommand;

        public MainViewModel(
            IForexSettings forexSettings,
            IForexService forexService,
            INavigationService navigationService,
            IPageDialogService pageDialogService)
        {
            this.forexSettings = forexSettings;
            this.forexService = forexService;
            this.navigationService = navigationService;
            this.pageDialogService = pageDialogService;

            this.Title = "Welcome to ForexApp";
            this.Quotes = new ObservableCollection<QuoteViewModel>();
        }

        public string Title
        {
            get => this.title;
            private set => this.SetProperty(ref this.title, value);
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            this.RefreshButtonCommand.Execute(null);
        }

        private async Task LoadData()
        {
            if (!this.Quotes.Any())
            {
                var initialPairs = this.forexSettings.Symbols;
                await this.LoadAndUpdateQuotes(initialPairs);
            }
            else
            {
                var pairs = this.Quotes.Select(q => q.Symbol).ToArray();
                await this.LoadAndUpdateQuotes(pairs);
            }
        }

        public bool IsBusy
        {
            get => this.isBusy;
            private set => this.SetProperty(ref this.isBusy, value);
        }

        public bool IsRefreshing
        {
            get => this.isRefreshing;
            set => this.SetProperty(ref this.isRefreshing, value);
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
                this.Quotes.Add(new QuoteViewModel(quoteDto, this.OnQuoteDelete));
            }
            else
            {
                vm.Update(quoteDto);
            }
        }

        private void OnQuoteDelete(QuoteViewModel quoteViewModel)
        {
            this.Quotes.Remove(quoteViewModel);
        }

        public ICommand AddSymbolCommand => new Command(async () => await this.AddSymbol(), () => this.IsNewQuoteSymbolEnabled);

        private async Task AddSymbol()
        {
            var symbol = this.NewQuoteSymbol;

            // Get new market data + update UI
            var quoteDto = await this.forexService.GetQuote(symbol);
            if (quoteDto != null)
            {
                // Add new symbol to settings
                var symbols = this.forexSettings.Symbols.ToList();
                symbols.Add(symbol);
                this.forexSettings.Symbols = symbols.ToArray();

                this.AddOrUpdateQuote(quoteDto);
                this.RaisePropertyChanged(nameof(this.Quotes));
            }
            else
            {
                await this.pageDialogService.DisplayAlertAsync("Error", $"Failed to add currency pair '{symbol}'.", "OK");
            }

            this.NewQuoteSymbol = null;
        }

        public string NewQuoteSymbol
        {
            get => this.newQuoteSymbol;
            set
            {
                this.newQuoteSymbol = value?.ToUpperInvariant();
                this.RaisePropertyChanged(nameof(this.NewQuoteSymbol));
                this.RaisePropertyChanged(nameof(this.IsNewQuoteSymbolEnabled));
            }
        }

        public bool IsNewQuoteSymbolEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.NewQuoteSymbol);
            }
        }

        public ObservableCollection<QuoteViewModel> Quotes
        {
            get => this.quotes;
            set
            {
                this.quotes = value;
                this.RaisePropertyChanged(nameof(this.Quotes));
            }
        }

        //public ICommand SelectQuoteCommand => new Command(async () => await this.OnSelectQuote());

        public ICommand SelectQuoteCommand
        {
            get { return this.selectQuoteCommand ?? (this.selectQuoteCommand = new Command(async () => await this.OnSelectQuote())); }
        }

        public QuoteViewModel SelectedItem { get; set; }

        private async Task OnSelectQuote()
        {
            var navigationParameter = new NavigationParameters();
            navigationParameter.AddQuoteDetail(this.SelectedItem.Symbol);
            await this.navigationService.NavigateAsync(Pages.QuoteDetail, navigationParameter);
        }
    }
}